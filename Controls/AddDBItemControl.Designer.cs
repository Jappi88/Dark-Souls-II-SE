namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class AddDBItemControl
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
            this.xname = new Telerik.WinControls.UI.RadTextBox();
            this.xid = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.xtypes = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.xadd = new Telerik.WinControls.UI.RadButton();
            this.xsetimage = new Telerik.WinControls.UI.RadButton();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.ximage = new System.Windows.Forms.PictureBox();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.xdescription = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.xname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xadd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xsetimage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).BeginInit();
            this.SuspendLayout();
            // 
            // xname
            // 
            this.xname.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xname.Location = new System.Drawing.Point(104, 3);
            this.xname.MinimumSize = new System.Drawing.Size(0, 24);
            this.xname.Name = "xname";
            // 
            // 
            // 
            this.xname.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xname.Size = new System.Drawing.Size(347, 24);
            this.xname.TabIndex = 1;
            this.xname.Text = "Enter Name..";
            this.xname.ThemeName = "VisualStudio2012Dark";
            this.xname.Enter += new System.EventHandler(this.xname_Enter);
            this.xname.Leave += new System.EventHandler(this.xname_Leave);
            // 
            // xid
            // 
            this.xid.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xid.Location = new System.Drawing.Point(222, 26);
            this.xid.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.xid.MinimumSize = new System.Drawing.Size(0, 24);
            this.xid.Name = "xid";
            // 
            // 
            // 
            this.xid.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xid.Size = new System.Drawing.Size(229, 24);
            this.xid.TabIndex = 4;
            this.xid.TabStop = false;
            this.xid.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.xid.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel2
            // 
            this.radLabel2.AutoSize = false;
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.Location = new System.Drawing.Point(104, 26);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(112, 22);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "Enter Item ID";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel2.ThemeName = "VisualStudio2012Dark";
            // 
            // xtypes
            // 
            this.xtypes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xtypes.Location = new System.Drawing.Point(222, 50);
            this.xtypes.Name = "xtypes";
            this.xtypes.Size = new System.Drawing.Size(229, 27);
            this.xtypes.TabIndex = 6;
            this.xtypes.Text = "Select Catagory";
            this.xtypes.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel3
            // 
            this.radLabel3.AutoSize = false;
            this.radLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel3.Location = new System.Drawing.Point(104, 50);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(112, 22);
            this.radLabel3.TabIndex = 7;
            this.radLabel3.Text = "Choose Catagory";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel3.ThemeName = "VisualStudio2012Dark";
            // 
            // xadd
            // 
            this.xadd.BackColor = System.Drawing.Color.Transparent;
            this.xadd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xadd.Location = new System.Drawing.Point(222, 73);
            this.xadd.Name = "xadd";
            this.xadd.Size = new System.Drawing.Size(229, 24);
            this.xadd.TabIndex = 8;
            this.xadd.Text = "&Add Item";
            this.xadd.ThemeName = "VisualStudio2012Dark";
            this.xadd.Click += new System.EventHandler(this.xadd_Click);
            // 
            // xsetimage
            // 
            this.xsetimage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xsetimage.Location = new System.Drawing.Point(105, 73);
            this.xsetimage.Name = "xsetimage";
            this.xsetimage.Size = new System.Drawing.Size(111, 24);
            this.xsetimage.TabIndex = 9;
            this.xsetimage.Text = "&Choose Item Image";
            this.xsetimage.ThemeName = "VisualStudio2012Dark";
            this.xsetimage.Click += new System.EventHandler(this.xsetimage_Click);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.xname);
            this.radPanel1.Controls.Add(this.xsetimage);
            this.radPanel1.Controls.Add(this.ximage);
            this.radPanel1.Controls.Add(this.xadd);
            this.radPanel1.Controls.Add(this.xid);
            this.radPanel1.Controls.Add(this.radLabel3);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.xtypes);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(756, 100);
            this.radPanel1.TabIndex = 10;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // ximage
            // 
            this.ximage.Dock = System.Windows.Forms.DockStyle.Left;
            this.ximage.Image = global::Dark_Souls_II_Save_Editor.Properties.Resources.unknown;
            this.ximage.Location = new System.Drawing.Point(0, 0);
            this.ximage.Name = "ximage";
            this.ximage.Size = new System.Drawing.Size(100, 100);
            this.ximage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ximage.TabIndex = 3;
            this.ximage.TabStop = false;
            // 
            // xdescription
            // 
            this.xdescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.xdescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdescription.Location = new System.Drawing.Point(0, 100);
            this.xdescription.MinimumSize = new System.Drawing.Size(0, 24);
            this.xdescription.Name = "xdescription";
            // 
            // 
            // 
            this.xdescription.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.xdescription.Size = new System.Drawing.Size(756, 24);
            this.xdescription.TabIndex = 11;
            this.xdescription.Text = "Enter Description..";
            this.xdescription.ThemeName = "VisualStudio2012Dark";
            this.xdescription.Enter += new System.EventHandler(this.xdescription_Enter);
            this.xdescription.Leave += new System.EventHandler(this.xdescription_Leave);
            // 
            // AddDBItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xdescription);
            this.Controls.Add(this.radPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AddDBItemControl";
            this.Size = new System.Drawing.Size(756, 122);
            ((System.ComponentModel.ISupportInitialize)(this.xname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xadd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xsetimage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdescription)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox xname;
        private System.Windows.Forms.PictureBox ximage;
        private Telerik.WinControls.UI.RadSpinEditor xid;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadDropDownList xtypes;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadButton xadd;
        private Telerik.WinControls.UI.RadButton xsetimage;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadTextBox xdescription;
    }
}
