using UnityEngine;
using System.Collections.Generic;

public class DeserializedLevelsSaver
{
    public const string xmlWallsToExportGOName = "XmlWallsToExport";

    public void SaveExportItems()
    {
        GameObject xmlWallsToExportGO;
        
        // Create XmlItemsToExport or find existing
        if (GameObject.Find(xmlWallsToExportGOName) == null)
        {
            new GameObject(xmlWallsToExportGOName);
            //we have nothing to save so skip execution
            return;
        }
        else
        {
            xmlWallsToExportGO = GameObject.Find(xmlWallsToExportGOName);
        }

        Transform[] xmlWallsToExportGOchildren = xmlWallsToExportGO.GetComponentsInChildren<Transform>();

        // Check if there isn't any Transform components except parent's
        if (xmlWallsToExportGOchildren.Length == 1)
        {
            Debug.LogError("Add the prefabs to " + xmlWallsToExportGOName);
            return;
        }

        //create list of items
        List<DeserializedLevel.Wall> wallList = new List<DeserializedLevel.Wall>();

        foreach (Transform wall in xmlWallsToExportGOchildren)
        {
            if (wall.parent == xmlWallsToExportGO.transform)
            {
                wallList.Add(new DeserializedLevel.Wall(wall));
            }
        }

        //copy list of items to the raw array
        DeserializedLevel levelsXmlToExport = new DeserializedLevel
        {
            walls = new DeserializedLevel.Wall[wallList.Count]
        };
        wallList.CopyTo(levelsXmlToExport.walls);

        string outputFilePath = "./Assets/Resources/Levels/" + xmlWallsToExportGOName + ".xml";
        XmlIO.SaveXml<DeserializedLevel>(levelsXmlToExport, outputFilePath);
    }

    public static string ToStringNullIfZero(float num)
    {
        return num == 0 ? null : MathRound(num, 2).ToString();
    }

    public static float MathRound(float round, int decimals)
    {
        return Mathf.Round(round * Mathf.Pow(10, decimals)) / Mathf.Pow(10, decimals);
    }
}
