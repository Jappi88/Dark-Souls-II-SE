using System.Drawing;
using System.IO;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class Bonfire
    {
        public Bonfire()
        {
        }

        public Bonfire(ushort id)
        {
            Id = id;
        }

        public Bonfire(string name, ushort id, byte[] image)
        {
            Id = id;
            ImageStream = new MemoryStream(image);
            Name = name;
        }

        internal MemoryStream ImageStream { get; set; }
        public ushort AreaId { get; set; }
        public ushort Id { get; set; }
        public bool IsUnlocked { get; set; }
        public string Name { get; set; }
        public Image BonfireImage => ImageStream == null ? null : Image.FromStream(ImageStream);
    }
}