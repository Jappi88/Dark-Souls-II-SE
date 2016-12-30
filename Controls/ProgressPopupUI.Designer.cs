namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class ProgressPopupUI
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
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.radProgressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radProgressBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radProgressBar1.Location = new System.Drawing.Point(40, 0);
            this.radProgressBar1.Name = "radProgressBar1";
            // 
            // 
            // 
            this.radProgressBar1.RootElement.ApplyShapeToControl = true;
            this.radProgressBar1.RootElement.ClipDrawing = false;
            this.radProgressBar1.SeparatorColor1 = System.Drawing.Color.Maroon;
            this.radProgressBar1.SeparatorColor2 = System.Drawing.Color.Maroon;
            this.radProgressBar1.SeparatorColor3 = System.Drawing.Color.Black;
            this.radProgressBar1.SeparatorColor4 = System.Drawing.Color.Black;
            this.radProgressBar1.Size = new System.Drawing.Size(814, 50);
            this.radProgressBar1.StepWidth = 10;
            this.radProgressBar1.TabIndex = 0;
            this.radProgressBar1.ThemeName = "TelerikMetroBlue";
            this.radProgressBar1.Value1 = 100;
            // 
            // radButton1
            // 
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radButton1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(0, 0);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(40, 50);
            this.radButton1.TabIndex = 1;
            this.radButton1.Text = ">>";
            this.radButton1.ThemeName = "VisualStudio2012Dark";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButton1.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButton1.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Mainpage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.radProgressBar1);
            this.Controls.Add(this.radButton1);
            this.DoubleBuffered = true;
            this.Name = "Mainpage";
            this.Size = new System.Drawing.Size(854, 50);
            this.SizeChanged += new System.EventHandler(this.MainPage_SizeChanged);
            this.Resize += new System.EventHandler(this.MainPage_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}
