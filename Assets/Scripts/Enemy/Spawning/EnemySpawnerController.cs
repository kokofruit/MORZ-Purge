// Moth Harper
// This script will control each enemy spawner and allow them to instantiate enemies

using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnableEnemies;
    
    // determines if the enemy spawns at start or on trigger
    public bool spawnAtStart;
    // the spawning chance table for this spawner
    [SerializeField] private SpawnTable _spawnTable;
    // the range for spawning enemies
    [SerializeField] private int _spawnRadius;
    // the range of enemies that can be spawned
    [SerializeField] private int _minEnemies;
    // inclusive max
    [SerializeField] private int _maxEnemies;
    // how many attempts TOTAL can be made for spawning somethings
    [SerializeField] private int _maxSpawningAttempts;

    public void SpawnEnemies()
    {
        // choose a random amount of enemies to spawn
        if (_spawnableEnemies.Count == 0) return;
        int enemyAmount = UnityEngine.Random.Range(_minEnemies, _maxEnemies + 1);
        // keep track of how many enemies have been spawned
        int enemiesSpawned = 0;
        // keep track of how many times spawning has been tried (to avoid an infinite loop)
        int spawningAttempts = 0;

        // instantiate the decided amount of enemies
        while ((enemiesSpawned < enemyAmount) && (spawningAttempts < _maxSpawningAttempts))
        {
            // Decide the type of enemy
            GameObject enemyPrefab = _spawnTable.ChooseItem(UnityEngine.Random.value);
            
            // Choose a random position
            Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;

            // Get the navmesh mask of the enemy
            NavMeshAgent enemyAgent = enemyPrefab.GetComponent<NavMeshAgent>();
            int mask = enemyAgent != null ? enemyAgent.areaMask : 0;

            // Attempt to find a position 
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _spawnRadius, mask))
            {
                // create an enemy of specified type at specified position
                GameObject enemy = Instantiate(enemyPrefab, hit.position, quaternion.identity);

                // increase the count of successful spawns
                enemiesSpawned++;
            }
            
            // increase the count of all attempts
            spawningAttempts++;
        }

        // destroy spawner
        Destroy(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        // if player enters trigger, spawn enemies
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnEnemies();
        }
    }
}
