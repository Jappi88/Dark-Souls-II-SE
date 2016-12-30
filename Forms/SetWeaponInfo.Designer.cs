namespace Dark_Souls_II_Save_Editor.Forms
{
    partial class SetWeaponInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.weaponStatEditor1 = new Dark_Souls_II_Save_Editor.Controls.WeaponStatEditor();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // weaponStatEditor1
            // 
            this.weaponStatEditor1.BackColor = System.Drawing.Color.Transparent;
            this.weaponStatEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weaponStatEditor1.Location = new System.Drawing.Point(0, 0);
            this.weaponStatEditor1.Name = "weaponStatEditor1";
            this.weaponStatEditor1.Size = new System.Drawing.Size(568, 236);
            this.weaponStatEditor1.Slot = null;
            this.weaponStatEditor1.TabIndex = 0;
            // 
            // SetWeaponInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 236);
            this.Controls.Add(this.weaponStatEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SetWeaponInfo";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetWeaponInfo";
            this.ThemeName = "VisualStudio2012Dark";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal Controls.WeaponStatEditor weaponStatEditor1;

    }
}