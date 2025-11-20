// Main Contributor: Vin Lettich
// Secondary Contributor: 
// Reviewer: 
// Description: Controller for temporary Stimulation Upgrade

using System.Collections;
using UnityEngine;
using System;
using UnityEngine.WSA;

public class TempPickupController : PickupController
{

    // Private Variables //
    [SerializeField] private float upgradeDuration = 10f;
    // upgrade type
    [SerializeField] private int upgradeType;

    // Trigger event (initiated by Player_Controller)
    public override void PickupObject()
    {
        //SetUpgrade in HUD
        HUDController.instance.SetUpgrade(upgradeType, upgradeDuration);
        base.PickupObject();
    }
}
