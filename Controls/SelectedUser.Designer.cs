namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class SelectedUser
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
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.xplayerstatus = new Telerik.WinControls.UI.RadTileElement();
            this.xequipment = new Telerik.WinControls.UI.RadTileElement();
            this.xinventory = new Telerik.WinControls.UI.RadTileElement();
            this.xmapinfo = new Telerik.WinControls.UI.RadTileElement();
            this.xdatabase = new Telerik.WinControls.UI.RadTileElement();
            this.xtools = new Telerik.WinControls.UI.RadTileElement();
            this.xsavechanges = new Telerik.WinControls.UI.RadTileElement();
            this.xsavedb = new Telerik.WinControls.UI.RadTileElement();
            this.xcontainer = new Telerik.WinControls.UI.RadListControl();
            this.xdock = new Telerik.WinControls.UI.Docking.RadDock();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.xmenupanel = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).BeginInit();
            this.xcontainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xdock)).BeginInit();
            this.xdock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xmenupanel)).BeginInit();
            this.xmenupanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // radPanorama1
            // 
            this.radPanorama1.AllowDragDrop = false;
            this.radPanorama1.AutoScroll = true;
            this.radPanorama1.CellSize = new System.Drawing.Size(150, 50);
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanorama1.EnableZooming = false;
            this.radPanorama1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPanorama1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.xplayerstatus,
            this.xequipment,
            this.xinventory,
            this.xmapinfo,
            this.xdatabase,
            this.xtools,
            this.xsavechanges,
            this.xsavedb});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.MouseWheelBehavior = Telerik.WinControls.UI.PanoramaMouseWheelBehavior.Scroll;
            this.radPanorama1.Name = "radPanorama1";
            // 
            // 
            // 
            this.radPanorama1.RootElement.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radPanorama1.RootElement.AutoSize = false;
            this.radPanorama1.RootElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radPanorama1.RowsCount = 7;
            this.radPanorama1.ScrollBarThickness = 12;
            this.radPanorama1.Size = new System.Drawing.Size(1072, 59);
            this.radPanorama1.TabIndex = 2;
            // 
            // xplayerstatus
            // 
            this.xplayerstatus.AccessibleDescription = "Load XBOX360 Save";
            this.xplayerstatus.AccessibleName = "Load XBOX360 Save";
            this.xplayerstatus.AutoToolTip = true;
            this.xplayerstatus.Name = "xplayerstatus";
            this.xplayerstatus.ShowHorizontalLine = false;
            this.xplayerstatus.StretchHorizontally = true;
            this.xplayerstatus.StretchVertically = true;
            this.xplayerstatus.Text = "Player Status";
            this.xplayerstatus.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xplayerstatus.ToolTipText = "View and edit your player stats";
            this.xplayerstatus.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xplayerstatus.Click += new System.EventHandler(this.xplayerstatus_Click);
            // 
            // xequipment
            // 
            this.xequipment.AccessibleDescription = "Load Save From USB";
            this.xequipment.AccessibleName = "Load Save From USB";
            this.xequipment.AutoToolTip = true;
            this.xequipment.Column = 1;
            this.xequipment.Name = "xequipment";
            this.xequipment.Text = "Equipment";
            this.xequipment.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xequipment.ToolTipText = "View and edit your current equipment";
            this.xequipment.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xequipment.Click += new System.EventHandler(this.xequipment_Click);
            // 
            // xinventory
            // 
            this.xinventory.AccessibleDescription = "Load PS3 Save Directory";
            this.xinventory.AccessibleName = "Load PS3 Save Directory";
            this.xinventory.AutoToolTip = true;
            this.xinventory.Column = 2;
            this.xinventory.Name = "xinventory";
            this.xinventory.Text = "Inventory";
            this.xinventory.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xinventory.ToolTipText = "View and manage your inventory and itembox";
            this.xinventory.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xinventory.Click += new System.EventHandler(this.xinventory_Click);
            // 
            // xmapinfo
            // 
            this.xmapinfo.AccessibleDescription = "Bonfires";
            this.xmapinfo.AccessibleName = "Bonfires";
            this.xmapinfo.AutoToolTip = true;
            this.xmapinfo.Column = 7;
            this.xmapinfo.Name = "xmapinfo";
            this.xmapinfo.Text = "Bonfires";
            this.xmapinfo.ToolTipText = "View and edit bonfire areas";
            this.xmapinfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.xmapinfo.Click += new System.EventHandler(this.xmapinfo_Click);
            // 
            // xdatabase
            // 
            this.xdatabase.AccessibleDescription = "DataBase";
            this.xdatabase.AccessibleName = "DataBase";
            this.xdatabase.AutoToolTip = true;
            this.xdatabase.Column = 3;
            this.xdatabase.Name = "xdatabase";
            this.xdatabase.Text = "DataBase";
            this.xdatabase.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xdatabase.ToolTipText = "View and manage database";
            this.xdatabase.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xdatabase.Click += new System.EventHandler(this.xdatabase_Click);
            // 
            // xtools
            // 
            this.xtools.AccessibleDescription = "Close File";
            this.xtools.AccessibleName = "Close File";
            this.xtools.AutoToolTip = true;
            this.xtools.Column = 4;
            this.xtools.Name = "xtools";
            this.xtools.Text = "Tools";
            this.xtools.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xtools.ToolTipText = "Varios tools to help you on your modding journey";
            this.xtools.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xtools.Click += new System.EventHandler(this.xtools_Click);
            // 
            // xsavechanges
            // 
            this.xsavechanges.AccessibleDescription = "Save Changes";
            this.xsavechanges.AccessibleName = "Save Changes";
            this.xsavechanges.AutoToolTip = true;
            this.xsavechanges.Column = 5;
            this.xsavechanges.Name = "xsavechanges";
            this.xsavechanges.Text = "Save Changes";
            this.xsavechanges.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xsavechanges.ToolTipText = "Save all changes made to this slot";
            this.xsavechanges.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xsavechanges.Click += new System.EventHandler(this.xsavechanges_Click);
            // 
            // xsavedb
            // 
            this.xsavedb.AccessibleDescription = "Save DataBase";
            this.xsavedb.AccessibleName = "Save DataBase";
            this.xsavedb.AutoSize = false;
            this.xsavedb.AutoToolTip = true;
            this.xsavedb.Bounds = new System.Drawing.Rectangle(0, 0, 150, 40);
            this.xsavedb.Column = 6;
            this.xsavedb.Name = "xsavedb";
            this.xsavedb.Text = "Save DataBase";
            this.xsavedb.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.xsavedb.ToolTipText = "Save all changes made to the database";
            this.xsavedb.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xsavedb.Click += new System.EventHandler(this.xsavedb_Click);
            // 
            // xcontainer
            // 
            this.xcontainer.AutoScroll = true;
            this.xcontainer.Controls.Add(this.xdock);
            this.xcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcontainer.Location = new System.Drawing.Point(0, 59);
            this.xcontainer.Name = "xcontainer";
            this.xcontainer.Size = new System.Drawing.Size(1072, 603);
            this.xcontainer.TabIndex = 3;
            this.xcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // xdock
            // 
            this.xdock.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.xdock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.xdock.Controls.Add(this.documentContainer1);
            this.xdock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xdock.DocumentManager.DocumentCloseActivation = Telerik.WinControls.UI.Docking.DocumentCloseActivation.FirstInZOrder;
            this.xdock.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdock.IsCleanUpTarget = true;
            this.xdock.Location = new System.Drawing.Point(0, 0);
            this.xdock.MainDocumentContainer = this.documentContainer1;
            this.xdock.Name = "xdock";
            this.xdock.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.xdock.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.xdock.ShowDocumentCloseButton = true;
            this.xdock.ShowToolCloseButton = true;
            this.xdock.Size = new System.Drawing.Size(1072, 603);
            this.xdock.SplitterWidth = 2;
            this.xdock.TabIndex = 0;
            this.xdock.TabStop = false;
            this.xdock.Text = "radDock1";
            this.xdock.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.SplitPanelElement)(this.xdock.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xdock.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xdock.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xdock.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xdock.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xdock.GetChildAt(0).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.xdock.GetChildAt(0).GetChildAt(1))).BottomWidth = 0F;
            // 
            // documentContainer1
            // 
            this.documentContainer1.Name = "documentContainer1";
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SplitterWidth = 2;
            this.documentContainer1.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.SplitPanelElement)(this.documentContainer1.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // xmenupanel
            // 
            this.xmenupanel.Controls.Add(this.xcontainer);
            this.xmenupanel.Controls.Add(this.radPanorama1);
            this.xmenupanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmenupanel.Location = new System.Drawing.Point(0, 0);
            this.xmenupanel.Name = "xmenupanel";
            this.xmenupanel.Size = new System.Drawing.Size(1072, 662);
            this.xmenupanel.TabIndex = 4;
            // 
            // SelectedUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xmenupanel);
            this.DoubleBuffered = true;
            this.Name = "SelectedUser";
            this.Size = new System.Drawing.Size(1072, 662);
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontainer)).EndInit();
            this.xcontainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xdock)).EndInit();
            this.xdock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xmenupanel)).EndInit();
            this.xmenupanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadTileElement xplayerstatus;
        private Telerik.WinControls.UI.RadTileElement xequipment;
        private Telerik.WinControls.UI.RadTileElement xinventory;
        private Telerik.WinControls.UI.RadTileElement xtools;
        private Telerik.WinControls.UI.RadTileElement xsavechanges;
        private Telerik.WinControls.UI.RadTileElement xdatabase;
        private Telerik.WinControls.UI.RadListControl xcontainer;
        private Telerik.WinControls.UI.RadTileElement xsavedb;
        private Telerik.WinControls.UI.RadPanel xmenupanel;
        private Telerik.WinControls.UI.Docking.RadDock xdock;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.RadTileElement xmapinfo;
    }
}
