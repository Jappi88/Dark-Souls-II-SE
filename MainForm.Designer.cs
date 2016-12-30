using HavenInterface.LoadingCircle;

namespace Dark_Souls_II_Save_Editor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.xoptioncontainer = new Telerik.WinControls.UI.RadPanorama();
            this.xfile = new Telerik.WinControls.UI.RadTileElement();
            this.xlogin = new Telerik.WinControls.UI.RadTileElement();
            this.xsettings = new Telerik.WinControls.UI.RadTileElement();
            this.xrestore = new Telerik.WinControls.UI.RadTileElement();
            this.xloaddb = new Telerik.WinControls.UI.RadTileElement();
            this.xbanned = new Telerik.WinControls.UI.RadTileElement();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.xdock = new Telerik.WinControls.UI.Docking.RadDock();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            ((System.ComponentModel.ISupportInitialize)(this.xoptioncontainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdock)).BeginInit();
            this.xdock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // xoptioncontainer
            // 
            this.xoptioncontainer.AllowDragDrop = false;
            this.xoptioncontainer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.xoptioncontainer.CellSize = new System.Drawing.Size(130, 50);
            this.xoptioncontainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.xoptioncontainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xoptioncontainer.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.xfile,
            this.xlogin,
            this.xsettings,
            this.xrestore,
            this.xloaddb,
            this.xbanned});
            this.xoptioncontainer.Location = new System.Drawing.Point(0, 0);
            this.xoptioncontainer.Name = "xoptioncontainer";
            // 
            // 
            // 
            this.xoptioncontainer.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 240, 150);
            this.xoptioncontainer.RowsCount = 3;
            this.xoptioncontainer.ScrollBarThickness = 12;
            this.xoptioncontainer.Size = new System.Drawing.Size(1248, 62);
            this.xoptioncontainer.TabIndex = 3;
            this.xoptioncontainer.Text = "radPanorama1";
            this.xoptioncontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // xfile
            // 
            this.xfile.AccessibleDescription = "FILE";
            this.xfile.AccessibleName = "FILE";
            this.xfile.Name = "xfile";
            this.xfile.Text = "Load";
            this.xfile.Click += new System.EventHandler(this.xfile_Click);
            // 
            // xlogin
            // 
            this.xlogin.Column = 1;
            this.xlogin.Name = "xlogin";
            this.xlogin.Text = "Donate";
            this.xlogin.ToolTipText = "Buy me a Beer ;)";
            this.xlogin.Click += new System.EventHandler(this.xlogin_Click);
            // 
            // xsettings
            // 
            this.xsettings.AccessibleDescription = "Parameters";
            this.xsettings.AccessibleName = "Parameters";
            this.xsettings.Column = 2;
            this.xsettings.Name = "xsettings";
            this.xsettings.Text = "Settings";
            this.xsettings.ToolTipText = "Edit Tool Settings";
            this.xsettings.Click += new System.EventHandler(this.xgameparams_Click);
            // 
            // xrestore
            // 
            this.xrestore.AccessibleDescription = "Restore STFS";
            this.xrestore.AccessibleName = "Restore STFS";
            this.xrestore.Column = 3;
            this.xrestore.Name = "xrestore";
            this.xrestore.Text = "Fix STFS";
            this.xrestore.ToolTipText = "Restores Corrupted DSII STFS Files";
            this.xrestore.Click += new System.EventHandler(this.xrestore_Click);
            // 
            // xloaddb
            // 
            this.xloaddb.Column = 4;
            this.xloaddb.Name = "xloaddb";
            this.xloaddb.Text = "Reload DB";
            this.xloaddb.ToolTipText = "Reloads Database into memory";
            this.xloaddb.Click += new System.EventHandler(this.xloaddb_Click);
            // 
            // xbanned
            // 
            this.xbanned.Column = 5;
            this.xbanned.Name = "xbanned";
            this.xbanned.Text = "Prevent Ban!";
            this.xbanned.ToolTipText = "Prevent Ban, Read THIS!";
            this.xbanned.Click += new System.EventHandler(this.xbanned_Click);
            // 
            // xdock
            // 
            this.xdock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.xdock.Controls.Add(this.documentContainer1);
            this.xdock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xdock.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xdock.IsCleanUpTarget = true;
            this.xdock.Location = new System.Drawing.Point(0, 62);
            this.xdock.MainDocumentContainer = this.documentContainer1;
            this.xdock.Name = "xdock";
            this.xdock.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.xdock.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.xdock.ShowDocumentCloseButton = true;
            this.xdock.ShowToolCloseButton = true;
            this.xdock.Size = new System.Drawing.Size(1248, 564);
            this.xdock.SplitterWidth = 2;
            this.xdock.TabIndex = 4;
            this.xdock.TabStop = false;
            this.xdock.Text = "radDock1";
            this.xdock.ThemeName = "VisualStudio2012Dark";
            this.xdock.ToolWindowInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront;
            // 
            // documentContainer1
            // 
            this.documentContainer1.BackColor = System.Drawing.Color.Transparent;
            this.documentContainer1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("documentContainer1.BackgroundImage")));
            this.documentContainer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.documentContainer1.Name = "documentContainer1";
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SplitterWidth = 2;
            this.documentContainer1.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.SplitPanelElement)(this.documentContainer1.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.documentContainer1.GetChildAt(0).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 626);
            this.Controls.Add(this.xdock);
            this.Controls.Add(this.xoptioncontainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(975, 630);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Dark Souls II Save Editor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.xoptioncontainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xdock)).EndInit();
            this.xdock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama xoptioncontainer;
        private Telerik.WinControls.UI.RadTileElement xfile;
        private Telerik.WinControls.UI.RadTileElement xsettings;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadTileElement xrestore;
        private Telerik.WinControls.UI.Docking.RadDock xdock;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.RadTileElement xloaddb;
        private Telerik.WinControls.UI.RadTileElement xlogin;
        private Telerik.WinControls.UI.RadTileElement xbanned;



    }
}

