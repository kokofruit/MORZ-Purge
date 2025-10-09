// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Invulnerability Armor Upgrade

using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class ArmorUpgradeController : PickupController
{
    /* 
     * TODO:
     * change UI to visually show armor activation
     * make it so health cannot be updated during this time
     */

    // Private Variables //
    private float upgradeDuration = 10f;
    private int upgradeType = 0;
    // I could see a potential bug here if you get more than one upgrade at once

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        // Invoke armor upgrade event that will stop player damage from happening
        Player_Controller.instance.ActivateUpgrade(upgradeType);
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
