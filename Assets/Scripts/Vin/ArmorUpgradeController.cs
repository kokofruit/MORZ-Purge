// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Invulnerability Armor Upgrade

using System.Collections;
using UnityEngine;
using System;

public class ArmorUpgradeController : PickupController
{

    // Private Variables //
    private float upgradeDuration = 10f;
    // Armor upgrade is type 0
    private int upgradeType = 0;

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
