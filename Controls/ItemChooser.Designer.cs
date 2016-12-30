namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class ItemChooser
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
            this.itemcontainer = new Telerik.WinControls.UI.RadPanorama();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemcontainer)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.itemcontainer);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(383, 98);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.ThemeName = "TelerikMetroBlue";
            // 
            // itemcontainer
            // 
            this.itemcontainer.AllowDragDrop = false;
            this.itemcontainer.AutoScroll = true;
            this.itemcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemcontainer.Location = new System.Drawing.Point(0, 0);
            this.itemcontainer.MouseWheelBehavior = Telerik.WinControls.UI.PanoramaMouseWheelBehavior.Scroll;
            this.itemcontainer.Name = "itemcontainer";
            this.itemcontainer.ScrollBarThickness = 20;
            this.itemcontainer.Size = new System.Drawing.Size(383, 98);
            this.itemcontainer.TabIndex = 1;
            this.itemcontainer.Text = "radPanorama1";
            this.itemcontainer.ThemeName = "VisualStudio2012Dark";
            this.itemcontainer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.itemcontainer_MouseClick);
            // 
            // ItemChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanel1);
            this.Name = "ItemChooser";
            this.Size = new System.Drawing.Size(383, 98);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itemcontainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadPanorama itemcontainer;
    }
}
