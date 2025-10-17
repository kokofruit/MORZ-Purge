// Main Contributor: Moth Harper
// Secondary Contributor: Gabe & Phil :) & Gub :(
// Reviewer:
// Description: Spawns pickups in the level

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupSpawnerManager : MonoBehaviour
{
    // the singleton instance
    public static PickupSpawnerManager instance;

    // NOT ENOUGH BALLS
    [SerializeField] private GameObject[] _pickupObjects;
    // the amount of pickups desired
    [SerializeField, Min(0)] private int _pickupAmount;
    // the amount of ammo vs health pickups
    [SerializeField, Range(0, 1)] private float _ammoToHealthRatio;

    // the chance of spawning a pickup when the player destroys a breakable object
    [SerializeField, Range(0, 1)] private float _breakableSpawnRate;

    private List<PickupSpawnerController> _spawners;

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

    public void SpawnPickups()
    {
        // Find all spawners in the scene and add them into the list
        _spawners = FindObjectsByType<PickupSpawnerController>(FindObjectsSortMode.None).ToList();

        // some error proofing
        if (_pickupAmount == 0) return;
        if (_pickupAmount > _spawners.Count) _pickupAmount = _spawners.Count;

        // set amounts for each pickup
        int ammoAmount = (int)Mathf.Round(_ammoToHealthRatio * _pickupAmount);
        int healthAmout = _pickupAmount - ammoAmount;

        // create a list of unused spawners
        List<PickupSpawnerController> availableSpawners = _spawners;
        // create ammo pickups
        for (int i = 0; i < ammoAmount; i++)
        {
            int index = Random.Range(0, availableSpawners.Count);
            availableSpawners[index].GetComponent<PickupSpawnerController>()?.CreatePickup(_pickupObjects[(i % 3 == 0) ? 1 : (i % 2 == 0) ? 3 : 2]);
            availableSpawners.RemoveAt(index);
        }
        // create health pickups
        for (int i = 0; i < healthAmout; i++)
        {
            int index = Random.Range(0, availableSpawners.Count);
            availableSpawners[index].GetComponent<PickupSpawnerController>()?.CreatePickup(_pickupObjects[0]);
            availableSpawners.RemoveAt(index);
        }
    }

    public bool SpawnFromBreakable(out GameObject pickup)
    {
        // runs at a chance determined by the breakable spawn rate 
        if (Random.value <= _breakableSpawnRate)
        {
            // possibly return an ammo pickup
            if (Random.value <= _ammoToHealthRatio)
            {
                // return a random type of ammo
                int ammoType = Random.Range(1, 4);
                pickup = _pickupObjects[ammoType];
            }
            // otherwise, return a health pickup
            else
            {
                pickup = _pickupObjects[0];
            }
            // return true
            return true;
        }
        // if not spawning a pickup, return false and a null value
        else
        {
            pickup = null;
            return false;
        }
    }
}
