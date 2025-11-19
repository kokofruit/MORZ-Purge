using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnTable", menuName = "Scriptable Objects/SpawnTable")]
public class SpawnTable : ScriptableObject
{
    public List<SpawnTableEntry> spawnTableEntries;

    public GameObject ChooseItem(float num)
    {
        // return nothing if table is empty
        if (spawnTableEntries.Count == 0) return null;

        // store cumulative chances
        float runningSum = 0;
        // find object by chance value
        foreach (var tableEntry in spawnTableEntries)
        {
            // add to sum
            runningSum += tableEntry.spawnChance;
            // if an enemy's chances align with the random value, return it ot be spawned
            if (num <= runningSum)
            {
                return tableEntry.spawnObject;
            }
        }

        // return first enemy in the event of chances that don't add up to one
        return spawnTableEntries[0].spawnObject;
    }
}
 
[Serializable]
public struct SpawnTableEntry
{
    public GameObject spawnObject;
    [Range(0f, 1f)] public float spawnChance;
}