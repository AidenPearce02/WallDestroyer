using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Level")]
public class DeserializedLevel
{
    [XmlElement("Wall")]
    public Wall[] walls;
    public class Wall {
        [XmlAttribute("distance")]
        public string distance;
        
        [XmlElement("booster")]
        public string booster;

        [XmlElement("prefabRoof")]
        public string prefabRoof;

        [XmlElement("prefabLeftLeg")]
        public string prefabLeftLeg;

        [XmlElement("prefabRightLeg")]
        public string prefabRightLeg;

        public Wall() { }
        public Wall(Transform wall)
        {
            distance = DeserializedLevelsSaver.ToStringNullIfZero(wall.transform.position.y);
            foreach (Transform prefab in wall)
            {
                if (prefab.name.Contains("Roof"))
                {
                    booster = prefab.gameObject.GetComponent<Effect>().CurrentEffect.ToString();
                    prefabRoof = prefab.name.Replace("(Clone)","");
                }
                if (prefab.name.Contains("LeftLeg"))
                {
                    prefabLeftLeg = prefab.name.Replace("(Clone)", "");
                }
                if (prefab.name.Contains("RightLeg"))
                {
                    prefabRightLeg = prefab.name.Replace("(Clone)", "");
                }
            }
        }
    }
}

