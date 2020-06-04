using System.Xml;
using System.Xml.Serialization;

[XmlRoot("GlobalData")]
public class GlobalData
{
    [XmlElement("CountLevels")]
    public int CountLevels;
}
