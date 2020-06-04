using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DeserializedLevelsCrossChecker
{
    // cross check /Resources/Prefabs and Levels.xml if there are any item prefabs that exist only in one but not the other
    public void CrossCheck()
    {
        // create a two sets of prefabs
        HashSet<string> resPrefabSet = new HashSet<string>();
        HashSet<string> xmlPrefabSet = new HashSet<string>();

        // Get prefabs from Levels.xml
        GetLevelPrefabs(xmlPrefabSet);

        // Get prefabs from the /Resources/Prefabs folder
        GetResPrefabs(resPrefabSet);

        // Cross checks
        foreach (string prefab in xmlPrefabSet.Except(resPrefabSet).ToList())
            Debug.LogError(prefab + " is missing in the /Resorces/Prefabs folder but used in Levels.xml");

        foreach (string prefab in resPrefabSet.Except(xmlPrefabSet).ToList())
            Debug.Log(prefab + " exists in the /Resorces/Prefabs folder but not used in Levels.xml");

        Debug.Log("Cross Check Done");
    }

    public static void GetResPrefabs(HashSet<string> resPrefabList)
    {
        // get all child items in the /Resources/Prefabs folder
        DirectoryInfo dirRoofs = new DirectoryInfo("Assets/Resources/Prefabs/Roofs");
        DirectoryInfo dirLeftLegs = new DirectoryInfo("Assets/Resources/Prefabs/LeftLegs");
        DirectoryInfo dirRightLegs = new DirectoryInfo("Assets/Resources/Prefabs/RightLegs");
        AddToList(dirRoofs,resPrefabList);
        AddToList(dirLeftLegs, resPrefabList);
        AddToList(dirRightLegs, resPrefabList);
    }

    private static void AddToList(DirectoryInfo temp, HashSet<string> resPrefabList) {
        FileInfo[] fileInfos = temp.GetFiles("*.prefab");
        fileInfos.Select(f => f.FullName).ToArray();

        // Add each prefab's file name to prefabList and truncate the .prefab extension from the end
        foreach (FileInfo fileInfo in fileInfos)
        {
            resPrefabList.Add(fileInfo.Name.Substring(0, fileInfo.Name.Length - ".prefab".Length));
        }
    }

    private static void GetLevelPrefabs(HashSet<string> xmlPrefabSet)
    {
        DeserializedLevel[] levels = XmlIO.LoadAllXmlLevels<DeserializedLevel>().ToArray(); 
        foreach (DeserializedLevel level in levels) {
            foreach (DeserializedLevel.Wall wall in level.walls)
            {
                xmlPrefabSet.Add(wall.prefabRoof);
                if (wall.prefabLeftLeg != null)
                    xmlPrefabSet.Add(wall.prefabLeftLeg);
                if (wall.prefabRightLeg != null)
                    xmlPrefabSet.Add(wall.prefabRightLeg);
            }
        }
        
        
    }

}
