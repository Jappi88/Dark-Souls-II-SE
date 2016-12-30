namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class SpellParamViewer
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
            this.radSplitContainer1 = new Telerik.WinControls.UI.RadSplitContainer();
            this.splitPanel1 = new Telerik.WinControls.UI.SplitPanel();
            this.xitems = new Telerik.WinControls.UI.RadListView();
            this.splitPanel2 = new Telerik.WinControls.UI.SplitPanel();
            this.spellProperties1 = new Dark_Souls_II_Save_Editor.Controls.SpellProperties();
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer1)).BeginInit();
            this.radSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).BeginInit();
            this.splitPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xitems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).BeginInit();
            this.splitPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radSplitContainer1
            // 
            this.radSplitContainer1.AutoScroll = true;
            this.radSplitContainer1.Controls.Add(this.splitPanel1);
            this.radSplitContainer1.Controls.Add(this.splitPanel2);
            this.radSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.radSplitContainer1.Name = "radSplitContainer1";
            // 
            // 
            // 
            this.radSplitContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radSplitContainer1.Size = new System.Drawing.Size(703, 495);
            this.radSplitContainer1.SplitterWidth = 4;
            this.radSplitContainer1.TabIndex = 0;
            this.radSplitContainer1.TabStop = false;
            this.radSplitContainer1.Text = "radSplitContainer1";
            // 
            // splitPanel1
            // 
            this.splitPanel1.Controls.Add(this.xitems);
            this.splitPanel1.Location = new System.Drawing.Point(0, 0);
            this.splitPanel1.Name = "splitPanel1";
            // 
            // 
            // 
            this.splitPanel1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.splitPanel1.Size = new System.Drawing.Size(283, 495);
            this.splitPanel1.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(-0.0951359F, 0F);
            this.splitPanel1.SizeInfo.SplitterCorrection = new System.Drawing.Size(-76, 0);
            this.splitPanel1.TabIndex = 0;
            this.splitPanel1.TabStop = false;
            this.splitPanel1.Text = "splitPanel1";
            // 
            // xitems
            // 
            this.xitems.AllowColumnReorder = false;
            this.xitems.AllowEdit = false;
            this.xitems.AllowRemove = false;
            this.xitems.AutoScroll = true;
            this.xitems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xitems.ItemSize = new System.Drawing.Size(200, 100);
            this.xitems.ItemSpacing = 2;
            this.xitems.Location = new System.Drawing.Point(0, 0);
            this.xitems.Name = "xitems";
            this.xitems.Size = new System.Drawing.Size(283, 495);
            this.xitems.TabIndex = 0;
            this.xitems.Text = "radListView1";
            this.xitems.SelectedItemChanged += new System.EventHandler(this.radListView1_SelectedItemChanged);
            // 
            // splitPanel2
            // 
            this.splitPanel2.Controls.Add(this.spellProperties1);
            this.splitPanel2.Location = new System.Drawing.Point(287, 0);
            this.splitPanel2.Name = "splitPanel2";
            // 
            // 
            // 
            this.splitPanel2.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.splitPanel2.Size = new System.Drawing.Size(416, 495);
            this.splitPanel2.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0.09513593F, 0F);
            this.splitPanel2.SizeInfo.SplitterCorrection = new System.Drawing.Size(76, 0);
            this.splitPanel2.TabIndex = 1;
            this.splitPanel2.TabStop = false;
            this.splitPanel2.Text = "splitPanel2";
            // 
            // spellProperties1
            // 
            this.spellProperties1.BackColor = System.Drawing.Color.Transparent;
            this.spellProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spellProperties1.Location = new System.Drawing.Point(0, 0);
            this.spellProperties1.Name = "spellProperties1";
            this.spellProperties1.Size = new System.Drawing.Size(416, 495);
            this.spellProperties1.TabIndex = 0;
            this.spellProperties1.Resize += new System.EventHandler(this.spellProperties1_Resize);
            // 
            // SpellParamViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radSplitContainer1);
            this.DoubleBuffered = true;
            this.Name = "SpellParamViewer";
            this.Size = new System.Drawing.Size(703, 495);
            this.SizeChanged += new System.EventHandler(this.SpellParamViewer_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer1)).EndInit();
            this.radSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).EndInit();
            this.splitPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xitems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).EndInit();
            this.splitPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadSplitContainer radSplitContainer1;
        private Telerik.WinControls.UI.SplitPanel splitPanel1;
        private Telerik.WinControls.UI.SplitPanel splitPanel2;
        private Telerik.WinControls.UI.RadListView xitems;
        private SpellProperties spellProperties1;
    }
}
