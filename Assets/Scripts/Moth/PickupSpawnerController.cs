using Unity.Mathematics;
using UnityEngine;

public class PickupSpawnerController : MonoBehaviour
{
    public void CreatePickup(GameObject pickup)
    {
        Instantiate(pickup, transform.position, quaternion.identity);
    }
}
