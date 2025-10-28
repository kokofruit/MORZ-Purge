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

    // Private Variables //
    private float upgradeDuration = 10f;
    // Ammo upgrade is type 3
    private int upgradeType = 3;

    // Trigger event (initiated by Weapon_Action_Controller)
    public override void PickupObject()
    {
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
