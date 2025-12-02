using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor: Kris
// Reviewer: Vin
// Description: child script for Upgrade pickup

public class UpgradePickup : PickupController
{
    // Drag needed upgrade into specific pickup
    public UpgradeTemplate upgrade;
   
    public override void PickupObject()
    {
        InventoryManager.instance.playerInventory.AddUpgrade(upgrade);
        
        // Finds the index of the upgrade picked up and then outputs that upgrades info text.
        int index = DialogueManager.instance.upgradeChoices.IndexOf(upgrade);
        
        if (index >= 0)
            DialogueManager.instance.UpgradeDisplay(index);

        base.PickupObject();
    }
}
