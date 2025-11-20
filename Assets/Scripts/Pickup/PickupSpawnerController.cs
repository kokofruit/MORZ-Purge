// Moth Harper
// This script will control each pickup spawner and allow them to instantiate a certain pickup

using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerController : MonoBehaviour
{
    public void SpawnPickup(GameObject pickup)
    {
        Instantiate(pickup, transform.position, quaternion.identity);
    }
}
