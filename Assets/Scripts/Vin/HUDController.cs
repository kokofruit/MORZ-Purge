using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WeaponTemplate;

// Main Contributor: Vin
// Secondary Contributor: 
// Reviewer: 
// Description: Controller to show health, ammo, and upgrades on the HUD

public class HUDController : MonoBehaviour
{
    // Pulic variables
    public float Health, MaxHealth, Width, Height;
    public Image healthBar;
    public TextMeshProUGUI ammoTxt;
    public TextMeshProUGUI[] inventoryAmmo = new TextMeshProUGUI[3];
    public int[] Ammo, MaxAmmo;
    public int ammo;
    public Inventory_Manager inventoryManager;
    public static HUDController instance;


    // Private variables
    [SerializeField]
    private int healthBarWidth = 100;
    private int healthBarHeight = 100;
    private RectTransform healthBarRect;

    private string[] ammoString = new string[3];
    private int[] ammoCaps = new int[3];

    private void Awake()
    {
       instance = this;
    }

    void Start()
    {

        // Set health bar stuffs
        healthBarRect = healthBar.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, healthBarHeight);
        SetMaxHealth(healthBarHeight);

        // Set ammo
        ammoString[0] = "Light  ";
        ammoString[1] = "Medium";
        ammoString[2] = "Heavy";
        ammoCaps[0] = 60;
        ammoCaps[1] = 260;
        ammoCaps[2] = 40;
        Ammo = Inventory_Manager.instance.GetAmmo();
        SetAmmo(Ammo);
    }

    //Setting max health
    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, maxHealth);
    }

    //Setting health
    public void SetHealth(float health)
    {
        Health = health;
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, Health);
    }

    // Set ammo inventory
    public void UpdateAmmo(WeaponTemplate.AmmoType ammoType)
    {
       inventoryAmmo[(int)ammoType].text = "" + ammoString[(int)ammoType] + "\t" + Inventory_Manager.instance.GetAmmo(ammoType).ToString() + "/" + ammoCaps[(int)ammoType];
    }

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
