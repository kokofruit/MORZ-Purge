using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Health pickup

public class HealthPickup : PickupController
{

    private float healthAmount = 20;

    // Setting trigger event for testing
    public override void PickupObject()
    {
        Player_Controller.instance.AddHealth(healthAmount);

        base.PickupObject();
    }
}
