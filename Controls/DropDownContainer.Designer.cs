namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class DropDownContainer
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
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.itemcontainer = new Telerik.WinControls.UI.RadPanorama();
            ((System.ComponentModel.ISupportInitialize)(this.itemcontainer)).BeginInit();
            this.SuspendLayout();
            // 
            // itemcontainer
            // 
            this.itemcontainer.AllowDragDrop = false;
            this.itemcontainer.AutoScroll = true;
            this.itemcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemcontainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemcontainer.Location = new System.Drawing.Point(0, 0);
            this.itemcontainer.MouseWheelBehavior = Telerik.WinControls.UI.PanoramaMouseWheelBehavior.Scroll;
            this.itemcontainer.Name = "itemcontainer";
            this.itemcontainer.ScrollBarThickness = 10;
            this.itemcontainer.Size = new System.Drawing.Size(520, 115);
            this.itemcontainer.TabIndex = 0;
            this.itemcontainer.Text = "radPanorama1";
            this.itemcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // DropDownContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.itemcontainer);
            this.DoubleBuffered = true;
            this.Name = "DropDownContainer";
            this.Size = new System.Drawing.Size(520, 115);
            ((System.ComponentModel.ISupportInitialize)(this.itemcontainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadPanorama itemcontainer;

    }
}
