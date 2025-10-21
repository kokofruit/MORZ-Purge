// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Ammo Upgrade

using System.Collections;
using UnityEngine;
using System;
using UnityEngine.WSA;

public class AmmoUpgradeController : PickupController
{
    /* 
     * TODO:
     * fix stacking upgrades
     */

    // Private Variables //
    private float upgradeDuration = 10f;
    // Ammo upgrade is type 3
    private int upgradeType = 3;

    // Trigger event (initiated by Weapon_Action_Controller)
    public override void PickupObject()
    {
        // Invoke ammo upgrade that will stop ammo from decreasing for 10s
        Weapon_Action_Controller.instance.ActivateUpgrade(upgradeType);
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
