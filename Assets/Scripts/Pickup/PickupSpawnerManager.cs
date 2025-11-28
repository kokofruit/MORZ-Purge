// Main Contributor: Moth Harper
// Secondary Contributor: Gabe & Gub :) & Phil :)
// Reviewer:
// Description: Spawns pickups in the level

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupSpawnerManager : MonoBehaviour
{
    // the singleton instance
    public static PickupSpawnerManager instance;

    // the amount of pickups desired
    [SerializeField, Min(0)] private int _pickupAmount;

    // Upgrade pickups
    [SerializeField] private SpawnTable[] _upgradeTables;

    // Ammo and health pickups
    [SerializeField] private SpawnTable _regularPickupTable;

    // the chance of spawning a pickup when the player destroys a breakable object
    [SerializeField, Range(0, 1)] private float _breakableSpawnRate;

    // the chance of spawning a pickup from an enemy death
    [SerializeField, Range(0, 1)] private float _enemyDeathSpawnRate;
    // the loot table for spawning from an enemy death
    [SerializeField] private SpawnTable _enemyDeathSpawnTable;

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

        // Make persistent
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnPickups(int level)
    {
        // Make a list of all available spawners in the scene
        List <PickupSpawnerController> availableSpawners = FindObjectsByType<PickupSpawnerController>(FindObjectsSortMode.None).ToList();

        // more error proofing yippee
        level = Mathf.Clamp(level, 0, _upgradeTables.Length - 1); // keep level index inside range of list
        if (_upgradeTables.Length == 0) return; // stop if the list is empty
        if (_upgradeTables[level].spawnTableEntries.Count > availableSpawners.Count) return; // stop if there's not enough spawners

        // spawn one of each upgrade
        if (level < _upgradeTables.Length) // error proofing again
        {
            foreach (SpawnTableEntry upgradePickup in _upgradeTables[level].spawnTableEntries)
            {
                // error proofing again again
                if (availableSpawners.Count < 1) return;

                // choose a random spawner to spawn at
                int index = Random.Range(0, availableSpawners.Count);

                // tell spawner to spawn the upgrade
                availableSpawners[index].SpawnPickup(upgradePickup.spawnObject);

                // remove spawner from available options
                availableSpawners.RemoveAt(index);
            }
        }

        // some error proofing
        if (_pickupAmount > availableSpawners.Count) _pickupAmount = availableSpawners.Count;

        // spawn at other pickups
        for (int i = 0; i < _pickupAmount; i++)
        {
            // choose a random spawner and spawn at it
            int index = Random.Range(0, availableSpawners.Count);

            // get a random pickup (ammo or health)
            GameObject pickup = _regularPickupTable.ChooseItem(Random.value);
            // tell spawner to spawn the pickup
            availableSpawners[index].SpawnPickup(pickup);

            // remove spawner from available options
            availableSpawners.RemoveAt(index);
        }
    }

    public bool SpawnFromEnemyDeath(out GameObject pickup)
    {
        // runs at a chance determined by the breakable spawn rate 
        if (Random.value <= _enemyDeathSpawnRate)
        {
            // retrive a random pickup
            pickup = _enemyDeathSpawnTable.ChooseItem(Random.value);
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
