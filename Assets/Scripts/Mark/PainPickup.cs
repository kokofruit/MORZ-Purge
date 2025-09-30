using UnityEngine;

// Main Contributor: Mark
// Secondary Contributor:
// Reviewer: 
// Description: To apply damage to the player through a pickup

public class PainPickup : PickupController
{
    // How much damage the player will take
    public float PainAmount = 10;

    public override void PickupObject()
    {
        // Calling the SubtactHealth function within the player_Controller and applying the damage amount.
        Player_Controller.instance.SubtractHealth(PainAmount);
        base.PickupObject();
    }
}
