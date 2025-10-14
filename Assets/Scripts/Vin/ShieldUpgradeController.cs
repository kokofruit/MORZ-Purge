// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Invisibility Shield upgrade

using System.Collections;
using UnityEngine;
using System;

public class ShieldUpgradeController : PickupController
{
    /* 
     * TODO:
     * only slightly works, bug goes to last known player location and just waits
     * change UI to visually show shield activation
     * fix stacking upgrades
     */

    // Private Variables //
    private float upgradeDuration = 10f;
    // Stimulant upgrade is type 2
    private int upgradeType = 2;

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        // Invoke shield upgrade event that will stop enemy from chasing player
        EnemyController.instance.ActivateUpgrade(upgradeType);
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}

