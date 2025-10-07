// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Invulnerability Armor Upgrade

using System.Collections;
using UnityEngine;

public class ArmorUpgradeController : PickupController
{
    /* 
     * TODO:
     * Use a coroutine for countdown
     * Set a flag to watch for actived to turn true or false
     */
     
    // Private Variables //
    private float upgradeDuration = 10f;
    // Get UpgradeInfo_txt to adjust upgrades
    // I could see a potential bug here if you get more than one upgrade at once

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        // Set activated bool to true
        activated = true;
        // Start the ActivateUpgrade countdown and effects
        StartCoroutine(ActivateUpgrade());

        base.PickupObject();
    }

    IEnumerator ActivateUpgrade()
    {
        // Wait for allotted upgrade time
        yield return new WaitForSeconds(upgradeDuration);
        /* 
         * TODO:
         * update the timer
         * change UI to visually show armor activation
         * make it so health cannot be updated during this time
         */
    }
}
