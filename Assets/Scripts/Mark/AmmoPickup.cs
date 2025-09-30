using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Ammo pickup

public class AmmoPickup : PickupController
{
    public int AmmoAmount = 20;
    public WeaponTemplate.AmmoType ammoType;

    // Setting trigger event for testing
    public override void PickupObject()
    {
        Inventory_Manager.instance.playerInventory.AddAmmo(ammoType, AmmoAmount);
        // Update ammo HUD
        HUDController.instance.UpdateAmmo(ammoType);

        base.PickupObject();
    }
}
