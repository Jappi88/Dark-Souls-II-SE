using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class SettingsUI : UserControl
    {
        public SettingsUI()
        {
            InitializeComponent();
            LoadSettings();
        }

        public event EventHandler CloseRequest;

        protected virtual void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        private void radBrowseEditor1_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTip.ToolTipTitle = "Resource Directory";
            e.ToolTipText = "Choose the desired path for the resources to be stored";
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            SaveSettings();
            OnCloseRequest();
        }

        private void radBrowseEditor1_ToolTipTextNeeded_1(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTip.ToolTipTitle = "Backup Directory";
            e.ToolTipText = "Choose the desired path for the Backups";
        }

        private void LoadSettings()
        {
            if (MainForm.AppPreferences.ViewTabs == null)
                MainForm.AppPreferences.ViewTabs = new int[] {};
            var x = MainForm.AppPreferences;
            xpreventduplicates.ToggleState = x.PreventDuplicates ? ToggleState.On : ToggleState.Off;
            for (var i = 0; i < xviewtabs.Items.Count; i++)
            {
                xviewtabs.Items[i].Tag = i;
                xviewtabs.Items[i].CheckState = x.ViewTabs.Contains(i) ? ToggleState.On : ToggleState.Off;
            }
            xbrowseresource.Value = MainForm.Settings.ResourcePath;
            xbrowsebackup.Value = MainForm.Settings.BackupPath;
        }

        private void SaveSettings()
        {
            var x = MainForm.AppPreferences;
            x.PreventDuplicates = xpreventduplicates.ToggleState == ToggleState.On;
            var viewtabs = new List<int>();
            viewtabs.AddRange(from item in xviewtabs.Items where item.CheckState == ToggleState.On select (int) item.Tag);
            x.ViewTabs = viewtabs.ToArray();
            MainForm.Settings.ResourcePath = xbrowseresource.Value;
            MainForm.Settings.BackupPath = xbrowsebackup.Value;
            x.Save();
            MainForm.Settings.Save();
        }

        private void xtitlebar_Close(object sender, EventArgs args)
        {
            OnCloseRequest();
        }

        private void xbrowseresource_ValueChanged(object sender, EventArgs e)
        {
            var x = sender as RadBrowseEditorElement;
            if (x != null)
            {
                x.ToolTipText = x.Value;
            }
        }

        private void xviewtabs_ItemMouseDoubleClick(object sender, ListViewItemEventArgs e)
        {
            e.Item.CheckState = e.Item.CheckState == ToggleState.On ? ToggleState.Off : ToggleState.On;
        }

        private void xalloweddlc_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            var item = sender as SimpleListViewVisualItem;
            if (item != null)
                e.ToolTipText = item.Data.Text;
        }

        private void xpreventduplicates_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTip.ToolTipTitle = "Prevent Item Duplication";
            e.ToolTipText = "Enable this if you wish to prevent duplicate items";
        }
    }
}