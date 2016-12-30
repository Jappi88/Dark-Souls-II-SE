namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class PreventBan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreventBan));
            this.radTextBoxControl1 = new Telerik.WinControls.UI.RadTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxControl1
            // 
            this.radTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTextBoxControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTextBoxControl1.IsReadOnly = true;
            this.radTextBoxControl1.Location = new System.Drawing.Point(0, 0);
            this.radTextBoxControl1.Multiline = true;
            this.radTextBoxControl1.Name = "radTextBoxControl1";
            this.radTextBoxControl1.Size = new System.Drawing.Size(763, 168);
            this.radTextBoxControl1.TabIndex = 0;
            this.radTextBoxControl1.Text = resources.GetString("radTextBoxControl1.Text");
            this.radTextBoxControl1.ThemeName = "VisualStudio2012Dark";
            // 
            // PreventBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radTextBoxControl1);
            this.DoubleBuffered = true;
            this.Name = "PreventBan";
            this.Size = new System.Drawing.Size(763, 168);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControl1;
    }
}
