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

    // Private Variables //
    private float upgradeDuration = 10f;
    // Stimulant upgrade is type 1
    private int upgradeType = 1;

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
