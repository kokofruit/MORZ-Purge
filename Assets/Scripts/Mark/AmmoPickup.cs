using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Ammo pickup

public class AmmoPickup : PickupController
{
    private int AmmoAmount = 20;

    // Setting trigger event for testing
    public override void PickupObject()
    {
        Inventory_Manager.instance.playerInventory.AddAmmo(0, AmmoAmount);
        // Update ammo HUD
        HUDController.instance.UpdateAmmo(0);

        base.PickupObject();
    }
}
