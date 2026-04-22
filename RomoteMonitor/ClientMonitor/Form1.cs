using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMonitor
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private NetworkStream _ns;
        private CancellationTokenSource _cts;
        private Task _sendTask;
        private bool _previewEnabled = true;
        private int _intervalMs = 1000; // default 1s
        private long _jpegQuality = 55L;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Ready";
            txtPort.Text = "9000";
            btnDisconnect.Enabled = false;
            btnTogglePreview.Text = "Turn Preview Off";

            // quality
            cbQuality.Items.Clear();
            cbQuality.Items.Add("Low (40%)");
            cbQuality.Items.Add("Medium (65%)");
            cbQuality.Items.Add("High (90%)");
            cbQuality.SelectedIndex = 1;

            // fps
            cbFPS.Items.Clear();
            cbFPS.Items.Add("500 ms");
            cbFPS.Items.Add("1000 ms");
            cbFPS.Items.Add("2000 ms");
            cbFPS.SelectedIndex = 1;
        }

        private void cbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbQuality.SelectedIndex)
            {
                case 0: _jpegQuality = 40L; break;
                case 1: _jpegQuality = 65L; break;
                case 2: _jpegQuality = 90L; break;
            }
        }

        private void cbFPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFPS.SelectedIndex)
            {
                case 0: _intervalMs = 500; break;
                case 1: _intervalMs = 1000; break;
                case 2: _intervalMs = 2000; break;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            StartClient();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            StopClient();
        }

        private void btnTogglePreview_Click(object sender, EventArgs e)
        {
            _previewEnabled = !_previewEnabled;
            btnTogglePreview.Text = _previewEnabled ? "Turn Preview Off" : "Turn Preview On";
            if (!_previewEnabled)
            {
                if (picPreview.Image != null)
                {
                    var old = picPreview.Image;
                    picPreview.Image = null;
                    old.Dispose();
                }
            }
        }

        private void StartClient()
        {
            if (_sendTask != null && !_sendTask.IsCompleted) return;

            try
            {
                string ip = txtIP.Text.Trim();
                if (!int.TryParse(txtPort.Text.Trim(), out int port)) port = 9000;

                _client = new TcpClient();
                _client.Connect(ip, port);
                _ns = _client.GetStream();
                _cts = new CancellationTokenSource();

                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus.Text = "Connected";
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                // set initial quality & fps from comboboxes
                cbQuality_SelectedIndexChanged(null, null);
                cbFPS_SelectedIndexChanged(null, null);

                _sendTask = Task.Run(() => SendLoopAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect: " + ex.Message);
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Connect failed";
            }
        }

        private async Task SendLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _client?.Connected == true)
                {
                    Rectangle bounds = Screen.PrimaryScreen.Bounds;
                    using (var bmp = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (var g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                        }

                        if (_previewEnabled)
                        {
                            var preview = (Image)bmp.Clone();
                            this.InvokeIfRequired(() =>
                            {
                                if (picPreview.Image != null)
                                {
                                    var old = picPreview.Image;
                                    picPreview.Image = null;
                                    old.Dispose();
                                }
                                picPreview.Image = preview;
                            });
                        }

                        using (var ms = new MemoryStream())
                        {
                            var encoder = GetEncoder(ImageFormat.Jpeg);
                            if (encoder != null)
                            {
                                var ep = new EncoderParameters(1);
                                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _jpegQuality);
                                bmp.Save(ms, encoder, ep);
                            }
                            else
                            {
                                bmp.Save(ms, ImageFormat.Jpeg);
                            }

                            byte[] data = ms.ToArray();
                            byte[] len = BitConverter.GetBytes(data.Length);

                            await _ns.WriteAsync(len, 0, 4, token).ConfigureAwait(false);
                            await _ns.WriteAsync(data, 0, data.Length, token).ConfigureAwait(false);
                        }
                    }

                    await Task.Delay(_intervalMs, token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException) { /* cancelled */ }
            catch (Exception)
            {
                this.InvokeIfRequired(() =>
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Disconnected";
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                });
            }
        }

        private void StopClient()
        {
            try
            {
                _cts?.Cancel();
                _ns?.Close();
                _client?.Close();
            }
            catch { }

            this.InvokeIfRequired(() =>
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                lblStatus.ForeColor = Color.Gray;
                lblStatus.Text = "Stopped";
            });

            if (picPreview.Image != null)
            {
                var old = picPreview.Image;
                picPreview.Image = null;
                old.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var c in codecs)
                if (c.FormatID == format.Guid) return c;
            return null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopClient();
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
