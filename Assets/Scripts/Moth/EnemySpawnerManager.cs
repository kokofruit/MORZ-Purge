// Main Contributor: Moth Harper
// Secondary Contributor:
// Reviewer:
// Description: This script will oversee all enemy spawners and control zone spawns

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    // The singleton instance
    public static EnemySpawnerManager instance;

    // A 2D list that contains all spawners and separates them by zone
    private List<List<EnemySpawnerController>> _spawners = new();
    // the next zone to spawn
    private int _zoneIndex = 0;

    // Set the instance or destroy if it's a duplicate
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Create all sublists based on how many zones are present
        for (int i = 0; i < Enum.GetNames(typeof(EnemySpawnerController.Zone)).Length; i++)
        {
            _spawners.Add(new List<EnemySpawnerController>());
        }
        // Find all spawners in the scene and add them into the list
        foreach (EnemySpawnerController spawner in FindObjectsByType<EnemySpawnerController>(FindObjectsSortMode.None))
        {
            int listIndex = (int)spawner.spawnLocation;
            _spawners[listIndex].Add(spawner);
        }

        // TODO: HANDLE WITH GAME MANAGER
        // TEMPORARY !!!
        SpawnWave();
    }

    public void SpawnWave()
    {
        // if the zone index is out of range, return
        if (_zoneIndex >= _spawners.Count) return;

        // call the spawning function for each spawner in the current zone
        foreach (EnemySpawnerController spawner in _spawners[_zoneIndex])
        {
            spawner.SpawnEnemies();
        }

        // // increment the zone index
        // _zoneIndex++;
    }
}
