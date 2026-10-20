using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SaveIdValidator
{
    [MenuItem("Ruins/Validate Save IDs")]
    public static void Validate()
    {
        var seen = new HashSet<string>();
        int fixedCount = 0;

        foreach (var entity in Object.FindObjectsByType<SaveableId>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            var so = new SerializedObject(entity);
            SerializedProperty id = so.FindProperty("id");

            bool empty = string.IsNullOrEmpty(id.stringValue);
            if (empty || !seen.Add(id.stringValue))
            {
                id.stringValue = System.Guid.NewGuid().ToString("N");
                so.ApplyModifiedProperties();   // with Undo support
                seen.Add(id.stringValue);
                fixedCount++;
                Debug.LogWarning($"New ID for {entity.name}", entity);
            }
        }
        Debug.Log(fixedCount == 0 ? "All IDs are unique"
                                  : $"IDs fixed: {fixedCount}");
    }
}
