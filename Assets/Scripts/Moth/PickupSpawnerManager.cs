using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupSpawnerManager : MonoBehaviour
{
    // the singleton instance
    public static PickupSpawnerManager instance;

    // the prefab for ammo pickups
    [SerializeField] private GameObject _ammoPickupPrefab;
    // the prefab for health pickups
    [SerializeField] private GameObject _healthPickupPrefab;
    // the amount of pickups desired
    [SerializeField] private int _pickupAmount;
    // the amount of ammo vs health pickups
    [SerializeField, Range(0, 1)] private float _ammoToHealthRatio;

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

    void Start()
    {
        // Find all spawners in the scene and add them into the list
        _spawners = FindObjectsByType<PickupSpawnerController>(FindObjectsSortMode.None).ToList();

        // TODO: CALL IN GAME MANAGER
        SpawnPickups();
    }

    public void SpawnPickups()
    {
        // some error proofing
        if (_pickupAmount == 0) return;
        if (_pickupAmount > _spawners.Count) _pickupAmount = _spawners.Count;

        // set amounts for each pickup
        int ammoAmount = (int) Mathf.Round(_ammoToHealthRatio * _pickupAmount);
        int healthAmout = _pickupAmount - ammoAmount;

        // create a list of unused spawners
        List<PickupSpawnerController> availableSpawners = _spawners;
        // create ammo pickups
        for (int i = 0; i < ammoAmount; i++)
        {
            int index = Random.Range(0, availableSpawners.Count);
            availableSpawners[index].GetComponent<PickupSpawnerController>()?.CreatePickup(_ammoPickupPrefab);
            availableSpawners.RemoveAt(index);
        }
        // create health pickups
        for (int i = 0; i < healthAmout; i++)
        {
            int index = Random.Range(0, availableSpawners.Count);
            availableSpawners[index].GetComponent<PickupSpawnerController>()?.CreatePickup(_healthPickupPrefab);
            availableSpawners.RemoveAt(index);
        }
    }
}
