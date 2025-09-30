// Main Contributor: Vin
// Secondary Contributor: 
// Reviewer: Gabriel Heiser
// Description: Controller to show health, ammo, and upgrades on the HUD

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //// Pulic variables ////
    // References health bar image on HUD
    public Image healthBar;
    // References the text box displayed for ammo currently in mag
    public TextMeshProUGUI ammoTxt;
    // References the text boxes for light, medium, and heavy ammo
    public TextMeshProUGUI[] inventoryAmmo = new TextMeshProUGUI[3];
    // Instance for HUDController
    public static HUDController instance;


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
    public void SetUpgrade()
    {
        // Manipulate upgrade text
    }
}
