namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class SlotSelector
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
            this.xslotcontainer = new Telerik.WinControls.UI.RadListControl();
            ((System.ComponentModel.ISupportInitialize)(this.xslotcontainer)).BeginInit();
            this.SuspendLayout();
            // 
            // xslotcontainer
            // 
            this.xslotcontainer.AutoScroll = true;
            this.xslotcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xslotcontainer.ItemHeight = 100;
            this.xslotcontainer.Location = new System.Drawing.Point(0, 0);
            this.xslotcontainer.Name = "xslotcontainer";
            this.xslotcontainer.Size = new System.Drawing.Size(434, 330);
            this.xslotcontainer.TabIndex = 0;
            this.xslotcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // SlotSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xslotcontainer);
            this.DoubleBuffered = true;
            this.Name = "SlotSelector";
            this.Size = new System.Drawing.Size(434, 330);
            ((System.ComponentModel.ISupportInitialize)(this.xslotcontainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadListControl xslotcontainer;

    }
}
