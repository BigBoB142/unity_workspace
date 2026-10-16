using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : EditorWindow
{
    [MenuItem("Tools/Replace With Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceWithPrefab>("Replace With Prefab");
    }

    private GameObject prefab;

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Исходный Префаб", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Заменить выделенные обьекты") && prefab != null)
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            Undo.RegisterCompleteObjectUndo(selectedObjects, "Replace With Prefab");

            foreach (GameObject obj in selectedObjects)
            {
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                newObj.transform.position = obj.transform.position;
                newObj.transform.rotation = obj.transform.rotation;
                newObj.transform.localScale = obj.transform.localScale;
                newObj.transform.SetParent(obj.transform.parent);

                Undo.DestroyObjectImmediate(obj);
            }

            Close();


        }
    }
}
