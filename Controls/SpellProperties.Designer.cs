namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class SpellProperties
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
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.xspelldescription = new Telerik.WinControls.UI.RadLabel();
            this.ximage = new System.Windows.Forms.PictureBox();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.xname = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.xslotused = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.xvalue2 = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.xvalue1 = new Telerik.WinControls.UI.RadSpinEditor();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.radPanel3 = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xspelldescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xslotused)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xvalue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xvalue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).BeginInit();
            this.radPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radGridView1);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox1.HeaderText = "Spell Uses Per Level";
            this.radGroupBox1.Location = new System.Drawing.Point(158, 0);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(270, 338);
            this.radGroupBox1.TabIndex = 2;
            this.radGroupBox1.Text = "Spell Uses Per Level";
            // 
            // radGridView1
            // 
            this.radGridView1.AutoScroll = true;
            this.radGridView1.AutoSizeRows = true;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(2, 18);
            // 
            // radGridView1
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnChooser = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnReorder = false;
            this.radGridView1.MasterTemplate.AllowColumnResize = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.AllowRowResize = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(266, 318);
            this.radGridView1.TabIndex = 0;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ValueChanging += new Telerik.WinControls.UI.ValueChangingEventHandler(this.radGridView1_ValueChanging);
            // 
            // xspelldescription
            // 
            this.xspelldescription.AutoSize = false;
            this.xspelldescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xspelldescription.Location = new System.Drawing.Point(0, 25);
            this.xspelldescription.Name = "xspelldescription";
            this.xspelldescription.Size = new System.Drawing.Size(328, 148);
            this.xspelldescription.TabIndex = 0;
            this.xspelldescription.Text = "description";
            // 
            // ximage
            // 
            this.ximage.Dock = System.Windows.Forms.DockStyle.Right;
            this.ximage.Location = new System.Drawing.Point(328, 0);
            this.ximage.Name = "ximage";
            this.ximage.Size = new System.Drawing.Size(100, 173);
            this.ximage.TabIndex = 1;
            this.ximage.TabStop = false;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.xspelldescription);
            this.radPanel1.Controls.Add(this.xname);
            this.radPanel1.Controls.Add(this.ximage);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(428, 173);
            this.radPanel1.TabIndex = 2;
            // 
            // xname
            // 
            this.xname.AutoSize = false;
            this.xname.Dock = System.Windows.Forms.DockStyle.Top;
            this.xname.Location = new System.Drawing.Point(0, 0);
            this.xname.Name = "xname";
            this.xname.Size = new System.Drawing.Size(328, 25);
            this.xname.TabIndex = 1;
            this.xname.Text = "name";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(3, 70);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(59, 18);
            this.radLabel3.TabIndex = 8;
            this.radLabel3.Text = "Slot Used :";
            // 
            // xslotused
            // 
            this.xslotused.Location = new System.Drawing.Point(64, 68);
            this.xslotused.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.xslotused.Name = "xslotused";
            this.xslotused.Size = new System.Drawing.Size(84, 20);
            this.xslotused.TabIndex = 7;
            this.xslotused.TabStop = false;
            this.xslotused.ValueChanged += new System.EventHandler(this.xslotused_ValueChanged);
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(3, 44);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(46, 18);
            this.radLabel2.TabIndex = 6;
            this.radLabel2.Text = "Value2 :";
            // 
            // xvalue2
            // 
            this.xvalue2.Location = new System.Drawing.Point(64, 42);
            this.xvalue2.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.xvalue2.Name = "xvalue2";
            this.xvalue2.Size = new System.Drawing.Size(84, 20);
            this.xvalue2.TabIndex = 5;
            this.xvalue2.TabStop = false;
            this.xvalue2.ValueChanged += new System.EventHandler(this.xvalue2_ValueChanged);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 18);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(46, 18);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "Value1 :";
            // 
            // xvalue1
            // 
            this.xvalue1.Location = new System.Drawing.Point(64, 16);
            this.xvalue1.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.xvalue1.Name = "xvalue1";
            this.xvalue1.Size = new System.Drawing.Size(84, 20);
            this.xvalue1.TabIndex = 2;
            this.xvalue1.TabStop = false;
            this.xvalue1.ValueChanged += new System.EventHandler(this.xvalue1_ValueChanged);
            // 
            // radPanel2
            // 
            this.radPanel2.Controls.Add(this.radGroupBox1);
            this.radPanel2.Controls.Add(this.radPanel3);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel2.Location = new System.Drawing.Point(0, 173);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(428, 338);
            this.radPanel2.TabIndex = 3;
            // 
            // radPanel3
            // 
            this.radPanel3.Controls.Add(this.radLabel1);
            this.radPanel3.Controls.Add(this.radLabel2);
            this.radPanel3.Controls.Add(this.radLabel3);
            this.radPanel3.Controls.Add(this.xvalue2);
            this.radPanel3.Controls.Add(this.xslotused);
            this.radPanel3.Controls.Add(this.xvalue1);
            this.radPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.radPanel3.Location = new System.Drawing.Point(0, 0);
            this.radPanel3.Name = "radPanel3";
            this.radPanel3.Size = new System.Drawing.Size(158, 338);
            this.radPanel3.TabIndex = 9;
            // 
            // SpellProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radPanel2);
            this.Controls.Add(this.radPanel1);
            this.DoubleBuffered = true;
            this.Name = "SpellProperties";
            this.Size = new System.Drawing.Size(428, 511);
            this.Resize += new System.EventHandler(this.SpellProperties_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xspelldescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ximage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xslotused)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xvalue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xvalue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).EndInit();
            this.radPanel3.ResumeLayout(false);
            this.radPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel xspelldescription;
        private System.Windows.Forms.PictureBox ximage;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadSpinEditor xslotused;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadSpinEditor xvalue2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadSpinEditor xvalue1;
        private Telerik.WinControls.UI.RadLabel xname;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadPanel radPanel3;

    }
}
