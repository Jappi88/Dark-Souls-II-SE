using System;
using System.Drawing;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    [Serializable]
    public class ItemTypeInstance
    {
        public enum ItemTypes
        {
            Weapon,
            Bow,
            Staff,
            Shield,
            Head,
            Chest,
            Arm,
            Legs,
            Ring,
            Arrow,
            Item,
            Key,
            Shard,
            Spell,
            Gesture,
            Ignored,
            None
        }

        public uint StartID { get; set; }
        public uint EndID { get; set; }
        public Image Image { get; set; }

        public ItemTypes ItemType
        {
            get
            {
                if (StartID >= 10 && StartID < 130)
                    return ItemTypes.Weapon;
                if (StartID >= 130 && StartID < 140)
                    return ItemTypes.Bow;
                if (StartID >= 140 && StartID < 150)
                    return ItemTypes.Staff;
                if (StartID >= 150 && StartID < 160)
                    return ItemTypes.Shield;
                if (StartID >= 400 && StartID < 410)
                    return ItemTypes.Head;
                if (StartID >= 410 && StartID < 420)
                    return ItemTypes.Chest;
                if (StartID >= 420 && StartID < 430)
                    return ItemTypes.Arm;
                if (StartID >= 430 && StartID < 500)
                    return ItemTypes.Legs;
                if (StartID >= 500 && StartID < 600)
                    return ItemTypes.Ring;
                if (StartID >= 600 && StartID < 700)
                    return ItemTypes.Arrow;
                if (StartID >= 700 && StartID < 710)
                    return ItemTypes.Item;
                if (StartID >= 710 && StartID < 720)
                    return ItemTypes.Key;
                if (StartID >= 720 && StartID < 800)
                    return ItemTypes.Shard;
                if (StartID >= 800 && StartID < 1000)
                    return ItemTypes.Spell;
                if (StartID >= 1000 && StartID < 1010)
                    return ItemTypes.Gesture;
                if (StartID == 9999)
                    return ItemTypes.Ignored;
                return ItemTypes.None;
            }
        }
    }
}