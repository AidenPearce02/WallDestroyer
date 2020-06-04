using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
public class XmlLevelEditor : EditorWindow
{

    DeserializedLevelsLoader deserializedLevelsLoader;
    DeserializedLevelsSaver deserializedLevelsSaver;
    DeserializedLevelsCrossChecker deserializedLevelsCrossChecker;

    readonly string importGOName = DeserializedLevelsLoader.xmlWallsGOName;
    readonly string exportGOName = DeserializedLevelsSaver.xmlWallsToExportGOName;
    private int index = 0;

    [MenuItem("Window/Xml Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(XmlLevelEditor));
    }

    void OnGUI()
    {
        // create DeserializedLevelsLoader, Saver and CrossChecker instances
        Init();

        // Import section
        GUILayout.Label("Import", EditorStyles.boldLabel);
        GUILayout.Label("Import Levels.xml into the scene");
        var xmlTextAssets = Resources.LoadAll("Levels/", typeof(TextAsset)).Cast<TextAsset>();
        List<string> options = new List<string>();
        foreach (var item in xmlTextAssets.ToArray())
        {
            options.Add(item.name);
        }
        
        index = EditorGUILayout.Popup(index, options.ToArray());
        if (GUILayout.Button("Import Level " + options.ToArray()[index]))
            deserializedLevelsLoader.GenerateItems(int.Parse(options.ToArray()[index]));

        // Export section
        GUILayout.Label("Export", EditorStyles.boldLabel);
        GUILayout.Label("Export children of \"" + exportGOName + "\" GameObject into " + exportGOName + ".xml", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Export " + exportGOName))
            deserializedLevelsSaver.SaveExportItems();


        // Delete section
        GUILayout.Label("Delete", EditorStyles.boldLabel);
        GUILayout.Label("Delete " + importGOName + " and " + exportGOName + " GameObjects from scene", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Delete"))
        {
            DestroyImmediate(GameObject.Find(importGOName));
            DestroyImmediate(GameObject.Find(exportGOName));
        }


        // Cross check section
        GUILayout.Label("Cross Check", EditorStyles.boldLabel);
        GUILayout.Label("Cross check /Resources/Prefabs and Levels.xml if there are any item prefabs that exist only in one but not the other", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Cross Check"))
            deserializedLevelsCrossChecker.CrossCheck();
    }

    private void Init()
    {
        if (deserializedLevelsLoader == null) deserializedLevelsLoader = new DeserializedLevelsLoader();
        if (deserializedLevelsSaver == null) deserializedLevelsSaver = new DeserializedLevelsSaver();
        if (deserializedLevelsCrossChecker == null) deserializedLevelsCrossChecker = new DeserializedLevelsCrossChecker();
    }

}
#endif