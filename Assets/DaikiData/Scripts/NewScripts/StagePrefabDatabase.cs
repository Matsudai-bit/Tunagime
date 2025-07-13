using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StagePrefabDatabase", menuName = "Stage/PrefabDatabase")]
public class StagePrefabDatabase : ScriptableObject
{
    public List<StagePrefabEntry> entries;

    public GameObject GetPrefab(string key)
    {
       
        return entries.FirstOrDefault(e => e.key == key)?.prefab;
    }
}

[System.Serializable]
public class StagePrefabEntry
{
    public string key;
    public GameObject prefab;
}
