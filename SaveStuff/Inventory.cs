using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Dark_Souls_II_Save_Editor.Properties;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class Inventory
    {
        public const int BlockMax = 1272;
        private bool _itembox;

        public Inventory(SaveSlot slot)
        {
            SlotInstance = slot;
            ReadItems();
        }

        public SaveSlot SlotInstance { get; internal set; }
        public ItemEntry[] Items { get; internal set; }
        public ItemEntry[] Keys { get; internal set; }
        public ItemEntry[] Gestures { get; internal set; }

        public bool IsItemBox
        {
            get { return _itembox; }
            set
            {
                if (value != _itembox)
                {
                    _itembox = value;
                    OnInventoryChanged();
                }
            }
        }

        public int InventoryCount
        {
            get { return Items.Where(t => t != null && t.IsInItemBox == IsItemBox).ToArray().Length; }
        }

        public event EventHandler InventoryChanged;

        public virtual void OnInventoryChanged()
        {
            InventoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearEvent()
        {
            InventoryChanged = null;
        }

        public void ReadItems()
        {
            if (SlotInstance == null || SlotInstance.SaveBlocks == null || SlotInstance.SaveBlocks.Length != 8)
                return;
            SlotInstance.DoProgress("Loading Items..", 0, 1, 1);
            var io = SlotInstance.SaveBlocks[4].GetStream(0, SlotInstance.MainInstance.IsBigEndian);
            uint inventoryindex = 0;
            var ents = new List<ItemEntry>();
            for (var i = 0; i < 0xF00; i++)
            {
                var id = io.ReadUInt32();
                if (id == 0)
                {
                    io.Position += 0xc;
                    inventoryindex++;
                    continue;
                }
                var ent = MainFile.DataBase.Items.GetEntryById(id, false);
                if (ent == null || ent.EntryIndex == 0)
                {
                    ent = new ItemEntry(new ItemTypeInstance {StartID = 10, EndID = 19})
                    {
                        Name = "Unknown Item",
                        Description = "This Item Has not been added to the database yet",
                        ID1 = id,
                        ID2 = id,
                        Index = (uint) i
                    };
                    ent.ImageStream = new MemoryStream();
                    Resources.noImage.Save(ent.ImageStream, ImageFormat.Png);
                }
                ent = ent.Clone();
                ent.Index = (uint) i;
                SlotInstance.DoProgress("Loading " + SlotInstance.Name + " Items : " + ent.Name,
                    Functions.GetPercentage(i, 0xF00), 1, 1);
                ent.Data = io.ReadBytes(12, false);
                ents.Add(ent);
                //if (ent.IsInItemBox)
                //    ent.Index = boxindex++;
                //else
            }
            Items = ents.ToArray();
            io.Position += 4;
            inventoryindex = 0;
            uint xges = 0;
            ents = new List<ItemEntry>();
            var gest = new List<ItemEntry>();
            for (var i = 0; i < 0x100; i++)
            {
                var id = io.ReadUInt32();
                if (id == 0)
                {
                    io.Position += 0xc;
                    continue;
                }
                var ent = MainFile.DataBase.Gestures.GetEntryById(id, false);
                if (ent == null || ent.EntryIndex == 0)
                    ent = MainFile.DataBase.Items.GetEntryById(id, false);
                if (ent == null || ent.EntryIndex == 0)
                {
                    ent = new ItemEntry(new ItemTypeInstance {StartID = 710, EndID = 719})
                    {
                        Name = "Unknown Item",
                        Description = "This Item Has not been added to the database yet",
                        ID1 = id,
                        ID2 = id
                    };
                    ent.ImageStream = new MemoryStream();
                    Resources.noImage.Save(ent.ImageStream, ImageFormat.Png);
                }
                ent = ent.Clone();
                SlotInstance.DoProgress("Loading " + SlotInstance.Name + " Items : " + ent.Name,
                    Functions.GetPercentage(i, 248), 1, 1);
                ent.Data = io.ReadBytes(12, false);
                ent.Index = ent.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture ? xges++ : inventoryindex++;
                if (ent.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                    gest.Add(ent);
                else
                    ents.Add(ent);
            }
            Keys = ents.ToArray();
            Gestures = gest.ToArray();
            SlotInstance.DoProgress("Loading Items Complete!", 100, 1, 1);
            OnInventoryChanged();
        }

        public void Reload()
        {
            foreach (var item in Items)
            {
                var db = MainFile.DataBase.Items.FirstOrDefault(x => x.ID1 == item.ID1);
                db?.CopyOver(item, false);
            }

            foreach (var item in Keys)
            {
                var db = MainFile.DataBase.Items.FirstOrDefault(x => x.ID1 == item.ID1);
                db?.CopyOver(item, false);
            }
            foreach (var item in Gestures)
            {
                var db = MainFile.DataBase.Gestures.FirstOrDefault(x => x.ID1 == item.ID1);
                db?.CopyOver(item, false);
            }
            OnInventoryChanged();
        }

        public int RemoveDuplicates()
        {
            var items = new List<ItemEntry>();
            var keys = new List<ItemEntry>();
            var gestures = new List<ItemEntry>();
            foreach (var item in Items.Where(item => items.All(x => x.ID1 != item.ID1)))
            {
                items.Add(item);
            }
            foreach (var item in Keys.Where(item => keys.All(x => x.ID1 != item.ID1)))
            {
                keys.Add(item);
            }
            var itemsremoved = Items.Length - items.Count;
            var keysremoved = Keys.Length - keys.Count;
            var gestremoved = Gestures.Length - gestures.Count;
            Items = items.ToArray();
            Keys = keys.ToArray();
            Gestures = gestures.ToArray();
            OnInventoryChanged();
            return itemsremoved + keysremoved + gestremoved;
        }

        public int RemoveAllDlcItems()
        {
            var items = new List<ItemEntry>();
            var keys = new List<ItemEntry>();
            var gest = new List<ItemEntry>();
            items.AddRange(
                Items.Where(t => !t.IsDlc || (t.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(t.DlcVersion))));
            keys.AddRange(Keys.Where(t => !t.IsDlc || (t.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(t.DlcVersion))));
            gest.AddRange(
                Gestures.Where(
                    t => !t.IsDlc || (t.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(t.DlcVersion))));
            var itemsremoved = Items.Length - items.Count;
            var keysremoved = Keys.Length - keys.Count;
            var gestremoved = Gestures.Length - gest.Count;
            Items = items.ToArray();
            Keys = keys.ToArray();
            Gestures = gest.ToArray();
            OnInventoryChanged();
            return itemsremoved + keysremoved + gestremoved;
        }

        public void WipeOutInventory()
        {
            Items = Items.Where(t => t.IsInItemBox != IsItemBox).ToArray();
            Keys = Keys.Where(t => t.IsInItemBox != IsItemBox).ToArray();
            OnInventoryChanged();
        }

        public int EnsureEveryItem()
        {
            var list = Items.ToList();
            var list2 = Keys.ToList();
            var count = 0;
            foreach (
                var item in
                    MainFile.DataBase.Items.Where(
                        item =>
                            !item.IsIgnored || (item.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(item.DlcVersion)))
                )
            {
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                    if (list2.FirstOrDefault(x => x != null && x.ID1 == item.ID1 && x.IsInItemBox == IsItemBox) != null)
                        continue;
                if (list.FirstOrDefault(x => x != null && x.ID1 == item.ID1 && x.IsInItemBox == IsItemBox) != null)
                    continue;
                var newitem = item.Clone();
                if (newitem.Data == null)
                {
                    newitem.GenerateData(IsItemBox);
                    SlotInstance.UserUI?.SetItemDefault(newitem);
                }
                else
                {
                    newitem.IsInItemBox = IsItemBox;
                }
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                    list2.Add(newitem);
                else
                    list.Add(newitem);
                count++;
            }
            Items = list.ToArray();
            Keys = list2.ToArray();
            OnInventoryChanged();
            return count;
        }

        public int EnsureEveryDlc()
        {
            if (SlotInstance.AllowdDlcIndexes == null || SlotInstance.AllowdDlcIndexes.Length == 0)
                return 0;
            var list = Items.ToList();
            var list2 = Keys.ToList();
            var count = 0;
            foreach (
                var item in
                    MainFile.DataBase.Items.Where(
                        x => x.IsDlc && SlotInstance.AllowdDlcIndexes.Contains(x.DlcVersion) && !x.IsIgnored))
            {
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                    if (list2.Any(x => x != null && x.ID1 == item.ID1 && x.IsInItemBox == IsItemBox && x.IsDlc))
                        continue;
                if (list.Any(x => x != null && x.ID1 == item.ID1 && x.IsInItemBox == IsItemBox && x.IsDlc))
                    continue;
                var newitem = item.Clone();
                newitem.GenerateData(IsItemBox);
                SlotInstance.UserUI?.SetItemDefault(newitem);
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                    list2.Add(newitem);
                else
                    list.Add(newitem);
                count++;
            }
            Items = list.ToArray();
            Keys = list2.ToArray();
            OnInventoryChanged();
            return count;
        }

        public void DeleteItem(ItemEntry item, bool all)
        {
            var found = false;
            var xitems = Items.ToList();
            var xitems1 = Keys.ToList();
            for (var i = 0; i < xitems.Count; i++)
            {
                if (xitems[i] != null && xitems[i].ID1 == item.ID1 && xitems[i].IsInItemBox == item.IsInItemBox)
                {
                    xitems.RemoveAt(i--);
                    found = true;
                    if (!all)
                        break;
                }
            }
            if (!found || all)
            {
                for (var i = 0; i < xitems1.Count; i++)
                {
                    if (xitems1[i] != null && xitems1[i].ID1 == item.ID1 && xitems1[i].IsInItemBox == item.IsInItemBox)
                    {
                        xitems1.RemoveAt(i--);
                        if (!all)
                            break;
                    }
                }
            }
            Items = xitems.ToArray();
            Keys = xitems1.ToArray();
        }

        public ItemEntry AddItem(ItemEntry item, int quantity, bool overwrite)
        {
            if (item == null || item.IsIgnored)
                return null;
            if (item.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(item.DlcVersion))
                throw new Exception("Could not add " + item.Name + "\nDLC Items are only allowed for donators.");
            var freespace = GetFreeSpaceForItem(item);
            if (freespace < quantity)
                throw new Exception("Could Not Add " + item.Name + "!\nYou are only allowed to have " + BlockMax +
                                    " items of each block");
            var added = false;
            var exist = GetItem(item) != null;
            if (exist && !overwrite)
                return item;
            item = item.Clone();
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
            {
                var ents = Keys.ToList();
                if (item.Data == null)
                    item.GenerateData(IsItemBox);
                else item.IsInItemBox = IsItemBox;
                for (var i = 0; i < quantity; i++)
                {
                    item = item.Clone();
                    ents.Add(item);
                    added = true;
                }
                Keys = ents.ToArray();
            }
            else if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
            {
                var ents = Gestures.ToList();
                if (item.Data == null)
                    item.GenerateData(IsItemBox);
                else item.IsInItemBox = IsItemBox;
                for (var i = 0; i < quantity; i++)
                {
                    item = item.Clone();
                    ents.Add(item);
                    added = true;
                }
                Gestures = ents.ToArray();
            }
            else
            {
                var ents = Items.ToList();
                if (item.Data == null)
                    item.GenerateData(IsItemBox);
                else item.IsInItemBox = IsItemBox;
                for (var i = 0; i < quantity; i++)
                {
                    item = item.Clone();
                    ents.Add(item);
                    added = true;
                }
                Items = ents.ToArray();
            }
            if (!added)
                throw new Exception("Could not add " + item.Name + "! " + (IsItemBox ? "ItemBox" : "Inventory") +
                                    " is full!");
            return item;
        }

        public void CloneItem(ItemEntry item)
        {
            var freespace = GetFreeSpaceForItem(item);
            if (freespace < 1)
                throw new Exception("Could Not Duplicate " + item.Name + "!\nYou are only allowed to have " + BlockMax +
                                    " items of each block");
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key ||
                item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
            {
                var ents = Keys.ToList();
                if (item.Data == null)
                    item.GenerateData(item.Type.ItemType != ItemTypeInstance.ItemTypes.Gesture &&
                                      item.Type.ItemType != ItemTypeInstance.ItemTypes.Key && IsItemBox);
                ents.Add(item.Clone());
                Keys = ents.ToArray();
            }
            else
            {
                var ents = Items.ToList();
                if (item.Data == null)
                    item.GenerateData(IsItemBox);
                ents.Add(item.Clone());
                Items = ents.ToArray();
            }
        }

        public short GetItemIndex(ItemEntry item)
        {
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
            {
                for (short i = 0; i < Gestures.Length; i++)
                {
                    if (Gestures[i].ID1 == item.ID1)
                        return i;
                }
                return -1;
            }
            if (item.Type.ItemType != ItemTypeInstance.ItemTypes.Key)
            {
                return XGetItemIndex(item);
            }
            return -1;
        }

        public ItemEntry[] GetItems(ItemEntry item)
        {
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                return
                    Keys.Where(t => t != null && t.ID1 == item.ID1 && !t.IsIgnored && t.IsInItemBox == IsItemBox)
                        .ToArray();
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                return Gestures.Where(t => t.ID1 == item.ID1 && !t.IsIgnored).ToArray();
            return Items.Where(t => t.ID1 == item.ID1 && !t.IsIgnored && t.IsInItemBox == IsItemBox).ToArray();
        }

        public void Build()
        {
            if (SlotInstance == null || SlotInstance.SaveBlocks == null || SlotInstance.SaveBlocks.Length != 8)
                return;
            var io = SlotInstance.SaveBlocks[4].GetStream(0, SlotInstance.MainInstance.IsBigEndian);
            uint inventoryindex = 0;

            var weapons = new List<ItemEntry>();
            var xitems = new List<ItemEntry>();
            var armor = new List<ItemEntry>();
            var rest = new List<ItemEntry>();
            foreach (var t in Items.Where(t => t != null))
            {
                if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Weapon ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Shield ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Bow ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Staff ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Ring)
                    weapons.Add(t);
                else if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                    xitems.Add(t);
                else if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Head ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Chest ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Arm ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Legs)
                {
                    armor.Add(t);
                }
                else
                {
                    rest.Add(t);
                }
            }
            if (rest.Count > 0)
                weapons.AddRange(rest.ToArray());
            int blockindex = 0, itemindex = 0, weaponindex = 0, armorindex = 0;
            for (var i = 0; i < 0xf00; i++)
            {
                ItemEntry item = null;
                switch (blockindex)
                {
                    case 0:
                        if (weaponindex < weapons.Count)
                            item = weapons[weaponindex];
                        weaponindex++;
                        break;
                    case 1:
                        if (itemindex < xitems.Count)
                            item = xitems[itemindex];
                        itemindex++;
                        break;
                    case 2:
                        if (armorindex < armor.Count)
                            item = armor[armorindex];
                        armorindex++;
                        break;
                }
                if (item == null)
                {
                    SlotInstance.DoProgress("Writing Items..", Functions.GetPercentage(i, 0xf00), i, 0xf00);
                    io.WriteBytes(new byte[0x10], false);
                }
                else
                {
                    SlotInstance.DoProgress("Writing Items : " + item.Name, Functions.GetPercentage(i, 0xf00), i, 0xf00);
                    io.WriteBytes(item.ToArray(), false);
                }
                if (item != null)
                    item.Index = (uint) i;
                if (blockindex == 0 && weaponindex%0x6a == 0)
                    blockindex = 1;
                else if (blockindex == 1 && itemindex%0x6a == 0)
                    blockindex = 2;
                else if (blockindex == 2 && armorindex%0x6a == 0)
                    blockindex = 0;
            }
            //io.WriteBytes(new byte[(0xf00 - written) * 0x10], false);
            io.Position += 4;
            inventoryindex = 0;
            //ItemEntry[] temp = Gestures.Where(x => x != null && x.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture).ToArray();
            var count = 0;
            foreach (var item in Gestures)
            {
                SlotInstance.DoProgress("Writing Gesture : " + item.Name,
                    Functions.GetPercentage(count, Gestures.Length), count, Gestures.Length);
                if (item.Data == null)
                    item.GenerateData(false);
                io.WriteUInt32(item.ID1);
                io.WriteBytes(item.Data, false);
                item.Index = (uint) count;
                count++;
            }
            //  temp = Others.Where(x => x.Type.ItemType == ItemTypeInstance.ItemTypes.Key).ToArray();
            count = 0;
            foreach (var item in Keys)
            {
                SlotInstance.DoProgress("Writing Key : " + item.Name, Functions.GetPercentage(count, Keys.Length), count,
                    Keys.Length);
                if (item.Data == null)
                    item.GenerateData(IsItemBox);
                io.WriteUInt32(item.ID1);
                io.WriteBytes(item.Data, false);
                item.Index = inventoryindex++;
                count++;
            }
            io.WriteBytes(new byte[(0x100 - (Gestures.Length + Keys.Length))*0x10], false);
            var items = new List<ItemEntry>();
            items.AddRange(weapons);
            items.AddRange(xitems);
            items.AddRange(armor);
            Items = items.ToArray();
            var xio = SlotInstance.SaveBlocks[0].GetStream(0x16c, true);
            SlotInstance.Equipments = new Equipment(SlotInstance, xio.ReadBytes(0xB0, false));
            io.Position = 0x10004;
            SlotInstance.Equipments.WriteEquipmentIndexes(io);
        }

        public ItemEntry[] GetAllItems()
        {
            return Items.Where(t => t != null && t.IsInItemBox == IsItemBox && !t.IsIgnored).ToArray();
        }

        internal short XGetItemIndex(ItemEntry item)
        {
            var ent = GetAllItemsInBlocks().FirstOrDefault(x => x != null && x.EntryIndex > 0 && x.ID1 == item.ID1);
            return (short) (ent != null ? (short) ent.Index : -1);
        }

        public ItemEntry[] GetAllItemsInBlocks()
        {
            var weapons = new List<ItemEntry>();
            var xitems = new List<ItemEntry>();
            var armor = new List<ItemEntry>();

            foreach (var t in Items)
            {
                if (t == null)
                    continue;
                if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Weapon ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Shield ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Bow ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Staff ||
                    t.Type.ItemType == ItemTypeInstance.ItemTypes.Ring)
                    weapons.Add(t);
                else if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                    xitems.Add(t);
                else if (t.Type.ItemType == ItemTypeInstance.ItemTypes.Head ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Chest ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Arm ||
                         t.Type.ItemType == ItemTypeInstance.ItemTypes.Legs)
                {
                    armor.Add(t);
                }
            }
            var result = new List<ItemEntry>();
            int blockindex = 0, itemindex = 0, weaponindex = 0, armorindex = 0;
            for (var i = 0; i < 0xf00; i++)
            {
                ItemEntry item = null;
                switch (blockindex)
                {
                    case 0:
                        if (weaponindex < weapons.Count)
                            item = weapons[weaponindex];
                        weaponindex++;
                        break;
                    case 1:
                        if (itemindex < xitems.Count)
                            item = xitems[itemindex];
                        itemindex++;
                        break;
                    case 2:
                        if (armorindex < armor.Count)
                            item = armor[armorindex];
                        armorindex++;
                        break;
                }
                result.Add(item ?? new ItemEntry(new ItemTypeInstance()));
                if (item != null)
                    item.Index = (uint) (result.Count - 1);
                if (blockindex == 0 && weaponindex%0x6a == 0)
                    blockindex = 1;
                else if (blockindex == 1 && itemindex%0x6a == 0)
                    blockindex = 2;
                else if (blockindex == 2 && armorindex%0x6a == 0)
                    blockindex = 0;
            }
            return result.ToArray();
        }

        public ItemEntry[] GetItems(ItemTypeInstance.ItemTypes type)
        {
            if (type == ItemTypeInstance.ItemTypes.Key)
                return
                    Keys.Where(t => t != null && t.IsInItemBox == IsItemBox && !t.IsIgnored && t.Type.ItemType == type)
                        .ToArray();
            if (type == ItemTypeInstance.ItemTypes.Gesture)
                return Gestures.Where(t => !t.IsIgnored && t.Type.ItemType == type).ToArray();
            return Items.Where(t => t.IsInItemBox == IsItemBox && !t.IsIgnored && t.Type.ItemType == type).ToArray();
        }

        public ItemEntry GetItem(ItemEntry item, bool initembox)
        {
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                return
                    Keys.FirstOrDefault(
                        t => t != null && t.ID1 == item.ID1 && !t.IsIgnored && t.IsInItemBox == initembox);
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                return
                    Gestures.FirstOrDefault(
                        t => t != null && t.ID1 == item.ID1 && !t.IsIgnored && t.IsInItemBox == initembox);
            return
                Items.FirstOrDefault(
                    t =>
                        t != null && t.ID1 == item.ID1 && !t.IsIgnored && t.IsInItemBox == initembox &&
                        item.IsEquiped == t.IsEquiped);
        }

        public ItemEntry GetItem(ItemEntry item)
        {
            return GetItem(item, IsItemBox);
        }

        public int GetFreeSpaceForItem(ItemEntry item)
        {
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key ||
                item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                return 0x100 -
                       (Keys.Where(x => x != null).ToArray().Length + Gestures.Where(x => x != null).ToArray().Length);
            if (Items != null)
                return BlockMax - Items.Where(x => x.BlockIndex == item.BlockIndex).ToArray().Length;
            return BlockMax;
        }

        public void Close()
        {
            Items = null;
            Keys = null;
            Gestures = null;
            SlotInstance = null;
        }
    }
}