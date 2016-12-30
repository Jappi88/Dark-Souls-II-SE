using System;
using System.Xml.Serialization;
using HavenInterface.Interfaces;

namespace Dark_Souls_II_Save_Editor
{
    [Serializable]
    [XmlRoot("Settings")]
    public class Preferences
    {
        public Preferences()
        {
            ViewTabs = new int[] {};
            AllowedDlcIndexes = new int[] {};
        }

        [XmlArrayItem]
        public int[] AllowedDlcIndexes { get; set; }

        [XmlArrayItem]
        public int[] ViewTabs { get; set; }

        [XmlAttribute]
        public bool PreventDuplicates { get; set; }

        [XmlAttribute]
        public string Language { get; set; }

        [XmlElement]
        public bool LicenceAgreed { get; set; }

        [XmlElement]
        public bool SettingsAdjusted { get; set; }

        public void Save()
        {
            Functions.SaveSettings();
        }
    }

    public static class AppGlobalInfo
    {
        public static GlobalAppSettings GlobalInfo = new GlobalAppSettings
        {
            Version = "2.0.0.5",
            Guid = "40679258-ab28-4508-80a4-816478857264",
            Company = "Jappi88",
            Title = "Dark Souls II Save Editor",
            Copyright = "Copyright © Jappi88 2014",
            Description = "Dark Souls II Save Editor",
            Build = "Debug",
            Email = ""

        };
    }
}