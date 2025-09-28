using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Ammo pickup

public class AmmoPickup : PickupController
{

    private int AmmoAmount = 20;

    // Setting trigger event for testing
    // Will implement interact "E" once completed
    private void OnTriggerEnter(Collider collider)
    {
        // Setting so only the player can trigger the pickup
        if (collider.gameObject.CompareTag("Player"))
        {
            // Add ammo to inventory
            Inventory_Manager.instance.AddAmmo(0, AmmoAmount);
            // Update ammo HUD
            HUDController.instance.UpdateAmmo(0);

            Destroy(this.gameObject);
        }
    }
}
