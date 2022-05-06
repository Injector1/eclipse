using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(PrefabsSpawner))]
public class PrefabsSpawnerEditor : Editor
{
    private PrefabsSpawner _prefabsSpawner;
    private List<string> _names;
    private List<GameObject> _gameObjects;

    public void OnEnable()
    {
        _prefabsSpawner = (PrefabsSpawner) target;
        _names = _prefabsSpawner.Names;
        _gameObjects = _prefabsSpawner.GameObjects;
    }

    public override void OnInspectorGUI()
    {
        _prefabsSpawner.SpawnTo = (GameObject) EditorGUILayout.ObjectField(_prefabsSpawner.SpawnTo, typeof(GameObject), true);
        for (var i = 0; i < _names.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            
            _names[i] = EditorGUILayout.TextField(_names[i]);
            _gameObjects[i] = (GameObject) EditorGUILayout.ObjectField(_gameObjects[i], typeof(GameObject), true);


            if (GUILayout.Button("X", GUILayout.Width(15)))
            {
                _names.RemoveAt(i);
                _gameObjects.RemoveAt(i);
                break;
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        
        if (GUILayout.Button("Добавить элемент"))
        {
            _names.Add("");
            _gameObjects.Add(null);
        }
        if (GUI.changed)
            SetObjectDirty(_prefabsSpawner.gameObject);
    }

    public static void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}