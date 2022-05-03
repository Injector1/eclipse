using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplatesSpawner : MonoBehaviour
{
    [SerializeField] public List<string> Names;
    [SerializeField] public List<GameObject> GameObjects;
    [SerializeField] public GameObject SpawnTo;
    public Dictionary<string, GameObject> GmObjByName;

    private void Awake()
    {
        GmObjByName = new Dictionary<string, GameObject>();
        for (var i = 0; i < Names.Count; i++)
            GmObjByName[Names[i]] = GameObjects[i];
        Check();
    }

    private void Check()
    {
        var spawnRequests = new List<GameObject>();
        for (var i = 0; i < transform.childCount; i++)
            spawnRequests.Add(transform.GetChild(i).gameObject);

        foreach (var request in spawnRequests)
        {
            var splittedName = request.name.Split('|');
            var spawnedObjName = splittedName.Length > 1 ? splittedName[1] : splittedName[0]; 
            var source = GmObjByName[splittedName[0]];
            var spawnedObj = Instantiate(source, request.transform);
            spawnedObj.transform.parent = SpawnTo.transform;
            spawnedObj.SetActive(true);
            spawnedObj.name = spawnedObjName;
            Destroy(request);
        }
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
    }
    
    /// <summary>
    /// Спавнит объект из шаблонов
    /// </summary>
    /// <param name="templateName">в формате "название шаблона" или "название шаблона|название заспавненного объекта"</param>
    /// <param name="position">поцизия спавна</param>
    public void Spawn(string templateName, Vector3 position)
    {
        var splittedName = templateName.Split('|');
        var spawnedObjName = splittedName.Length > 1 ? splittedName[1] : splittedName[0]; 
        var source = GmObjByName[splittedName[0]];
        var spawnedObj = Instantiate(source, SpawnTo.transform, true);
        spawnedObj.transform.position = position;
        spawnedObj.SetActive(true);
        spawnedObj.name = spawnedObjName;
    }
    
    /// <summary>
    /// Спавнит объект из шаблонов
    /// </summary>
    /// <param name="templateName">в формате "название шаблона" или "название шаблона|название заспавненного объекта"</param>
    public void Spawn(string templateName)
    {
        var splittedName = templateName.Split('|');
        var spawnedObjName = splittedName.Length > 1 ? splittedName[1] : splittedName[0]; 
        var source = GmObjByName[splittedName[0]];
        var spawnedObj = Instantiate(source, SpawnTo.transform, true);
        spawnedObj.SetActive(true);
        spawnedObj.name = spawnedObjName;
    }
}