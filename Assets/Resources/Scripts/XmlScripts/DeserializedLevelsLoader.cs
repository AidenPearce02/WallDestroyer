using System.Collections.Generic;
using UnityEngine;

public class DeserializedLevelsLoader
{
    // Levels deserialized
    private DeserializedLevel deserializedLevel;
    private int level;

    private const string prefabsFolder = "Prefabs/";
    public const string roofsFolder = "Roofs/";
    public const string leftLegsFolder = "LeftLegs/";
    public const string rightLegsFolder = "RightLegs/";

    struct WallStruct
    {
        public string prefab;
        public float distance;
        public Effect.EffectType booster;

        public WallStruct(string prefabGO, DeserializedLevel.Wall deserializedWall)
        {
            booster = (deserializedWall.booster != null)?(Effect.EffectType)System.Enum.Parse(typeof(Effect.EffectType), deserializedWall.booster):Effect.EffectType.NONE;
            prefab = prefabGO;
            distance = ToFloatZeroIfNull(deserializedWall.distance);
        }
    }

    // Cache prefabs in prefabDict
    Dictionary<string, GameObject> prefabPool;

    // Cache all items with locations
    private List<WallStruct> sceneWallsList;

    Transform parentOfXmlWalls;

    public const string xmlWallsGOName = "XmlWalls";

    private void Init()
    {
        prefabPool = new Dictionary<string, GameObject>();
        sceneWallsList = new List<WallStruct>();

        // if the XmlItems gameobject folder remained in the Hierarcy, then delete it
        while (GameObject.Find(xmlWallsGOName) != null)
            Object.DestroyImmediate(GameObject.Find(xmlWallsGOName));

        parentOfXmlWalls = new GameObject(xmlWallsGOName).transform;
    }

    public void GenerateItems(int level)
    {
        this.level = level;
        
        Init();

        CreateSceneWallsList();

        // Finally instantiate all items
        InstantiateItems();
    }
    private float spawnY = 4f;
    private void InstantiateItems()
    {
        List<GameObject> tempObjects = new List<GameObject>();
        int i = 0;
        foreach (WallStruct wall in sceneWallsList)
        {
            string[] mystring = wall.prefab.Split(',');
            // TODO load height coordinate from a directory
            GameObject newGameObject = new GameObject
            {
                name = wall.prefab
            };
            GameObject roofObject = Object.Instantiate(prefabPool[mystring[0]], newGameObject.transform);
            roofObject.GetComponent<Effect>().SetCurrentEffect(wall.booster);
            if(wall.distance==0) SetPos2D(roofObject, new Vector2(0, spawnY));
            else{
                SetPos2D(roofObject, new Vector2(0, tempObjects.ToArray()[i-1].transform.position.y+wall.distance));
            }
            if (mystring[1] != "null"){
                GameObject leftLegObject = Object.Instantiate(prefabPool[mystring[1]], newGameObject.transform);
                if (wall.distance == 0) SetPos2D(leftLegObject, new Vector2(leftLegObject.transform.position.x, spawnY - 0.5f));
                else
                {
                    SetPos2D(leftLegObject, new Vector2(leftLegObject.transform.position.x, tempObjects.ToArray()[i - 1].transform.position.y + wall.distance - 0.5f));
                }
                
            }
            if (mystring[2] != "null"){
                GameObject rightLegObject = Object.Instantiate(prefabPool[mystring[2]], newGameObject.transform);
                if (wall.distance == 0) SetPos2D(rightLegObject, new Vector2(rightLegObject.transform.position.x, spawnY - 0.5f));
                else
                {
                    SetPos2D(rightLegObject, new Vector2(rightLegObject.transform.position.x, tempObjects.ToArray()[i - 1].transform.position.y + wall.distance - 0.5f));
                }
            }
            // set position
            if (wall.distance == 0) SetPos2D(newGameObject, new Vector2(0, spawnY));
            else
            {
                SetPos2D(newGameObject, new Vector2(0, tempObjects.ToArray()[i - 1].transform.position.y + wall.distance));
            }
            newGameObject.gameObject.tag = "Wall";
            // set parent
            newGameObject.transform.parent = parentOfXmlWalls;
            tempObjects.Add(newGameObject);
            i++;
        }
    }

    private void CreateSceneWallsList()
    {
        deserializedLevel = XmlIO.LoadXml<DeserializedLevel>("Levels/"+level);
        if (deserializedLevel.walls!=null)
        {
            foreach (DeserializedLevel.Wall deserializedWall in deserializedLevel.walls)
            {
                string prefabRoofString = deserializedWall.prefabRoof;
                string prefabLeftLegString = "null", prefabRightLegString = "null";
                if (!prefabPool.ContainsKey(prefabRoofString))
                {
                    GameObject prefabRoofObject = Resources.Load(prefabsFolder + roofsFolder + prefabRoofString, typeof(GameObject)) as GameObject;
                    if (prefabRoofObject == null)
                    {
                        Debug.LogError("Prefab \"" + prefabRoofString + "\" does not exists.");
                        continue;
                    }
                    prefabPool.Add(prefabRoofString, prefabRoofObject);
                }
                if (deserializedWall.prefabLeftLeg != null)
                {
                    prefabLeftLegString = deserializedWall.prefabLeftLeg;
                    if (!prefabPool.ContainsKey(prefabLeftLegString))
                    {
                        GameObject prefabLeftLegObject = Resources.Load(prefabsFolder + leftLegsFolder + prefabLeftLegString, typeof(GameObject)) as GameObject;
                        if (prefabLeftLegObject == null)
                        {
                            Debug.LogError("Prefab \"" + prefabLeftLegString + "\" does not exists.");
                            continue;
                        }
                        prefabPool.Add(prefabLeftLegString, prefabLeftLegObject);
                    }
                }
                if (deserializedWall.prefabRightLeg != null)
                {
                    prefabRightLegString = deserializedWall.prefabRightLeg;
                    if (!prefabPool.ContainsKey(prefabRightLegString) && prefabRightLegString != "null")
                    {
                        GameObject prefabRightLegObject = Resources.Load(prefabsFolder + rightLegsFolder + prefabRightLegString, typeof(GameObject)) as GameObject;
                        if (prefabRightLegObject == null)
                        {
                            Debug.LogError("Prefab \"" + prefabRoofString + "\" does not exists.");
                            continue;
                        }
                        prefabPool.Add(prefabRightLegString, prefabRightLegObject);
                    }
                }
                string wallName = prefabRoofString + ',' + prefabLeftLegString + ',' + prefabRightLegString;
                WallStruct wall = new WallStruct(wallName, deserializedWall);
                sceneWallsList.Add(wall);
            }
        }
    }
    
    static float ToFloatZeroIfNull(string value) { return value == null ? 0 : float.Parse(value); }
    void SetPos2D(GameObject g, Vector2 pos)
    {
        g.transform.position = new Vector3(
            pos.x,
            pos.y,
            g.transform.position.z
        );
    }

}
