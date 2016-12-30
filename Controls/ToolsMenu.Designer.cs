namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class ToolsMenu
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
            this.xcopybuild = new Telerik.WinControls.UI.RadDropDownButton();
            this.xcopyprogress = new Telerik.WinControls.UI.RadDropDownButton();
            this.xcopyinventory = new Telerik.WinControls.UI.RadDropDownButton();
            this.xcopystats = new Telerik.WinControls.UI.RadDropDownButton();
            this.tileGroupElement1 = new Telerik.WinControls.UI.TileGroupElement();
            this.xextractchar = new Telerik.WinControls.UI.RadTileElement();
            this.xextractinventory = new Telerik.WinControls.UI.RadTileElement();
            this.xextractgameprogress = new Telerik.WinControls.UI.RadTileElement();
            this.xreplacechar = new Telerik.WinControls.UI.RadTileElement();
            this.xreplaceinventory = new Telerik.WinControls.UI.RadTileElement();
            this.xreplacegameprogress = new Telerik.WinControls.UI.RadTileElement();
            this.xextractbuild = new Telerik.WinControls.UI.RadTileElement();
            this.xreplacebuild = new Telerik.WinControls.UI.RadTileElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            this.radPanorama1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xcopybuild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopyprogress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopyinventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopystats)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanorama1
            // 
            this.radPanorama1.AllowDragDrop = false;
            this.radPanorama1.CellSize = new System.Drawing.Size(250, 80);
            this.radPanorama1.Controls.Add(this.xcopybuild);
            this.radPanorama1.Controls.Add(this.xcopyprogress);
            this.radPanorama1.Controls.Add(this.xcopyinventory);
            this.radPanorama1.Controls.Add(this.xcopystats);
            this.radPanorama1.Groups.AddRange(new Telerik.WinControls.RadItem[] {
            this.tileGroupElement1});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.RowsCount = 5;
            this.radPanorama1.ShowGroups = true;
            this.radPanorama1.Size = new System.Drawing.Size(781, 360);
            this.radPanorama1.TabIndex = 1;
            this.radPanorama1.Text = "radPanorama1";
            this.radPanorama1.ThemeName = "VisualStudio2012Dark";
            // 
            // xcopybuild
            // 
            this.xcopybuild.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.xcopybuild.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.xcopybuild.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xcopybuild.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.xcopybuild.Location = new System.Drawing.Point(525, 284);
            this.xcopybuild.Name = "xcopybuild";
            this.xcopybuild.Size = new System.Drawing.Size(250, 70);
            this.xcopybuild.TabIndex = 4;
            this.xcopybuild.Text = "Copy Character Build To";
            this.xcopybuild.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopybuild.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopybuild.GetChildAt(0))).Text = "Copy Character Build To";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopybuild.GetChildAt(0))).CanFocus = true;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xcopybuild.GetChildAt(0).GetChildAt(1).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // xcopyprogress
            // 
            this.xcopyprogress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.xcopyprogress.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.xcopyprogress.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xcopyprogress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.xcopyprogress.Location = new System.Drawing.Point(525, 205);
            this.xcopyprogress.Name = "xcopyprogress";
            this.xcopyprogress.Size = new System.Drawing.Size(250, 70);
            this.xcopyprogress.TabIndex = 3;
            this.xcopyprogress.Text = "Copy Game Progress To";
            this.xcopyprogress.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyprogress.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyprogress.GetChildAt(0))).Text = "Copy Game Progress To";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyprogress.GetChildAt(0))).CanFocus = true;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xcopyprogress.GetChildAt(0).GetChildAt(1).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // xcopyinventory
            // 
            this.xcopyinventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.xcopyinventory.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.xcopyinventory.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xcopyinventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.xcopyinventory.Location = new System.Drawing.Point(525, 125);
            this.xcopyinventory.Name = "xcopyinventory";
            this.xcopyinventory.Size = new System.Drawing.Size(250, 70);
            this.xcopyinventory.TabIndex = 2;
            this.xcopyinventory.Text = "Copy Inventory To";
            this.xcopyinventory.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyinventory.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyinventory.GetChildAt(0))).Text = "Copy Inventory To";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopyinventory.GetChildAt(0))).CanFocus = true;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xcopyinventory.GetChildAt(0).GetChildAt(1).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // xcopystats
            // 
            this.xcopystats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.xcopystats.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.xcopystats.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xcopystats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.xcopystats.Location = new System.Drawing.Point(525, 44);
            this.xcopystats.Name = "xcopystats";
            this.xcopystats.Size = new System.Drawing.Size(250, 70);
            this.xcopystats.TabIndex = 1;
            this.xcopystats.Text = "Copy Character Stats To";
            this.xcopystats.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopystats.GetChildAt(0))).DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopystats.GetChildAt(0))).Text = "Copy Character Stats To";
            ((Telerik.WinControls.UI.RadDropDownButtonElement)(this.xcopystats.GetChildAt(0))).CanFocus = true;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.xcopystats.GetChildAt(0).GetChildAt(1).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // tileGroupElement1
            // 
            this.tileGroupElement1.AccessibleDescription = "Tools";
            this.tileGroupElement1.AccessibleName = "Tools";
            this.tileGroupElement1.CellSize = new System.Drawing.Size(260, 80);
            this.tileGroupElement1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.xextractchar,
            this.xextractinventory,
            this.xextractgameprogress,
            this.xreplacechar,
            this.xreplaceinventory,
            this.xreplacegameprogress,
            this.xextractbuild,
            this.xreplacebuild});
            this.tileGroupElement1.Name = "tileGroupElement1";
            this.tileGroupElement1.RowsCount = 5;
            this.tileGroupElement1.Text = "Tools";
            this.tileGroupElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // xextractchar
            // 
            this.xextractchar.AccessibleDescription = "radTileElement1";
            this.xextractchar.AccessibleName = "radTileElement1";
            this.xextractchar.AutoSize = true;
            this.xextractchar.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xextractchar.Name = "xextractchar";
            this.xextractchar.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xextractchar.Text = "Extract Character Stats";
            this.xextractchar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xextractchar.Click += new System.EventHandler(this.xextractchar_Click);
            // 
            // xextractinventory
            // 
            this.xextractinventory.AccessibleDescription = "radTileElement2";
            this.xextractinventory.AccessibleName = "radTileElement2";
            this.xextractinventory.AutoSize = true;
            this.xextractinventory.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xextractinventory.Name = "xextractinventory";
            this.xextractinventory.Row = 1;
            this.xextractinventory.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xextractinventory.Text = "Extract Inventory";
            this.xextractinventory.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xextractinventory.Click += new System.EventHandler(this.xextractinventory_Click);
            // 
            // xextractgameprogress
            // 
            this.xextractgameprogress.AccessibleDescription = "radTileElement3";
            this.xextractgameprogress.AccessibleName = "radTileElement3";
            this.xextractgameprogress.AutoSize = true;
            this.xextractgameprogress.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xextractgameprogress.Name = "xextractgameprogress";
            this.xextractgameprogress.Row = 2;
            this.xextractgameprogress.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xextractgameprogress.Text = "Extract Game Progress";
            this.xextractgameprogress.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xextractgameprogress.Click += new System.EventHandler(this.xextractgameprogress_Click);
            // 
            // xreplacechar
            // 
            this.xreplacechar.AccessibleDescription = "radTileElement4";
            this.xreplacechar.AccessibleName = "radTileElement4";
            this.xreplacechar.AutoSize = true;
            this.xreplacechar.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xreplacechar.Column = 1;
            this.xreplacechar.Name = "xreplacechar";
            this.xreplacechar.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xreplacechar.Text = "Replace Character Stats";
            this.xreplacechar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xreplacechar.Click += new System.EventHandler(this.xreplacechar_Click);
            // 
            // xreplaceinventory
            // 
            this.xreplaceinventory.AccessibleDescription = "radTileElement5";
            this.xreplaceinventory.AccessibleName = "radTileElement5";
            this.xreplaceinventory.AutoSize = true;
            this.xreplaceinventory.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xreplaceinventory.Column = 1;
            this.xreplaceinventory.Name = "xreplaceinventory";
            this.xreplaceinventory.Row = 1;
            this.xreplaceinventory.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xreplaceinventory.Text = "Replace Inventory";
            this.xreplaceinventory.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xreplaceinventory.Click += new System.EventHandler(this.xreplaceinventory_Click);
            // 
            // xreplacegameprogress
            // 
            this.xreplacegameprogress.AccessibleDescription = "radTileElement6";
            this.xreplacegameprogress.AccessibleName = "radTileElement6";
            this.xreplacegameprogress.AutoSize = true;
            this.xreplacegameprogress.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xreplacegameprogress.Column = 1;
            this.xreplacegameprogress.Name = "xreplacegameprogress";
            this.xreplacegameprogress.Row = 2;
            this.xreplacegameprogress.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xreplacegameprogress.Text = "Replace Game Progress";
            this.xreplacegameprogress.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xreplacegameprogress.Click += new System.EventHandler(this.xreplacegameprogress_Click);
            // 
            // xextractbuild
            // 
            this.xextractbuild.AccessibleDescription = "radTileElement8";
            this.xextractbuild.AccessibleName = "radTileElement8";
            this.xextractbuild.AutoSize = true;
            this.xextractbuild.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xextractbuild.Name = "xextractbuild";
            this.xextractbuild.Row = 3;
            this.xextractbuild.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xextractbuild.Text = "Extract Character Build";
            this.xextractbuild.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xextractbuild.Click += new System.EventHandler(this.xextractbuild_Click);
            // 
            // xreplacebuild
            // 
            this.xreplacebuild.AccessibleDescription = "radTileElement9";
            this.xreplacebuild.AccessibleName = "radTileElement9";
            this.xreplacebuild.AutoSize = true;
            this.xreplacebuild.ClickMode = Telerik.WinControls.ClickMode.Press;
            this.xreplacebuild.Column = 1;
            this.xreplacebuild.Name = "xreplacebuild";
            this.xreplacebuild.Row = 3;
            this.xreplacebuild.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.xreplacebuild.Text = "Replace Character Build";
            this.xreplacebuild.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.xreplacebuild.Click += new System.EventHandler(this.xreplacebuild_Click);
            // 
            // ToolsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radPanorama1);
            this.DoubleBuffered = true;
            this.Name = "ToolsMenu";
            this.Size = new System.Drawing.Size(784, 362);
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            this.radPanorama1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xcopybuild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopyprogress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopyinventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcopystats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.TileGroupElement tileGroupElement1;
        private Telerik.WinControls.UI.RadTileElement xextractchar;
        private Telerik.WinControls.UI.RadTileElement xextractinventory;
        private Telerik.WinControls.UI.RadTileElement xextractgameprogress;
        private Telerik.WinControls.UI.RadTileElement xreplacechar;
        private Telerik.WinControls.UI.RadTileElement xreplaceinventory;
        private Telerik.WinControls.UI.RadTileElement xreplacegameprogress;
        private Telerik.WinControls.UI.RadTileElement xextractbuild;
        private Telerik.WinControls.UI.RadTileElement xreplacebuild;
        private Telerik.WinControls.UI.RadDropDownButton xcopybuild;
        private Telerik.WinControls.UI.RadDropDownButton xcopyprogress;
        private Telerik.WinControls.UI.RadDropDownButton xcopyinventory;
        private Telerik.WinControls.UI.RadDropDownButton xcopystats;
    }
}
