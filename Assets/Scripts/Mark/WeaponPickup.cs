using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Weapon pickup

public class WeaponPickup : PickupController
{
    // Drag needed weapon into specific pickup
    public WeaponTemplate weapon;

    // Setting trigger event for testing
    // Will implement interact "E" once completed
    private void OnTriggerEnter(Collider collider)
    {
        // Setting so only the player can trigger the pickup
        if (collider.gameObject.CompareTag("Player"))
        {
            // Calling 
            Inventory_Manager.instance.AddWeapon(weapon);
            // Add an if statment to swap weapons if player already has 3

            Debug.Log("pickup " + weapon.name);
            Destroy(this.gameObject);
        }
    }
}
