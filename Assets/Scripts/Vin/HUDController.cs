// Main Contributor: Vin
// Secondary Contributor: 
// Reviewer: Gabriel Heiser
// Description: Controller to show health, ammo, and upgrades on the HUD

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeTemplate;

public class HUDController : MonoBehaviour
{
    //// Pulic variables ////
    // References health bar image on HUD
    public Image healthBar;
    // References the text box displayed for ammo currently in mag
    public TextMeshProUGUI ammoTxt;
    // References the text boxes for light, medium, and heavy ammo
    public TextMeshProUGUI[] inventoryAmmo = new TextMeshProUGUI[3];
    // Reference to upgrade info text box
    public TextMeshProUGUI upgradeInfoTxt;
    // Reference to upgrade timer text box
    public TextMeshProUGUI upgradeTimerTxt;
    // Instance for HUDController
    public static HUDController instance;
    // Upgrade tint references
    public Image shieldUpgradeImg;
    public Image armorUpgradeImg;
    public Image stimUpgradeImg;
    public Image ammoUpgradeImg;


    //// Private variables ////
    // Health bar dimensions
    private int healthBarWidth = 100;
    private int healthBarHeight = 100;
    // Health bar RectTransform to manipulate
    private RectTransform healthBarRect;
    // Types of ammo stored in list
    private string[] ammoString = new string[3];
    // Ammo caps stored in list
    private int[] ammoCaps = new int[3];
    // Upgrade text stored in list
    private string[] upgradesString = new string[4];

    // Used to make an instance
    private void Awake()
    {
       instance = this;
    }

    void Start()
    {
        // Get health bar RectTransform
        healthBarRect = healthBar.GetComponent<RectTransform>();
        // Set health bar width and height
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, healthBarHeight);
        // Set max player health on start
        SetMaxHealth(healthBarHeight);

        // Populate ammoString with types of ammo
        ammoString[0] = "Light  ";
        ammoString[1] = "Medium";
        ammoString[2] = "Heavy";
        // Populate ammoCaps with amounts of max ammo
        ammoCaps[0] = 60;
        ammoCaps[1] = 260;
        ammoCaps[2] = 40;
        // Populate upgradesString with upgrade types
        upgradesString[0] = "Invulnerability Armor";
        upgradesString[1] = "Stimulant";
        upgradesString[2] = "Invisibility Shield";
        upgradesString[3] = "Ammo Pack";

        // Make sure upgrade tints are disabled
        shieldUpgradeImg.enabled = false;
        armorUpgradeImg.enabled = false;
        stimUpgradeImg.enabled = false;
        ammoUpgradeImg.enabled = false;

    }

    // Setting max health
    public void SetMaxHealth(float maxHealth)
    {
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, maxHealth);
    }

    // Setting health (used when health is added or subtracted)
    public void SetHealth(float health)
    {
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, health);
    }

    // Update ammo inventory after shooting/reloading
    public void UpdateAmmo(WeaponTemplate.AmmoType ammoType)
    {
        inventoryAmmo[(int)ammoType].text = "" + ammoString[(int)ammoType] + "\t" + Inventory_Manager.instance.playerInventory.GetAmmo(ammoType).ToString() + "/" + ammoCaps[(int)ammoType];
    }

    // Set ammo inventory to start with strings
    public void SetAmmo(int[] Ammo)
    {
        for (int i = 0; i < 3; i++)
        {
            inventoryAmmo[i].text = "" + ammoString[i] + "\t" + Ammo[i].ToString() + "/" + ammoCaps[i];
        }
    }

    // Setting amount in mag
    public void SetMagAmmo(float ammo)
    {
        ammoTxt.text = "" + ammo;
    }

    // Setting upgrade
    public void SetUpgrade(int upgradeType, float upgradeDuration)
    {
        // Set text to display what upgrade player obtained
        upgradeInfoTxt.text = "" + upgradesString[upgradeType];

        // Stop any running coroutines (Timer)
        StopAllCoroutines();

        // Deactivate any running upgrades before starting another upgrade
        DeactivateUpgrades(upgradeType);

        // Start timer coroutine
        StartCoroutine(Timer(upgradeType, upgradeDuration));

        // Activate upgrades and HUD tints
        if (upgradeType == 0 || upgradeType == 1)
        {
            if(upgradeType == 0) armorUpgradeImg.enabled = true;
            else stimUpgradeImg.enabled = true;
            // Activate upgrade
            Player_Controller.instance.ActivateUpgrade(upgradeType);
        }
        else if (upgradeType == 2)
        {
            // Activate upgrade
            EnemyController.instance.ActivateUpgrade(upgradeType);
            shieldUpgradeImg.enabled = true;
        }
        else if (upgradeType == 3)
        {
            // Activate upgrade
            Weapon_Action_Controller.instance.ActivateUpgrade(upgradeType);
            ammoUpgradeImg.enabled = true;
        }

    }

    // This is a timer used to count down upgradeDuration on the HUD
    IEnumerator Timer(int upgradeType, float upgradeDuration)
    {
        // Update timer while upgradeDuration hasn't run out
        while (upgradeDuration > 0)
        {
            // Set upgradeTimerTxt
            upgradeTimerTxt.text = "" + upgradeDuration.ToString() + "s";

            // Increment the countdown by one second
            yield return new WaitForSeconds(1f);

            // Decrement countdown time
            upgradeDuration -= 1f;
        }

        // Set timer text to 0s
        upgradeTimerTxt.text = "0s";
        // Reset upgrade description
        upgradeInfoTxt.text = "none";

        // Deactivate upgrades
        DeactivateUpgrades(upgradeType);

    }

    // This deactivates any running upgrades, and disables the tint image
    private void DeactivateUpgrades(int upgradeType)
    {
        // Deactivate armor and stim upgrade after timer runs out
        Player_Controller.instance.DeactivateUpgrade(0);
        Player_Controller.instance.DeactivateUpgrade(1);
        // Deactivate screen tints
        stimUpgradeImg.enabled = false;
        armorUpgradeImg.enabled = false;

        // Deactivate shield upgrade after timer runs out
        EnemyController.instance.DeactivateUpgrade(2);
        // Deactivate screen tint
        shieldUpgradeImg.enabled = false;

        // Deactivate ammo upgrade after timer runs out
        Weapon_Action_Controller.instance.DeactivateUpgrade(3);
        // Deactivate screen tint
        ammoUpgradeImg.enabled = false;
    }
}
