using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsSpawner : MonoBehaviour
{
    [SerializeField] public List<string> Names;
    [SerializeField] public List<GameObject> GameObjects;
    [SerializeField] public GameObject SpawnTo;
    public Dictionary<string, GameObject> GmObjByName;
    public event Action<GameObject> OnEntitySpawn;

    private void Awake()
    {
        GmObjByName = new Dictionary<string, GameObject>();
        for (var i = 0; i < Names.Count; i++)
            GmObjByName[Names[i]] = GameObjects[i];
    }

    /// <summary>
    /// Спавнит объект из шаблонов
    /// </summary>
    /// <param name="templateName">в формате "название шаблона" или "название шаблона|название заспавненного объекта"</param>
    /// <param name="position">поцизия спавна</param>
    /// <param name="rotation">поворот при спавне</param>
    public void Spawn(string templateName, Vector3 position, float rotation)
    {
        var splittedName = templateName.Split('|');
        var spawnedObjName = splittedName.Length > 1 ? splittedName[1] : splittedName[0]; 
        var source = GmObjByName[splittedName[0]];
        var spawnedObj = Instantiate(source, SpawnTo.transform, true);
        spawnedObj.transform.position = position;
        spawnedObj.transform.eulerAngles = new Vector3(0, 0, rotation);
        spawnedObj.SetActive(true);
        spawnedObj.name = spawnedObjName;
        OnEntitySpawn?.Invoke(spawnedObj);
    }
    
    /// <summary>
    /// Спавнит объект из шаблонов
    /// </summary>
    /// <param name="templateName">в формате "название шаблона" или "название шаблона|название заспавненного объекта"</param>
    /// <param name="position">поцизия спавна</param>
    public void Spawn(string templateName, Vector3 position)
    {
        Spawn(templateName, position, 0);
    }
    
    /// <summary>
    /// Спавнит объект из шаблонов
    /// </summary>
    /// <param name="templateName">в формате "название шаблона" или "название шаблона|название заспавненного объекта"</param>
    public void Spawn(string templateName)
    {
        Spawn(templateName, Vector3.zero, 0);
    }
}