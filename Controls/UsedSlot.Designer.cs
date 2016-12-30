namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class UsedSlot
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
            this.xdelete = new Telerik.WinControls.UI.RadButton();
            this.xreplace = new Telerik.WinControls.UI.RadButton();
            this.xextract = new Telerik.WinControls.UI.RadButton();
            this.xload = new Telerik.WinControls.UI.RadButton();
            this.xdescription = new Telerik.WinControls.UI.RadLabel();
            this.ximage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xdelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xreplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xextract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.xdelete);
            this.radPanel1.Controls.Add(this.xreplace);
            this.radPanel1.Controls.Add(this.xextract);
            this.radPanel1.Controls.Add(this.xload);
            this.radPanel1.Controls.Add(this.xdescription);
            this.radPanel1.Controls.Add(this.ximage);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(0, 100);
            this.radPanel1.TabIndex = 0;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // xdelete
            // 
            this.xdelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.xdelete.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdelete.Location = new System.Drawing.Point(400, 75);
            this.xdelete.Name = "xdelete";
            this.xdelete.Size = new System.Drawing.Size(0, 25);
            this.xdelete.TabIndex = 4;
            this.xdelete.Text = "Delete Character";
            this.xdelete.ThemeName = "VisualStudio2012Dark";
            this.xdelete.Click += new System.EventHandler(this.xdelete_Click);
            // 
            // xreplace
            // 
            this.xreplace.Dock = System.Windows.Forms.DockStyle.Top;
            this.xreplace.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xreplace.Location = new System.Drawing.Point(400, 50);
            this.xreplace.Name = "xreplace";
            this.xreplace.Size = new System.Drawing.Size(0, 25);
            this.xreplace.TabIndex = 3;
            this.xreplace.Text = "Replace Character";
            this.xreplace.ThemeName = "VisualStudio2012Dark";
            this.xreplace.Click += new System.EventHandler(this.xreplace_Click);
            // 
            // xextract
            // 
            this.xextract.Dock = System.Windows.Forms.DockStyle.Top;
            this.xextract.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xextract.Location = new System.Drawing.Point(400, 25);
            this.xextract.Name = "xextract";
            this.xextract.Size = new System.Drawing.Size(0, 25);
            this.xextract.TabIndex = 2;
            this.xextract.Text = "Extract Character";
            this.xextract.ThemeName = "VisualStudio2012Dark";
            this.xextract.Click += new System.EventHandler(this.xextract_Click);
            // 
            // xload
            // 
            this.xload.Dock = System.Windows.Forms.DockStyle.Top;
            this.xload.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xload.Location = new System.Drawing.Point(400, 0);
            this.xload.Name = "xload";
            this.xload.Size = new System.Drawing.Size(0, 25);
            this.xload.TabIndex = 5;
            this.xload.Text = "Load Character";
            this.xload.ThemeName = "VisualStudio2012Dark";
            this.xload.Click += new System.EventHandler(this.xload_Click);
            // 
            // xdescription
            // 
            this.xdescription.AutoSize = false;
            this.xdescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.xdescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdescription.Location = new System.Drawing.Point(128, 0);
            this.xdescription.Name = "xdescription";
            this.xdescription.Size = new System.Drawing.Size(272, 100);
            this.xdescription.TabIndex = 1;
            this.xdescription.ThemeName = "VisualStudio2012Dark";
            this.xdescription.DoubleClick += new System.EventHandler(this.UsedSlot_DoubleClick);
            // 
            // ximage
            // 
            this.ximage.Dock = System.Windows.Forms.DockStyle.Left;
            this.ximage.Image = global::Dark_Souls_II_Save_Editor.Properties.Resources.Icon;
            this.ximage.Location = new System.Drawing.Point(0, 0);
            this.ximage.Name = "ximage";
            this.ximage.Size = new System.Drawing.Size(128, 100);
            this.ximage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ximage.TabIndex = 0;
            this.ximage.TabStop = false;
            this.ximage.DoubleClick += new System.EventHandler(this.UsedSlot_DoubleClick);
            // 
            // UsedSlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(0, 100);
            this.MinimumSize = new System.Drawing.Size(0, 100);
            this.Name = "UsedSlot";
            this.Size = new System.Drawing.Size(0, 100);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xdelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xreplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xextract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton xdelete;
        private Telerik.WinControls.UI.RadButton xreplace;
        private Telerik.WinControls.UI.RadButton xextract;
        private Telerik.WinControls.UI.RadLabel xdescription;
        private System.Windows.Forms.PictureBox ximage;
        private Telerik.WinControls.UI.RadButton xload;

    }
}
