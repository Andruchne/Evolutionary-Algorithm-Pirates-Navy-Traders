using UnityEngine;
using UnityEditor;
using System.IO;

public class BoatStatsExtractor : EditorWindow
{
    private string prefabFolderPath = "Assets/Prefabs/SavedGeneration/";
    private string outputFilePath = "Assets/Docs/AgentInfo.txt";

    [MenuItem("Tools/Extract Boat Stats")]
    public static void ShowWindow()
    {
        GetWindow<BoatStatsExtractor>("Boat Stats Extractor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Boat Stats Exporter", EditorStyles.boldLabel);

        prefabFolderPath = EditorGUILayout.TextField("Prefab Folder Path", prefabFolderPath);
        outputFilePath = EditorGUILayout.TextField("Output File Path", outputFilePath);

        if (GUILayout.Button("Extract and Save"))
        {
            ExtractAgentStats();
        }
    }

    private void ExtractAgentStats()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });
        using StreamWriter writer = new StreamWriter(outputFilePath, false);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            AgentLogic agent = prefab.GetComponent<AgentLogic>();
            if (agent == null) continue;

            writer.WriteLine("______________________________");
            writer.WriteLine($"{prefab.name}:\n");
            writer.WriteLine(agent.GetInfoString());
            writer.WriteLine("\n\n");
        }

        Debug.Log($"Boat stats exported to: {outputFilePath}");
    }
}