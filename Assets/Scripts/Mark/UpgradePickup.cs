using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: Vin
// Description: child script for Upgrade pickup

public class UpgradePickup : PickupController
{
    // Drag needed upgrade into specific pickup
    public UpgradeTemplate upgrade;

    public override void PickupObject()
    {
        Inventory_Manager.instance.playerInventory.AddUpgrade(upgrade);

        base.PickupObject();
    }
}
