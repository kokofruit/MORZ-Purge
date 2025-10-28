// Main Contributor: Vin
// Secondary Contributor: Gabriel Heiser
// Reviewer: Gabriel Heiser
// Description: Controller to show health, ammo, and upgrades on the HUD

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //// Pulic variables ////
    public static HUDController instance;
    // References health bar image on HUD
    public Image healthBar;
    public Image cooldownBar;
    // References the text box displayed for ammo currently in mag
    // public TextMeshProUGUI ammoTxt;
    // References the text boxes for light, medium, and heavy ammo
    public TextMeshProUGUI[] inventoryAmmo = new TextMeshProUGUI[3];
    // Reference to upgrade info text box
    // public TextMeshProUGUI upgradeInfoTxt;
    // Reference to upgrade timer text box
    // public TextMeshProUGUI upgradeTimerTxt;
    // Upgrade tint references
    public Image shieldUpgradeImg;
    public Image armorUpgradeImg;
    public Image stimUpgradeImg;
    public Image ammoUpgradeImg;

    //Overarching UI objects containing changing sprites
    //total ammo
    public GameObject loadoutAmmoContainer;
    //mag ammo
    public GameObject magazineAmmoContainer;
    //current weapon
    public GameObject weaponSpriteContainer;
    //held weapons icons
    public GameObject weaponIconContainer;
    //Backgrounds behind held weapon
    public GameObject IconBGContainer;
    //upgrade dots next to held weapon icons
    public GameObject upgradeDotContainer;

    //// Private variables ////
    // Types of ammo stored in list
    private string[] ammoString = new string[3];
    // Ammo caps stored in list
    private int[] ammoCaps = new int[3];
    // Upgrade text stored in list
    private string[] upgradesString = new string[4];

    //comment later goober
    public Image[] upgradeSlots = new Image[9];


    // Used to make an instance
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Populate ammoString with types of ammo
        ammoString[0] = "Light  ";
        ammoString[1] = "Medium";
        ammoString[2] = "Heavy";
        // Populate ammoCaps with amounts of max ammo
        ammoCaps[0] = 60;
        ammoCaps[1] = 260;
        ammoCaps[2] = 40;

        SetUpgrades();
        ammoCaps = InventoryManager.instance.playerInventory.AMMO_CAPS;
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
    public void SetMaxHealth()
    {
        healthBar.fillAmount = 1;
    }

    // Setting health (used when health is added or subtracted)
    public void DisplayHealth(float health)
    {
        healthBar.fillAmount = health / 100;
    }

    // Update ammo inventory after shooting/reloading
    public void UpdateAmmo(WeaponTemplate.AmmoType ammoType)
    {
        loadoutAmmoContainer.transform.GetChild((int)ammoType).GetComponent<TMP_Text>().text = InventoryManager.instance.playerInventory.GetAmmo(ammoType).ToString();
    }

    // Set ammo inventory to start with strings
    public void DisplayInventoryAmmo(int[] Ammo)
    {
        for (int i = 0; i < 3; i++)
        {
            loadoutAmmoContainer.transform.GetChild(i).GetComponent<TMP_Text>().text = Ammo[i].ToString();
        }
    }
    //Current weaopn image
    public void SetWeaponImage(int idx)
    {
        //loop through weapon images
        for (int i = 0; i < weaponSpriteContainer.transform.childCount; i++)
        {
            if (i == idx)
                //turn on current weaopn
                weaponSpriteContainer.transform.GetChild(i).gameObject.SetActive(true);
            else
                //turn off others
                weaponSpriteContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //set icon background
    public void SetIconBG(int idx)
    {
        //loop through background objects for gun icons
        for (int i = 0; i < IconBGContainer.transform.childCount; i++)
        {
            if (i == idx)
                //set current weapon icon to be brighter
                IconBGContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255);
            else
                //others to be darker
                IconBGContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, .2f);
        }
    }

    public void SetWeaponIcon(int ammoType, int stage)
    {
        //loop through weapon icon game objects for a stage
        for (int i = 0; i < weaponIconContainer.transform.GetChild(ammoType).childCount; i++)
        {
            //make held weapon visible
            if (i == stage) weaponIconContainer.transform.GetChild(ammoType).GetChild(i).gameObject.SetActive(true);
            //nonheld weapons invisible
            else weaponIconContainer.transform.GetChild(ammoType).GetChild(i).gameObject.SetActive(false);
        }
    }

    // Setting amount in mag
    public void LoadMagazineDisplay(Weapon weapon)
    {
        //stop current relaod cooldown bar
        StopAllCoroutines();
        if (weapon.GetCooldownStatus())
        {
            //bring up new cooldown bar if new gun is reloading
            cooldownBar.enabled = true;
            StartCoroutine(DisplayCooldown(weapon));
        }
        else cooldownBar.enabled = false;

        int ammo = weapon.ammo;
        int maxAmmo = weapon.magSize;

        //set up the ammo UI
        for (int i = 0; i < maxAmmo; i++)
        {
            magazineAmmoContainer.transform.GetChild(i).gameObject.SetActive(true);
            if (i < ammo)
            {
                //visible ammo icon
                magazineAmmoContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 1);
            }
            else
            {
                //invisible ammo icon
                magazineAmmoContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
        }
        for (int i = maxAmmo; i < magazineAmmoContainer.transform.childCount; i++)
        {
            //turn off ammo icons not in mag
            magazineAmmoContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    //reload cooldown bar
    public IEnumerator DisplayCooldown(Weapon weapon)
    {
        while (weapon.GetCooldownStatus())
        {
            cooldownBar.fillAmount = (Time.time - weapon.GetCooldownStartTime()) / weapon.cooldown;
            yield return null;
        }
        cooldownBar.enabled = false;
    }

    // Setting upgrade
    public void SetUpgrades()
    {
        float[] slots = new float[3];
        InventoryManager.instance.playerInventory.GetUpgradeSlots(ref slots);

        //three bit identifiers
        for (int i =0; i<3; i++) {
            //check slot 3
            if (slots[i] >= 4) {
                slots[i]-= 4;
                upgradeSlots[i*3+2].enabled = true;
            }else
                upgradeSlots[i * 3 + 2].enabled = false;
            //check slot 2
            if (slots[i] >= 2)
            {
                slots[i] -= 2;
                upgradeSlots[i * 3 + 1].enabled = true;
            }
            else
                upgradeSlots[i * 3 + 1].enabled = false;
            //check slot 1
            if (slots[i] == 1)
            {
                slots[i]--;
                upgradeSlots[i * 3 + 0].enabled = true;
            }
            else
                upgradeSlots[i * 3 + 0].enabled = false;
        }
    }
    
    public void SetUpgrade(int upgradeType, float upgradeDuration)
    {
        // Set text to display what upgrade player obtained
        // upgradeInfoTxt.text = "" + upgradesString[upgradeType];

        // Deactivate any running upgrades before starting another upgrade
        DeactivateUpgrades(upgradeType);

        // Start timer
        TempPickupManager.instance.StartTimer(upgradeType, upgradeDuration);

        // Activate upgrades and HUD tints
        if (upgradeType == 0 || upgradeType == 1)
        {
            if(upgradeType == 0) armorUpgradeImg.enabled = true;
            else stimUpgradeImg.enabled = true;
            // Activate upgrade
            PlayerController.instance.ActivateUpgrade(upgradeType);
        }
        else if (upgradeType == 2)
        {
            // Activate upgrade
            EnemyController.ActivateUpgrade(upgradeType);
            shieldUpgradeImg.enabled = true;
        }
        else if (upgradeType == 3)
        {
            // Activate upgrade
            WeaponActionController.instance.ActivateUpgrade(upgradeType);
            ammoUpgradeImg.enabled = true;
        }

    }

    // This deactivates any running upgrades, and disables the tint image
    public void DeactivateUpgrades(int upgradeType)
    {
        // Deactivate armor and stim upgrade after timer runs out
        PlayerController.instance.DeactivateUpgrade(0);
        PlayerController.instance.DeactivateUpgrade(1);
        // Deactivate screen tints
        stimUpgradeImg.enabled = false;
        armorUpgradeImg.enabled = false;

        // Deactivate shield upgrade after timer runs out
        EnemyController.DeactivateUpgrade(2);
        // Deactivate screen tint
        shieldUpgradeImg.enabled = false;

        // Deactivate ammo upgrade after timer runs out
        WeaponActionController.instance.DeactivateUpgrade(3);
        // Deactivate screen tint
        ammoUpgradeImg.enabled = false;
    }
}
