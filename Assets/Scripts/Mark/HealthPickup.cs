using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Health pickup

public class HealthPickup : PickupController
{

    private float healthAmount = 20;

    // Setting trigger event for testing
    // Will implement interact "E" once completed
    private void OnTriggerEnter(Collider collider)
    {
        // Setting so only the player can trigger the pickup
        if (collider.gameObject.CompareTag("Player"))
        {
            Player_Controller.instance.AddHealth(healthAmount);

            Destroy(this.gameObject);
        }
    }
}
