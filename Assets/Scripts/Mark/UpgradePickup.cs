using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Upgrade pickup

public class UpgradePickup : PickupController
{

    private void OnTriggerEnter(Collider collider)
    {
        // Setting so only the player can trigger the pickup
        if (collider.gameObject.CompareTag("Player"))
        {
      
            // Call upgrade functions here

            Destroy(this.gameObject);
        }
    }
}
