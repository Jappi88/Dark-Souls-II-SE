namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class FileMain
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xcontainer = new Telerik.WinControls.UI.RadPanel();
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.xloadxbox = new Telerik.WinControls.UI.RadTileElement();
            this.xloadusb = new Telerik.WinControls.UI.RadTileElement();
            this.xloadftp = new Telerik.WinControls.UI.RadTileElement();
            this.xloadps3 = new Telerik.WinControls.UI.RadTileElement();
            this.xsavefile = new Telerik.WinControls.UI.RadTileElement();
            this.xclosefile = new Telerik.WinControls.UI.RadTileElement();
            this.xselectslot = new Telerik.WinControls.UI.RadTileElement();
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            this.SuspendLayout();
            // 
            // xcontainer
            // 
            this.xcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcontainer.Location = new System.Drawing.Point(273, 0);
            this.xcontainer.Name = "xcontainer";
            this.xcontainer.Size = new System.Drawing.Size(640, 460);
            this.xcontainer.TabIndex = 1;
            this.xcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // radPanorama1
            // 
            this.radPanorama1.AllowDragDrop = false;
            this.radPanorama1.AutoScroll = true;
            this.radPanorama1.CellSize = new System.Drawing.Size(250, 60);
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radPanorama1.EnableZooming = false;
            this.radPanorama1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.xloadxbox,
            this.xloadusb,
            this.xloadftp,
            this.xloadps3,
            this.xselectslot,
            this.xsavefile,
            this.xclosefile});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.MouseWheelBehavior = Telerik.WinControls.UI.PanoramaMouseWheelBehavior.Scroll;
            this.radPanorama1.Name = "radPanorama1";
            // 
            // 
            // 
            this.radPanorama1.RootElement.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radPanorama1.RootElement.AutoSize = false;
            this.radPanorama1.RootElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radPanorama1.RowsCount = 7;
            this.radPanorama1.ScrollBarThickness = 12;
            this.radPanorama1.Size = new System.Drawing.Size(273, 460);
            this.radPanorama1.TabIndex = 0;
            this.radPanorama1.ThemeName = "VisualStudio2012Dark";
            // 
            // xloadxbox
            // 
            this.xloadxbox.AccessibleDescription = "Load XBOX360 Save";
            this.xloadxbox.AccessibleName = "Load XBOX360 Save";
            this.xloadxbox.Name = "xloadxbox";
            this.xloadxbox.ShowHorizontalLine = false;
            this.xloadxbox.StretchHorizontally = true;
            this.xloadxbox.StretchVertically = true;
            this.xloadxbox.Text = "Load XBOX360 Save";
            this.xloadxbox.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xloadxbox.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xloadxbox.Click += new System.EventHandler(this.xloadxbox_Click);
            // 
            // xloadusb
            // 
            this.xloadusb.AccessibleDescription = "Load Save From USB";
            this.xloadusb.AccessibleName = "Load Save From USB";
            this.xloadusb.Name = "xloadusb";
            this.xloadusb.Row = 1;
            this.xloadusb.Text = "Load Save From USB";
            this.xloadusb.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xloadusb.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xloadusb.Click += new System.EventHandler(this.xloadusb_Click);
            // 
            // xloadftp
            // 
            this.xloadftp.AccessibleDescription = "Load Save From FTP";
            this.xloadftp.AccessibleName = "Load Save From FTP";
            this.xloadftp.Name = "xloadftp";
            this.xloadftp.Row = 2;
            this.xloadftp.Text = "Load Save From FTP";
            this.xloadftp.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xloadftp.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xloadftp.Click += new System.EventHandler(this.xloadftp_Click);
            // 
            // xloadps3
            // 
            this.xloadps3.AccessibleDescription = "Load PS3 Save Directory";
            this.xloadps3.AccessibleName = "Load PS3 Save Directory";
            this.xloadps3.Name = "xloadps3";
            this.xloadps3.Row = 3;
            this.xloadps3.Text = "Load PS3 Save Directory";
            this.xloadps3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xloadps3.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xloadps3.Click += new System.EventHandler(this.xloadps3_Click);
            // 
            // xsavefile
            // 
            this.xsavefile.AccessibleDescription = "Close File";
            this.xsavefile.AccessibleName = "Close File";
            this.xsavefile.Name = "xsavefile";
            this.xsavefile.Row = 5;
            this.xsavefile.Text = "Save All Slots";
            this.xsavefile.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xsavefile.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.xsavefile.Click += new System.EventHandler(this.xsavefile_Click);
            // 
            // xclosefile
            // 
            this.xclosefile.AccessibleDescription = "Close File";
            this.xclosefile.AccessibleName = "Close File";
            this.xclosefile.Name = "xclosefile";
            this.xclosefile.Row = 6;
            this.xclosefile.Text = "Close Save";
            this.xclosefile.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xclosefile.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.xclosefile.Click += new System.EventHandler(this.xclosefile_Click);
            // 
            // xselectslot
            // 
            this.xselectslot.AccessibleDescription = "Select Slot";
            this.xselectslot.AccessibleName = "Select Slot";
            this.xselectslot.Name = "xselectslot";
            this.xselectslot.Row = 4;
            this.xselectslot.Text = "Select Slot";
            this.xselectslot.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xselectslot.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.xselectslot.Click += new System.EventHandler(this.xselectslot_Click);
            // 
            // FileMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xcontainer);
            this.Controls.Add(this.radPanorama1);
            this.DoubleBuffered = true;
            this.Name = "FileMain";
            this.Size = new System.Drawing.Size(913, 460);
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadTileElement xloadxbox;
        private Telerik.WinControls.UI.RadTileElement xloadusb;
        private Telerik.WinControls.UI.RadTileElement xloadps3;
        private Telerik.WinControls.UI.RadPanel xcontainer;
        private Telerik.WinControls.UI.RadTileElement xsavefile;
        private Telerik.WinControls.UI.RadTileElement xclosefile;
        private Telerik.WinControls.UI.RadTileElement xloadftp;
        private Telerik.WinControls.UI.RadTileElement xselectslot;
    }
}
