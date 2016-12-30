using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;

namespace Dark_Souls_II_Save_Editor.Controls
{
    [Serializable]
    public partial class SlotSelector : UserControl
    {
        public delegate void SlotSelectedArg(SaveSlot slot);

        private bool _busy;
        public MainFile MainInstance = null;

        public SlotSelector()
        {
            InitializeComponent();
        }

        public event SlotSelectedArg OnSlotSelected;

        public void LoadSlots(MainFile main)
        {
            _busy = true;
            xslotcontainer.Controls.Clear();
            var count = 0;
            foreach (var s in main.UsedSlots.Reverse())
            {
                Application.DoEvents();
                Application.DoEvents();
                var slot = new UsedSlot(s);
                slot.OnSlotSelected += SetSelected;
                slot.OnSlotChanged += SlotChanged;
                Application.DoEvents();
                slot.Dock = DockStyle.Top;
                xslotcontainer.Controls.Add(slot);
                xslotcontainer.Controls.Add(new Label
                {
                    AutoSize = false,
                    Height = 5,
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Top
                });
                count++;
                xslotcontainer.Refresh();
                Application.DoEvents();
            }
            _busy = false;
        }

        private void SlotChanged(SaveSlot s)
        {
            LoadSlots(s.MainInstance);
        }

        private void SetSelected(SaveSlot s)
        {
            if (_busy)
                return;
            _busy = true;
            OnSlotSelected?.Invoke(s);
            _busy = false;
        }
    }
}