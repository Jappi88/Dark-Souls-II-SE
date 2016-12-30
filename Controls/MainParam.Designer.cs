namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class MainParam
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
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.xitemparams = new Telerik.WinControls.UI.RadTileElement();
            this.xspellparams = new Telerik.WinControls.UI.RadTileElement();
            this.xweaponstateffect = new Telerik.WinControls.UI.RadTileElement();
            this.xstatslevel = new Telerik.WinControls.UI.RadTileElement();
            this.xplayerlevelup = new Telerik.WinControls.UI.RadTileElement();
            this.xothers = new Telerik.WinControls.UI.RadTileElement();
            this.xsave = new Telerik.WinControls.UI.RadTileElement();
            this.xcontainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel2
            // 
            this.radPanel2.AutoScroll = true;
            this.radPanel2.Controls.Add(this.radPanorama1);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.radPanel2.Location = new System.Drawing.Point(0, 0);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(215, 499);
            this.radPanel2.TabIndex = 5;
            // 
            // radPanorama1
            // 
            this.radPanorama1.AllowDragDrop = false;
            this.radPanorama1.AutoScroll = true;
            this.radPanorama1.CellSize = new System.Drawing.Size(200, 60);
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanorama1.EnableZooming = false;
            this.radPanorama1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.xitemparams,
            this.xspellparams,
            this.xweaponstateffect,
            this.xstatslevel,
            this.xplayerlevelup,
            this.xothers,
            this.xsave});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.MouseWheelBehavior = Telerik.WinControls.UI.PanoramaMouseWheelBehavior.Scroll;
            this.radPanorama1.Name = "radPanorama1";
            // 
            // 
            // 
            this.radPanorama1.RootElement.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radPanorama1.RootElement.ApplyShapeToControl = true;
            this.radPanorama1.RootElement.AutoSize = false;
            this.radPanorama1.RootElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radPanorama1.RowsCount = 8;
            this.radPanorama1.ScrollBarThickness = 12;
            this.radPanorama1.Size = new System.Drawing.Size(215, 499);
            this.radPanorama1.TabIndex = 3;
            this.radPanorama1.Text = "radPanorama1";
            this.radPanorama1.ThemeName = "VisualStudio2012Dark";
            // 
            // xitemparams
            // 
            this.xitemparams.AccessibleDescription = "Item Parameters";
            this.xitemparams.AccessibleName = "Item Parameters";
            this.xitemparams.Name = "xitemparams";
            this.xitemparams.ShowHorizontalLine = false;
            this.xitemparams.StretchHorizontally = true;
            this.xitemparams.StretchVertically = true;
            this.xitemparams.Text = "Item Parameters";
            this.xitemparams.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xitemparams.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xitemparams.Click += new System.EventHandler(this.xitemparams_Click);
            // 
            // xspellparams
            // 
            this.xspellparams.AccessibleDescription = "Spell Parameters";
            this.xspellparams.AccessibleName = "Spell Parameters";
            this.xspellparams.Name = "xspellparams";
            this.xspellparams.Row = 1;
            this.xspellparams.Text = "Spell Parameters";
            this.xspellparams.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xspellparams.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xspellparams.Click += new System.EventHandler(this.xspellparams_Click);
            // 
            // xweaponstateffect
            // 
            this.xweaponstateffect.AccessibleDescription = "Weapon Stat Effect";
            this.xweaponstateffect.AccessibleName = "Weapon Stat Effect";
            this.xweaponstateffect.Name = "xweaponstateffect";
            this.xweaponstateffect.Row = 2;
            this.xweaponstateffect.Text = "Weapon Stat Effect";
            this.xweaponstateffect.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xweaponstateffect.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // xstatslevel
            // 
            this.xstatslevel.AccessibleDescription = "Physical Stats Level";
            this.xstatslevel.AccessibleName = "Physical Stats Level";
            this.xstatslevel.Name = "xstatslevel";
            this.xstatslevel.Row = 3;
            this.xstatslevel.Text = "Physical Stats Level";
            this.xstatslevel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xstatslevel.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // xplayerlevelup
            // 
            this.xplayerlevelup.AccessibleDescription = "Player Level Up";
            this.xplayerlevelup.AccessibleName = "Player Level Up";
            this.xplayerlevelup.Name = "xplayerlevelup";
            this.xplayerlevelup.Row = 4;
            this.xplayerlevelup.Text = "Player Level Up";
            this.xplayerlevelup.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xplayerlevelup.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // xothers
            // 
            this.xothers.AccessibleDescription = "Save Changes";
            this.xothers.AccessibleName = "Save Changes";
            this.xothers.Name = "xothers";
            this.xothers.Row = 5;
            this.xothers.Text = "All Parameters";
            this.xothers.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xothers.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xothers.Click += new System.EventHandler(this.xothers_Click);
            // 
            // xsave
            // 
            this.xsave.AccessibleDescription = "Save Changes";
            this.xsave.AccessibleName = "Save Changes";
            this.xsave.Name = "xsave";
            this.xsave.Row = 6;
            this.xsave.Text = "Save Changes";
            this.xsave.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.xsave.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xsave.Click += new System.EventHandler(this.xsave_Click);
            // 
            // xcontainer
            // 
            this.xcontainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.xcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcontainer.Location = new System.Drawing.Point(215, 0);
            this.xcontainer.Name = "xcontainer";
            this.xcontainer.Size = new System.Drawing.Size(530, 499);
            this.xcontainer.TabIndex = 6;
            // 
            // MainParam
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xcontainer);
            this.Controls.Add(this.radPanel2);
            this.DoubleBuffered = true;
            this.Name = "MainParam";
            this.Size = new System.Drawing.Size(745, 499);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadTileElement xitemparams;
        private Telerik.WinControls.UI.RadTileElement xweaponstateffect;
        private Telerik.WinControls.UI.RadTileElement xstatslevel;
        private Telerik.WinControls.UI.RadTileElement xplayerlevelup;
        private Telerik.WinControls.UI.RadTileElement xothers;
        private Telerik.WinControls.UI.RadTileElement xsave;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadTileElement xspellparams;
        private System.Windows.Forms.Panel xcontainer;
    }
}
