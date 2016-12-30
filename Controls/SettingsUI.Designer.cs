namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class SettingsUI
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
            Telerik.WinControls.UI.ListViewDetailColumn listViewDetailColumn1 = new Telerik.WinControls.UI.ListViewDetailColumn("Column 0", "");
            Telerik.WinControls.UI.ListViewDataItem listViewDataItem1 = new Telerik.WinControls.UI.ListViewDataItem("Statistics");
            Telerik.WinControls.UI.ListViewDataItem listViewDataItem2 = new Telerik.WinControls.UI.ListViewDataItem("Equipments");
            Telerik.WinControls.UI.ListViewDataItem listViewDataItem3 = new Telerik.WinControls.UI.ListViewDataItem("Inventory");
            Telerik.WinControls.UI.ListViewDataItem listViewDataItem4 = new Telerik.WinControls.UI.ListViewDataItem("Database");
            Telerik.WinControls.UI.ListViewDataItem listViewDataItem5 = new Telerik.WinControls.UI.ListViewDataItem("Tools");
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox3 = new Telerik.WinControls.UI.RadGroupBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.xbrowsebackup = new Telerik.WinControls.UI.RadBrowseEditor();
            this.xbrowseresource = new Telerik.WinControls.UI.RadBrowseEditor();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.xviewtabs = new Telerik.WinControls.UI.RadListView();
            this.xpreventduplicates = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).BeginInit();
            this.radGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xbrowsebackup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xbrowseresource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xviewtabs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpreventduplicates)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radButton1);
            this.radPanel1.Controls.Add(this.radGroupBox3);
            this.radPanel1.Controls.Add(this.radGroupBox2);
            this.radPanel1.Controls.Add(this.xpreventduplicates);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(624, 278);
            this.radPanel1.TabIndex = 0;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            // 
            // radButton1
            // 
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radButton1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(0, 236);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(624, 42);
            this.radButton1.TabIndex = 3;
            this.radButton1.Text = "Save Changes!";
            this.radButton1.ThemeName = "VisualStudio2012Dark";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radGroupBox3
            // 
            this.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox3.Controls.Add(this.radLabel2);
            this.radGroupBox3.Controls.Add(this.radLabel1);
            this.radGroupBox3.Controls.Add(this.xbrowsebackup);
            this.radGroupBox3.Controls.Add(this.xbrowseresource);
            this.radGroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGroupBox3.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.radGroupBox3.HeaderText = "Paths";
            this.radGroupBox3.Location = new System.Drawing.Point(0, 145);
            this.radGroupBox3.Name = "radGroupBox3";
            this.radGroupBox3.Size = new System.Drawing.Size(624, 91);
            this.radGroupBox3.TabIndex = 2;
            this.radGroupBox3.Text = "Paths";
            this.radGroupBox3.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.Location = new System.Drawing.Point(5, 56);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(80, 21);
            this.radLabel2.TabIndex = 3;
            this.radLabel2.Text = "Backup Path";
            this.radLabel2.ThemeName = "VisualStudio2012Dark";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(5, 29);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(91, 21);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Resource Path";
            this.radLabel1.ThemeName = "VisualStudio2012Dark";
            // 
            // xbrowsebackup
            // 
            this.xbrowsebackup.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.FolderBrowseDialog;
            this.xbrowsebackup.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xbrowsebackup.Location = new System.Drawing.Point(102, 56);
            this.xbrowsebackup.Name = "xbrowsebackup";
            this.xbrowsebackup.Size = new System.Drawing.Size(517, 27);
            this.xbrowsebackup.TabIndex = 1;
            this.xbrowsebackup.ThemeName = "VisualStudio2012Dark";
            this.xbrowsebackup.ValueChanged += new System.EventHandler(this.xbrowseresource_ValueChanged);
            this.xbrowsebackup.ToolTipTextNeeded += new Telerik.WinControls.ToolTipTextNeededEventHandler(this.radBrowseEditor1_ToolTipTextNeeded_1);
            // 
            // xbrowseresource
            // 
            this.xbrowseresource.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.FolderBrowseDialog;
            this.xbrowseresource.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xbrowseresource.Location = new System.Drawing.Point(102, 26);
            this.xbrowseresource.Name = "xbrowseresource";
            this.xbrowseresource.Size = new System.Drawing.Size(517, 27);
            this.xbrowseresource.TabIndex = 0;
            this.xbrowseresource.ThemeName = "VisualStudio2012Dark";
            this.xbrowseresource.ValueChanged += new System.EventHandler(this.xbrowseresource_ValueChanged);
            this.xbrowseresource.ToolTipTextNeeded += new Telerik.WinControls.ToolTipTextNeededEventHandler(this.radBrowseEditor1_ToolTipTextNeeded);
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.xviewtabs);
            this.radGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGroupBox2.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.radGroupBox2.HeaderText = "Select Tabs to load on slot selection";
            this.radGroupBox2.Location = new System.Drawing.Point(0, 21);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(624, 124);
            this.radGroupBox2.TabIndex = 1;
            this.radGroupBox2.Text = "Select Tabs to load on slot selection";
            this.radGroupBox2.ThemeName = "VisualStudio2012Dark";
            // 
            // xviewtabs
            // 
            this.xviewtabs.AllowEdit = false;
            this.xviewtabs.AllowRemove = false;
            listViewDetailColumn1.HeaderText = "";
            this.xviewtabs.Columns.AddRange(new Telerik.WinControls.UI.ListViewDetailColumn[] {
            listViewDetailColumn1});
            this.xviewtabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xviewtabs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listViewDataItem1.CheckState = Telerik.WinControls.Enumerations.ToggleState.On;
            listViewDataItem1.Text = "Statistics";
            listViewDataItem2.Text = "Equipments";
            listViewDataItem3.Text = "Inventory";
            listViewDataItem4.Text = "Database";
            listViewDataItem5.Text = "Tools";
            this.xviewtabs.Items.AddRange(new Telerik.WinControls.UI.ListViewDataItem[] {
            listViewDataItem1,
            listViewDataItem2,
            listViewDataItem3,
            listViewDataItem4,
            listViewDataItem5});
            this.xviewtabs.Location = new System.Drawing.Point(2, 18);
            this.xviewtabs.Name = "xviewtabs";
            this.xviewtabs.ShowCheckBoxes = true;
            this.xviewtabs.Size = new System.Drawing.Size(620, 104);
            this.xviewtabs.TabIndex = 0;
            this.xviewtabs.ThemeName = "VisualStudio2012Dark";
            this.xviewtabs.ItemMouseDoubleClick += new Telerik.WinControls.UI.ListViewItemEventHandler(this.xviewtabs_ItemMouseDoubleClick);
            // 
            // xpreventduplicates
            // 
            this.xpreventduplicates.AutoSize = false;
            this.xpreventduplicates.Dock = System.Windows.Forms.DockStyle.Top;
            this.xpreventduplicates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpreventduplicates.Location = new System.Drawing.Point(0, 0);
            this.xpreventduplicates.Name = "xpreventduplicates";
            this.xpreventduplicates.Size = new System.Drawing.Size(624, 21);
            this.xpreventduplicates.TabIndex = 4;
            this.xpreventduplicates.Text = "Prevent Item Duplication";
            this.xpreventduplicates.ThemeName = "VisualStudio2012Dark";
            this.xpreventduplicates.ToolTipTextNeeded += new Telerik.WinControls.ToolTipTextNeededEventHandler(this.xpreventduplicates_ToolTipTextNeeded);
            // 
            // SettingsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radPanel1);
            this.DoubleBuffered = true;
            this.Name = "SettingsUI";
            this.Size = new System.Drawing.Size(624, 278);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).EndInit();
            this.radGroupBox3.ResumeLayout(false);
            this.radGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xbrowsebackup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xbrowseresource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xviewtabs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpreventduplicates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox3;
        private Telerik.WinControls.UI.RadBrowseEditor xbrowseresource;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.RadListView xviewtabs;
        private Telerik.WinControls.UI.RadBrowseEditor xbrowsebackup;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadCheckBox xpreventduplicates;
    }
}
