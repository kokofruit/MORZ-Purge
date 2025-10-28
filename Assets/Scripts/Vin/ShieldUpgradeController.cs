// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Invisibility Shield upgrade

using System.Collections;
using UnityEngine;
using System;

public class ShieldUpgradeController : PickupController
{

    // Private Variables //
    private float upgradeDuration = 10f;
    // Shield upgrade is type 2
    private int upgradeType = 2;

    // Trigger event (initiated by EnemyController)
    public override void PickupObject()
    {
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}

