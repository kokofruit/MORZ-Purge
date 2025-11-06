// Main Contributor: Moth Harper
// Secondary Contributor:
// Reviewer:
// Description: This script will oversee all enemy spawners and control zone spawns

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    // The singleton instance
    public static EnemySpawnerManager instance;

    // A list that contains all spawners in the scene
    private List<EnemySpawnerController> _spawners = new();

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

    void OnEnable()
    {
        // Find all spawners in the scene and add them into the list
        _spawners = FindObjectsByType<EnemySpawnerController>(FindObjectsSortMode.InstanceID).ToList();
        // Spawn at spawners that spawn at start
        SpawnEnemies();
    }


    public void SpawnEnemies()
    {
        // call the spawning function for each spawner
        foreach (EnemySpawnerController spawner in _spawners)
        {
            if (spawner.spawnAtStart)
            {
                spawner.SpawnEnemies();
            }
        }
    }
}
