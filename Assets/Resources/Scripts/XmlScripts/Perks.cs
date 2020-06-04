using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
[XmlRoot("Perks")]
public class Perks
{
    [XmlElement("Perk")]
    public Perk[] perks;
    public class Perk
    {
        [XmlElement("Name")]
        public string name;
        [XmlElement("Description")]
        public string description;
        [XmlElement("Level")]
        public int level;
        public Perk() { }
        public Perk(string name,string description, int level)
        {
            this.name = name;
            this.description = description;
            this.level = level;
        }
    }
}
