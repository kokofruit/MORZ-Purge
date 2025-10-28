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
    public override void PickupObject()
    {
        // Calling 
        InventoryManager.instance.playerInventory.AddWeapon(weapon);

        base.PickupObject();
    }
}
