// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Stimulation Upgrade

using System.Collections;
using UnityEngine;
using System;
using UnityEngine.WSA;

public class StimulantUpgradeController : PickupController
{
    /* 
     * TODO:
     * change UI to visually show stim activation
     * fix stacking upgrades
     */

    // Private Variables //
    private float upgradeDuration = 10f;
    // Stimulant upgrade is type 1
    private int upgradeType = 1;

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        // Invoke stimulant upgrade that will increase player _speed
        Player_Controller.instance.ActivateUpgrade(upgradeType);
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
