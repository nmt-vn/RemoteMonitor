using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerMonitor
{
    public partial class Form1 : Form
    {
        private TcpListener _listener;
        private CancellationTokenSource _cts;
        private const int PORT = 9000;
        private bool _clientConnected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Ready";
            btnStop.Enabled = false;

            // populate view modes
            cbViewMode.Items.Clear();
            cbViewMode.Items.Add("Zoom (no distortion)");
            cbViewMode.Items.Add("Stretch (may distort)");
            cbViewMode.Items.Add("Center (actual size)");
            cbViewMode.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void cbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbViewMode.SelectedIndex)
            {
                case 0:
                    pictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 1:
                    pictureBoxMain.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case 2:
                    pictureBoxMain.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                default:
                    pictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
            }
        }

        private void StartServer()
        {
            if (_listener != null) return;

            try
            {
                _cts = new CancellationTokenSource();
                _listener = new TcpListener(IPAddress.Any, PORT);
                _listener.Start();

                lblStatus.ForeColor = Color.DarkGreen;
                lblStatus.Text = $"Listening on port {PORT}";
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                Task.Run(() => AcceptLoopAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Start server error: " + ex.Message);
            }
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var tcp = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);

                    // single-client mode: if already connected, close the new one immediately
                    if (_clientConnected)
                    {
                        try { tcp.Close(); } catch { }
                        continue;
                    }

                    _clientConnected = true;
                    // handle receive loop
                    await Task.Run(() => ReceiveLoopAsync(tcp, token));
                    // when finished, allow next client
                    _clientConnected = false;
                }
            }
            catch (ObjectDisposedException) { /* listener stopped */ }
            catch (Exception ex)
            {
                this.InvokeIfRequired(() =>
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Accept error: " + ex.Message;
                });
            }
        }

        private async Task ReceiveLoopAsync(TcpClient tcp, CancellationToken token)
        {
            string remote = tcp.Client.RemoteEndPoint?.ToString() ?? "client";
            this.InvokeIfRequired(() =>
            {
                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus.Text = $"Connected: {remote}";
            });

            try
            {
                using (tcp)
                using (var ns = tcp.GetStream())
                {
                    ns.ReadTimeout = 10000;
                    while (!token.IsCancellationRequested && tcp.Connected)
                    {
                        byte[] lenBuf = new byte[4];
                        bool ok = await ReadExactAsync(ns, lenBuf, 0, 4, token).ConfigureAwait(false);
                        if (!ok) break;
                        int imgLen = BitConverter.ToInt32(lenBuf, 0);
                        if (imgLen <= 0 || imgLen > 50 * 1024 * 1024) break;

                        byte[] imgBuf = new byte[imgLen];
                        ok = await ReadExactAsync(ns, imgBuf, 0, imgLen, token).ConfigureAwait(false);
                        if (!ok) break;

                        try
                        {
                            using (var ms = new MemoryStream(imgBuf))
                            using (var img = Image.FromStream(ms))
                            {
                                var clone = (Image)img.Clone();
                                this.InvokeIfRequired(() =>
                                {
                                    if (pictureBoxMain.Image != null)
                                    {
                                        var old = pictureBoxMain.Image;
                                        pictureBoxMain.Image = null;
                                        old.Dispose();
                                    }
                                    pictureBoxMain.Image = clone;
                                    lblLastFrame.Text = DateTime.Now.ToString("HH:mm:ss") + " - " + remote;
                                });
                            }
                        }
                        catch
                        {
                            // ignore single-frame decode errors
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.InvokeIfRequired(() =>
                {
                    lblStatus.ForeColor = Color.OrangeRed;
                    lblStatus.Text = "Receive error: " + ex.Message;
                });
            }
            finally
            {
                this.InvokeIfRequired(() =>
                {
                    lblStatus.ForeColor = Color.Gray;
                    lblStatus.Text = "Client disconnected";
                });
            }
        }

        private async Task<bool> ReadExactAsync(Stream s, byte[] buffer, int offset, int count, CancellationToken token)
        {
            int read = 0;
            while (read < count)
            {
                if (token.IsCancellationRequested) return false;
                int r;
                try
                {
                    r = await s.ReadAsync(buffer, offset + read, count - read, token).ConfigureAwait(false);
                }
                catch (IOException) { return false; }
                catch (ObjectDisposedException) { return false; }
                if (r == 0) return false;
                read += r;
            }
            return true;
        }

        private void StopServer()
        {
            if (_listener == null) return;

            try
            {
                _cts?.Cancel();
                _listener.Stop();
                _listener = null;

                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lblStatus.ForeColor = Color.Gray;
                lblStatus.Text = "Stopped";

                if (pictureBoxMain.Image != null)
                {
                    var img = pictureBoxMain.Image;
                    pictureBoxMain.Image = null;
                    img.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop error: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }

    // helper
    public static class ControlExtensions
    {
        public static void InvokeIfRequired(this Control c, Action a)
        {
            if (c == null) return;
            if (c.InvokeRequired) c.Invoke(a);
            else a();
        }
    }
}
