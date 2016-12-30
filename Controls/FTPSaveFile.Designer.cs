namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class FTPSaveFile
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
            this.xdescription = new Telerik.WinControls.UI.RadLabel();
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.xdelete = new Telerik.WinControls.UI.RadButton();
            this.xextract = new Telerik.WinControls.UI.RadButton();
            this.xreplace = new Telerik.WinControls.UI.RadButton();
            this.xload = new Telerik.WinControls.UI.RadButton();
            this.ximage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).BeginInit();
            this.xdescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xdelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xextract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xreplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.xdescription);
            this.radPanel1.Controls.Add(this.radPanel2);
            this.radPanel1.Controls.Add(this.ximage);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(795, 100);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // xdescription
            // 
            this.xdescription.AutoSize = false;
            this.xdescription.Controls.Add(this.radProgressBar1);
            this.xdescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xdescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdescription.Location = new System.Drawing.Point(128, 0);
            this.xdescription.Name = "xdescription";
            this.xdescription.Size = new System.Drawing.Size(485, 100);
            this.xdescription.TabIndex = 1;
            this.xdescription.ThemeName = "VisualStudio2012Dark";
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.radProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radProgressBar1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radProgressBar1.Location = new System.Drawing.Point(0, 75);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.Size = new System.Drawing.Size(485, 25);
            this.radProgressBar1.TabIndex = 0;
            // 
            // radPanel2
            // 
            this.radPanel2.BackColor = System.Drawing.Color.Transparent;
            this.radPanel2.Controls.Add(this.xdelete);
            this.radPanel2.Controls.Add(this.xreplace);
            this.radPanel2.Controls.Add(this.xextract);
            this.radPanel2.Controls.Add(this.xload);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanel2.Location = new System.Drawing.Point(613, 0);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(182, 100);
            this.radPanel2.TabIndex = 6;
            // 
            // xdelete
            // 
            this.xdelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.xdelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdelete.Location = new System.Drawing.Point(0, 75);
            this.xdelete.Name = "xdelete";
            this.xdelete.Size = new System.Drawing.Size(182, 25);
            this.xdelete.TabIndex = 4;
            this.xdelete.Text = "Delete";
            this.xdelete.ThemeName = "VisualStudio2012Dark";
            this.xdelete.Click += new System.EventHandler(this.xdelete_Click);
            // 
            // xextract
            // 
            this.xextract.Dock = System.Windows.Forms.DockStyle.Top;
            this.xextract.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xextract.Location = new System.Drawing.Point(0, 25);
            this.xextract.Name = "xextract";
            this.xextract.Size = new System.Drawing.Size(182, 25);
            this.xextract.TabIndex = 2;
            this.xextract.Text = "Extract";
            this.xextract.ThemeName = "VisualStudio2012Dark";
            this.xextract.Click += new System.EventHandler(this.xextract_Click);
            // 
            // xreplace
            // 
            this.xreplace.Dock = System.Windows.Forms.DockStyle.Top;
            this.xreplace.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xreplace.Location = new System.Drawing.Point(0, 50);
            this.xreplace.Name = "xreplace";
            this.xreplace.Size = new System.Drawing.Size(182, 25);
            this.xreplace.TabIndex = 3;
            this.xreplace.Text = "Replace";
            this.xreplace.ThemeName = "VisualStudio2012Dark";
            this.xreplace.Click += new System.EventHandler(this.xreplace_Click);
            // 
            // xload
            // 
            this.xload.Dock = System.Windows.Forms.DockStyle.Top;
            this.xload.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xload.Location = new System.Drawing.Point(0, 0);
            this.xload.Name = "xload";
            this.xload.Size = new System.Drawing.Size(182, 25);
            this.xload.TabIndex = 5;
            this.xload.Text = "Load";
            this.xload.ThemeName = "VisualStudio2012Dark";
            this.xload.Click += new System.EventHandler(this.xload_Click);
            // 
            // ximage
            // 
            this.ximage.Dock = System.Windows.Forms.DockStyle.Left;
            this.ximage.Image = global::Dark_Souls_II_Save_Editor.Properties.Resources.darksouls2;
            this.ximage.Location = new System.Drawing.Point(0, 0);
            this.ximage.Name = "ximage";
            this.ximage.Size = new System.Drawing.Size(128, 100);
            this.ximage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ximage.TabIndex = 0;
            this.ximage.TabStop = false;
            // 
            // FTPSaveFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanel1);
            this.DoubleBuffered = true;
            this.Name = "FTPSaveFile";
            this.Size = new System.Drawing.Size(795, 100);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).EndInit();
            this.xdescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xdelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xextract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xreplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton xdelete;
        private Telerik.WinControls.UI.RadButton xreplace;
        private Telerik.WinControls.UI.RadButton xextract;
        private Telerik.WinControls.UI.RadButton xload;
        private Telerik.WinControls.UI.RadLabel xdescription;
        private System.Windows.Forms.PictureBox ximage;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
    }
}
