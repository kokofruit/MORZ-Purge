// Moth Harper
// This script will control each pickup spawner and allow them to instantiate a certain pickup

using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerController : MonoBehaviour
{
    [SerializeField] SpawnTable _spawnTable;

    public void SpawnPickup()
    {
        Instantiate(_spawnTable.ChooseItem(UnityEngine.Random.value), transform.position, quaternion.identity);
    }
    
    public void CreatePickup(GameObject pickup)
    {
        SpawnPickup();
        // Instantiate(pickup, transform.position, quaternion.identity);
    }
}
