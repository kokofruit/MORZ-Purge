// Main Contributor: Gabriel Heiser
// Secondary Contributor:
// Reviewer: Gabriel Heiser
// Description: Manages the temporary upgrades

using UnityEngine;
using System.Collections;

public class TempPickupManager : MonoBehaviour
{
    public static TempPickupManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartTimer(int upgradeType, float upgradeDuration)
    {
        StartCoroutine(Timer(upgradeType, upgradeDuration));
    }
    
    // This is a timer used to count down upgradeDuration on the HUD
    public IEnumerator Timer(int upgradeType, float upgradeDuration)
    {
        // Update timer while upgradeDuration hasn't run out
        while (upgradeDuration > 0)
        {
            // Set upgradeTimerTxt
            // upgradeTimerTxt.text = "" + upgradeDuration.ToString() + "s";

            // Increment the countdown by one second
            yield return new WaitForSeconds(1f);

            // Decrement countdown time
            upgradeDuration -= 1f;
        }

        // Set timer text to 0s
        // upgradeTimerTxt.text = "0s";
        // Reset upgrade description
        // upgradeInfoTxt.text = "none";

        // Deactivate upgrades
        HUDController.instance.DeactivateUpgrades(upgradeType);
    }
}
