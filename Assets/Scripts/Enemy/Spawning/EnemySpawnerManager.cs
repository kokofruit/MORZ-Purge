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
        // Spawn at spawners that spawn at start
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        // spawn at every spawner that activates at level start
        foreach (EnemySpawnerController spawner in FindObjectsByType<EnemySpawnerController>(FindObjectsSortMode.None))
        {
            if (spawner.spawnAtStart)
            {
                spawner.SpawnEnemies();
            }
        }
    }
}
