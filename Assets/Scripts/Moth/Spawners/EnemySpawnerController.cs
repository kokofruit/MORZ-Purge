// Moth Harper
// This script will control each enemy spawner and allow them to instantiate enemies

using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerController : MonoBehaviour
{
    public enum Zone { zone1, zone2, zone3 }
    public Zone spawnLocation;

    [SerializeField] private List<GameObject> _spawnableEnemies;
    [SerializeField] private int _minEnemies;
    [Tooltip("Exclusive maximum")]
    [SerializeField] private int _maxEnemies;
    [SerializeField] private int _spawnRadius;
    [SerializeField] private int _maxSpawningAttempts;

    public void SpawnEnemies()
    {
        // choose a random amount of enemies to spawn
        if (_spawnableEnemies.Count == 0) return;
        int enemyAmount = UnityEngine.Random.Range(_minEnemies, _maxEnemies);
        // keep track of how many enemies have been spawned
        int enemiesSpawned = 0;
        // keep track of how many times spawning has been tried (to avoid an infinite loop)
        int spawningAttempts = 0;
        // instantiate the decided amount of enemies
        while ((enemiesSpawned < enemyAmount) && (spawningAttempts < _maxSpawningAttempts))
        {
            // Decide the type of enemy
            GameObject enemyPrefab = RandomEnemy();
            // Choose a random position
            Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;
            // Get the navmesh mask of the enemy
            NavMeshAgent enemyAgent = enemyPrefab.GetComponent<NavMeshAgent>();
            int mask = enemyAgent != null ? enemyAgent.areaMask : 0;
            // Attempt to find a position 
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _spawnRadius, mask))
            {
                // create an enemy of specified type at specified position
                GameObject enemy = Instantiate(RandomEnemy(), hit.position, quaternion.identity);

                // increase the count of successful spawns
                enemiesSpawned++;
            }
            // increase the count of all attempts
            spawningAttempts++;
        }
    }

    private GameObject RandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, maxExclusive: _spawnableEnemies.Count);
        return _spawnableEnemies[randomIndex];
    }
}
