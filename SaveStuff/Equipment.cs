using System.Linq;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class Equipment
    {
        public Equipment(SaveSlot slot, byte[] data)
        {
            SlotInstance = slot;
            using (var io = new StreamIO(data, true))
            {
                RightWeapon1 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                LeftWeapon1 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                RightWeapon2 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                LeftWeapon2 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                RightWeapon3 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                LeftWeapon3 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);

                Head = slot.Inventory.Items.GetEntryById(MakeArmorID(io.ReadUInt32(), true), true);
                Chest = slot.Inventory.Items.GetEntryById(MakeArmorID(io.ReadUInt32(), true), true);
                Hands = slot.Inventory.Items.GetEntryById(MakeArmorID(io.ReadUInt32(), true), true);
                Legs = slot.Inventory.Items.GetEntryById(MakeArmorID(io.ReadUInt32(), true), true);
                io.Position += 8;
                Arrow1 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                Arrow2 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                Bolt1 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                Bolt2 = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);

                Rings = new ItemEntry[4];
                for (var i = 0; i < 4; i++)
                {
                    Rings[i] = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                    if (Rings[i] != null)
                        Rings[i].IsEquiped = true;
                }
                BeltSlots = new ItemEntry[10];
                int[] values = {0, 2, 4, 6, 8, 1, 3, 5, 7, 9};
                var offset = (int) io.Position;
                for (var i = 0; i < 10; i++)
                {
                    io.Position = offset + values[i]*4;
                    BeltSlots[i] = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                    if (BeltSlots[i] != null)
                        BeltSlots[i].IsEquiped = true;
                }
                io.Position = offset + 40;
                AtuneSpells = new ItemEntry[14];
                for (var i = 0; i < 14; i++)
                {
                    AtuneSpells[i] = slot.Inventory.Items.GetEntryById(io.ReadUInt32(), true);
                    if (AtuneSpells[i] != null)
                        AtuneSpells[i].IsEquiped = true;
                }
            }
        }

        public SaveSlot SlotInstance { get; }
        public ItemEntry RightWeapon1 { get; set; }
        public ItemEntry RightWeapon2 { get; set; }
        public ItemEntry RightWeapon3 { get; set; }
        public ItemEntry LeftWeapon1 { get; set; }
        public ItemEntry LeftWeapon2 { get; set; }
        public ItemEntry LeftWeapon3 { get; set; }
        public ItemEntry Head { get; set; }
        public ItemEntry Chest { get; set; }
        public ItemEntry Hands { get; set; }
        public ItemEntry Legs { get; set; }
        public ItemEntry Arrow1 { get; set; }
        public ItemEntry Arrow2 { get; set; }
        public ItemEntry Bolt1 { get; set; }
        public ItemEntry Bolt2 { get; set; }
        public ItemEntry[] Rings { get; }
        public ItemEntry[] BeltSlots { get; }
        public ItemEntry[] AtuneSpells { get; internal set; }

        internal uint MakeArmorID(uint id, bool equiped)
        {
            var values = id.ToString().ToArray();
            if (equiped)
                values[0] = (char) ((byte) values[0] + 1);
            else
                values[0] = (char) ((byte) values[0] - 1);
            uint value = 0;
            var number = "";
            foreach (var v in values)
                number += v;
            if (uint.TryParse(number, out value))
                return value;
            return id;
        }

        public void WriteEquipment(StreamIO io, bool modarmor)
        {
            if (RightWeapon1.EntryIndex > 0 && RightWeapon1.IsEquiped)
                io.WriteUInt32(RightWeapon1.ID1);
            else
                io.WriteUInt32(0x33E140);

            if (LeftWeapon1.EntryIndex > 0 && LeftWeapon1.IsEquiped)
                io.WriteUInt32(LeftWeapon1.ID1);
            else
                io.WriteUInt32(0x33E140);

            if (RightWeapon2.EntryIndex > 0 && RightWeapon2.IsEquiped)
                io.WriteUInt32(RightWeapon2.ID1);
            else
                io.WriteUInt32(0x33E140);

            if (LeftWeapon2.EntryIndex > 0 && LeftWeapon2.IsEquiped)
                io.WriteUInt32(LeftWeapon2.ID1);
            else
                io.WriteUInt32(0x33E140);

            if (RightWeapon3.EntryIndex > 0 && RightWeapon3.IsEquiped)
                io.WriteUInt32(RightWeapon3.ID1);
            else
                io.WriteUInt32(0x33E140);

            if (LeftWeapon3.EntryIndex > 0 && LeftWeapon3.IsEquiped)
                io.WriteUInt32(LeftWeapon3.ID1);
            else
                io.WriteUInt32(0x33E140);

            uint val = 0xA7DD0C;
            if (Head.EntryIndex > 0 && Head.IsEquiped)
                io.WriteUInt32(modarmor ? MakeArmorID(Head.ID1, false) : MakeArmorID(Head.ID1, false) - 6439000);
            else
                io.WriteUInt32(val);
            val++;
            if (Chest.EntryIndex > 0 && Chest.IsEquiped)
                io.WriteUInt32(modarmor ? MakeArmorID(Chest.ID1, false) : MakeArmorID(Chest.ID1, false) - 6439000);
            else
                io.WriteUInt32(val);
            val++;
            if (Hands.EntryIndex > 0 && Hands.IsEquiped)
                io.WriteUInt32(modarmor ? MakeArmorID(Hands.ID1, false) : MakeArmorID(Hands.ID1, false) - 6439000);
            else
                io.WriteUInt32(val);
            val++;
            if (Legs.EntryIndex > 0 && Legs.IsEquiped)
                io.WriteUInt32(modarmor ? MakeArmorID(Legs.ID1, false) : MakeArmorID(Legs.ID1, false) - 6439000);
            else
                io.WriteUInt32(val);

            io.WriteBytes(new byte[8]);

            if (Arrow1.EntryIndex > 0 && Arrow1.IsEquiped)
                io.WriteUInt32(Arrow1.ID1);
            else
                io.WriteInt32(-1);
            if (Arrow2.EntryIndex > 0 && Arrow2.IsEquiped)
                io.WriteUInt32(Arrow2.ID1);
            else
                io.WriteInt32(-1);
            if (Bolt1.EntryIndex > 0 && Bolt1.IsEquiped)
                io.WriteUInt32(Bolt1.ID1);
            else
                io.WriteInt32(-1);
            if (Bolt2.EntryIndex > 0 && Bolt2.IsEquiped)
                io.WriteUInt32(Bolt2.ID1);
            else
                io.WriteInt32(-1);

            for (var i = 0; i < 4; i++)
            {
                if (Rings[i].EntryIndex > 0 && Rings[i].IsEquiped)
                    io.WriteUInt32(Rings[i].ID1);
                else
                    io.WriteInt32(-1);
            }
            int[] values = {0, 2, 4, 6, 8, 1, 3, 5, 7, 9};
            for (var i = 0; i < 10; i++)
            {
                if (BeltSlots[values[i]].EntryIndex > 0 && BeltSlots[values[i]].IsEquiped)
                    io.WriteUInt32(BeltSlots[values[i]].ID1);
                else
                    io.WriteInt32(-1);
            }
            for (var i = 0; i < 14; i++)
            {
                if (AtuneSpells[i].EntryIndex > 0 && AtuneSpells[i].IsEquiped)
                    io.WriteUInt32(AtuneSpells[i].ID1);
                else
                    io.WriteInt32(-1);
            }
        }

        public void WriteEquipmentIndexes(StreamIO io)
        {
            if (RightWeapon1.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(RightWeapon1));

            if (LeftWeapon1.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(LeftWeapon1));

            if (RightWeapon2.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(RightWeapon2));

            if (LeftWeapon2.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(LeftWeapon2));

            if (RightWeapon3.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(RightWeapon3));

            if (LeftWeapon3.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(LeftWeapon3));


            if (Head.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Head));

            if (Chest.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Chest));

            if (Hands.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Hands));

            if (Legs.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Legs));

            for (var i = 0; i < 4; i++)
            {
                if (Rings[i].EntryIndex == 0)
                    io.WriteInt16(-1);
                else
                    io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Rings[i]));
            }

            if (Arrow1.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Arrow1));

            if (Arrow2.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Arrow2));

            if (Bolt1.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Bolt1));

            if (Bolt2.EntryIndex == 0)
                io.WriteInt16(-1);
            else
                io.WriteInt16(SlotInstance.Inventory.GetItemIndex(Bolt2));

            int[] values = {0, 2, 4, 6, 8, 1, 3, 5, 7, 9};
            for (var i = 0; i < 10; i++)
            {
                if (BeltSlots[values[i]].EntryIndex == 0)
                    io.WriteInt16(-1);
                else
                    io.WriteInt16(SlotInstance.Inventory.GetItemIndex(BeltSlots[values[i]]));
            }

            for (var i = 0; i < 14; i++)
            {
                if (AtuneSpells[i].EntryIndex == 0)
                    io.WriteInt16(-1);
                else
                    io.WriteInt16(SlotInstance.Inventory.GetItemIndex(AtuneSpells[i]));
            }
        }
    }
}