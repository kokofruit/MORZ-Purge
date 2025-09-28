using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerController : MonoBehaviour
{
    public enum zone { zone1, zone2, zone3 }
    public zone spawnLocation;

    public GameObject tempEnemyPrefab;
    public int enemyAmount;
    public int spawnRadius;


    // [SerializeField] private int _minEnemies;
    // [SerializeField] private int _maxEnemies;

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        int enemiesSpawned = 0;
        while (enemiesSpawned < enemyAmount)
        {
            Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
            print(randomPos);
            NavMeshAgent enemyAgent = tempEnemyPrefab.GetComponent<NavMeshAgent>();
            int mask = enemyAgent != null ? enemyAgent.areaMask : 0;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, spawnRadius, mask))
            {
                print("please");
                Instantiate(tempEnemyPrefab, hit.position, quaternion.identity);
            }

            enemiesSpawned++;
        }


    }
}
