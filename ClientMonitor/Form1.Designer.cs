namespace ClientMonitor
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnTogglePreview;
        private System.Windows.Forms.ComboBox cbQuality;
        private System.Windows.Forms.ComboBox cbFPS;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnTogglePreview = new System.Windows.Forms.Button();
            this.cbQuality = new System.Windows.Forms.ComboBox();
            this.cbFPS = new System.Windows.Forms.ComboBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(20, 64);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(200, 22);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "127.0.0.1";
            this.txtIP.BackColor = System.Drawing.Color.FromArgb(40, 44, 70);
            this.txtIP.ForeColor = System.Drawing.Color.White;
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(230, 64);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(80, 22);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "9000";
            this.txtPort.BackColor = System.Drawing.Color.FromArgb(40, 44, 70);
            this.txtPort.ForeColor = System.Drawing.Color.White;
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(316, 60);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 30);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(0, 180, 120);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(412, 60);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(90, 30);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.BackColor = System.Drawing.Color.FromArgb(200, 60, 60);
            this.btnDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.FlatAppearance.BorderSize = 0;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnTogglePreview
            // 
            this.btnTogglePreview.Location = new System.Drawing.Point(508, 60);
            this.btnTogglePreview.Name = "btnTogglePreview";
            this.btnTogglePreview.Size = new System.Drawing.Size(110, 30);
            this.btnTogglePreview.TabIndex = 4;
            this.btnTogglePreview.Text = "Turn Preview Off";
            this.btnTogglePreview.BackColor = System.Drawing.Color.FromArgb(80, 80, 160);
            this.btnTogglePreview.ForeColor = System.Drawing.Color.White;
            this.btnTogglePreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePreview.FlatAppearance.BorderSize = 0;
            this.btnTogglePreview.Click += new System.EventHandler(this.btnTogglePreview_Click);
            // 
            // cbQuality
            // 
            this.cbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQuality.Location = new System.Drawing.Point(20, 100);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(200, 24);
            this.cbQuality.TabIndex = 5;
            this.cbQuality.BackColor = System.Drawing.Color.FromArgb(40, 44, 70);
            this.cbQuality.ForeColor = System.Drawing.Color.White;
            this.cbQuality.SelectedIndexChanged += new System.EventHandler(this.cbQuality_SelectedIndexChanged);
            // 
            // cbFPS
            // 
            this.cbFPS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFPS.Location = new System.Drawing.Point(230, 100);
            this.cbFPS.Name = "cbFPS";
            this.cbFPS.Size = new System.Drawing.Size(150, 24);
            this.cbFPS.TabIndex = 6;
            this.cbFPS.BackColor = System.Drawing.Color.FromArgb(40, 44, 70);
            this.cbFPS.ForeColor = System.Drawing.Color.White;
            this.cbFPS.SelectedIndexChanged += new System.EventHandler(this.cbFPS_SelectedIndexChanged);
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.Location = new System.Drawing.Point(20, 140);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(610, 320);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 7;
            this.picPreview.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 475);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(610, 24);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status: ---";
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(0, 230, 180);
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(34, 40, 80);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 230, 180);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(650, 48);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "CLIENT - GỬI ẢNH MÀN HÌNH (1 CLIENT)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(650, 520);
            this.BackColor = System.Drawing.Color.FromArgb(24, 28, 50);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnTogglePreview);
            this.Controls.Add(this.cbQuality);
            this.Controls.Add(this.cbFPS);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form1";
            this.Text = "Client Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}