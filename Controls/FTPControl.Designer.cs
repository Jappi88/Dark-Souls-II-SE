namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class FTPControl
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
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.xconnect = new Telerik.WinControls.UI.RadButton();
            this.xport = new Telerik.WinControls.UI.RadSpinEditor();
            this.xpass = new Telerik.WinControls.UI.RadTextBox();
            this.xusername = new Telerik.WinControls.UI.RadTextBox();
            this.xip = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.xsavecontainer = new Telerik.WinControls.UI.RadListControl();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xconnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xusername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xsavecontainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.xconnect);
            this.radPanel1.Controls.Add(this.xport);
            this.radPanel1.Controls.Add(this.xpass);
            this.radPanel1.Controls.Add(this.xusername);
            this.radPanel1.Controls.Add(this.xip);
            this.radPanel1.Controls.Add(this.radLabel4);
            this.radPanel1.Controls.Add(this.radLabel3);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(246, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(314, 121);
            this.radPanel1.TabIndex = 0;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // xconnect
            // 
            this.xconnect.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xconnect.Location = new System.Drawing.Point(185, 92);
            this.xconnect.Name = "xconnect";
            this.xconnect.Size = new System.Drawing.Size(105, 25);
            this.xconnect.TabIndex = 8;
            this.xconnect.Text = "&Connect";
            this.xconnect.ThemeName = "VisualStudio2012Dark";
            this.xconnect.Click += new System.EventHandler(this.xconnect_Click);
            // 
            // xport
            // 
            this.xport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xport.Location = new System.Drawing.Point(92, 93);
            this.xport.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.xport.MinimumSize = new System.Drawing.Size(0, 24);
            this.xport.Name = "xport";
            // 
            // 
            // 
            this.xport.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xport.Size = new System.Drawing.Size(87, 24);
            this.xport.TabIndex = 7;
            this.xport.TabStop = false;
            this.xport.ThemeName = "VisualStudio2012Dark";
            // 
            // xpass
            // 
            this.xpass.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpass.Location = new System.Drawing.Point(92, 63);
            this.xpass.MinimumSize = new System.Drawing.Size(0, 24);
            this.xpass.Name = "xpass";
            // 
            // 
            // 
            this.xpass.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xpass.Size = new System.Drawing.Size(198, 24);
            this.xpass.TabIndex = 6;
            this.xpass.ThemeName = "VisualStudio2012Dark";
            // 
            // xusername
            // 
            this.xusername.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xusername.Location = new System.Drawing.Point(92, 33);
            this.xusername.MinimumSize = new System.Drawing.Size(0, 24);
            this.xusername.Name = "xusername";
            // 
            // 
            // 
            this.xusername.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xusername.Size = new System.Drawing.Size(198, 24);
            this.xusername.TabIndex = 5;
            this.xusername.ThemeName = "VisualStudio2012Dark";
            // 
            // xip
            // 
            this.xip.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.xip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xip.Location = new System.Drawing.Point(92, 3);
            this.xip.MinimumSize = new System.Drawing.Size(0, 24);
            this.xip.Name = "xip";
            // 
            // 
            // 
            this.xip.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xip.Size = new System.Drawing.Size(198, 24);
            this.xip.TabIndex = 4;
            this.xip.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel4
            // 
            this.radLabel4.AutoSize = false;
            this.radLabel4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel4.Location = new System.Drawing.Point(3, 93);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(83, 22);
            this.radLabel4.TabIndex = 3;
            this.radLabel4.Text = "Port";
            this.radLabel4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel4.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel3
            // 
            this.radLabel3.AutoSize = false;
            this.radLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel3.Location = new System.Drawing.Point(3, 63);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(83, 24);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "Password";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel3.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel2
            // 
            this.radLabel2.AutoSize = false;
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.Location = new System.Drawing.Point(3, 33);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(83, 24);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "Username";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel2.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(83, 24);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "IP Address";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel1.ThemeName = "VisualStudio2012Dark";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(807, 127);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // xsavecontainer
            // 
            this.xsavecontainer.AutoScroll = true;
            this.xsavecontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xsavecontainer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xsavecontainer.Location = new System.Drawing.Point(0, 160);
            this.xsavecontainer.Name = "xsavecontainer";
            this.xsavecontainer.Size = new System.Drawing.Size(807, 341);
            this.xsavecontainer.TabIndex = 3;
            this.xsavecontainer.Text = "radListControl1";
            // 
            // radLabel5
            // 
            this.radLabel5.AutoSize = false;
            this.radLabel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.radLabel5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel5.Location = new System.Drawing.Point(0, 0);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(807, 33);
            this.radLabel5.TabIndex = 4;
            this.radLabel5.Text = "Load DSII SaveGame From FTP Device";
            this.radLabel5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radLabel5.ThemeName = "VisualStudio2012Dark";
            // 
            // FTPControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xsavecontainer);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radLabel5);
            this.DoubleBuffered = true;
            this.Name = "FTPControl";
            this.Size = new System.Drawing.Size(807, 501);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xconnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xusername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xsavecontainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton xconnect;
        private Telerik.WinControls.UI.RadSpinEditor xport;
        private Telerik.WinControls.UI.RadTextBox xpass;
        private Telerik.WinControls.UI.RadTextBox xusername;
        private Telerik.WinControls.UI.RadTextBox xip;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadListControl xsavecontainer;
        private Telerik.WinControls.UI.RadLabel radLabel5;
    }
}
