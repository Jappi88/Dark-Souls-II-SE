namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class USBManager
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
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.xscan = new Telerik.WinControls.UI.RadButton();
            this.xcontainer = new Telerik.WinControls.UI.RadListControl();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xscan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel5
            // 
            this.radLabel5.AutoSize = false;
            this.radLabel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.radLabel5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel5.Location = new System.Drawing.Point(0, 0);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(575, 33);
            this.radLabel5.TabIndex = 5;
            this.radLabel5.Text = "Load DSII SaveGame From USB Device";
            this.radLabel5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radLabel5.ThemeName = "VisualStudio2012Dark";
            // 
            // xscan
            // 
            this.xscan.Dock = System.Windows.Forms.DockStyle.Top;
            this.xscan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xscan.Location = new System.Drawing.Point(0, 33);
            this.xscan.Name = "xscan";
            this.xscan.Size = new System.Drawing.Size(575, 38);
            this.xscan.TabIndex = 6;
            this.xscan.Text = "Search For DSII SaveGames";
            this.xscan.ThemeName = "VisualStudio2012Dark";
            this.xscan.Click += new System.EventHandler(this.xscan_Click);
            // 
            // xcontainer
            // 
            this.xcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcontainer.Location = new System.Drawing.Point(0, 71);
            this.xcontainer.Name = "xcontainer";
            this.xcontainer.Size = new System.Drawing.Size(575, 320);
            this.xcontainer.TabIndex = 7;
            this.xcontainer.Text = "xsavecontainer";
            this.xcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // USBManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xcontainer);
            this.Controls.Add(this.xscan);
            this.Controls.Add(this.radLabel5);
            this.DoubleBuffered = true;
            this.Name = "USBManager";
            this.Size = new System.Drawing.Size(575, 391);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xscan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadButton xscan;
        private Telerik.WinControls.UI.RadListControl xcontainer;
    }
}
