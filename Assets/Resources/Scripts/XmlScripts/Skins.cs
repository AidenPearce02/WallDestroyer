using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Skins")]
public class Skins
{
    [XmlElement("Skin")]
    public Skin[] skins;
    public class Skin {
        
        [XmlElement("Name")]
        public string name;

        [XmlElement("Price")]
        public int price;
        
        public Skin() { }
    }
}

