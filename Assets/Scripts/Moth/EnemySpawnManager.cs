using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    private List<List<EnemySpawnerController>> _spawners = new();
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
        for (int i = 0; i < Enum.GetNames(typeof(EnemySpawnerController.zone)).Length; i++)
        {
            _spawners.Add(new List<EnemySpawnerController>());
        }

        foreach (EnemySpawnerController spawner in FindObjectsByType<EnemySpawnerController>(FindObjectsSortMode.None))
        {
            int listIndex = (int)spawner.spawnLocation;
            _spawners[listIndex].Add(spawner);
        }
    }

    public void SpawnWave(int zone)
    {
        if (_zoneIndex >= _spawners.Count) return;

        foreach (EnemySpawnerController spawner in _spawners[_zoneIndex])
        {
            spawner.SpawnEnemies();
        }
        _zoneIndex++;
    }
}
