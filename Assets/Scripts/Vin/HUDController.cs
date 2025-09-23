using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// Main Contributor: Vin
// Secondary Contributor: 
// Reviewer: 
// Description: Controller to show health, ammo, and upgrades on the HUD

public class HUDController : MonoBehaviour
{
    //Variables
    public float Health, MaxHealth, Width, Height;
    public Image healthBar;
    public TextMeshProUGUI ammoTxt;
    public TextMeshProUGUI lightAmmoTxt;
    public TextMeshProUGUI medAmmoTxt;
    public TextMeshProUGUI hevAmmoTxt;
    public int[] Ammo, MaxAmmo;
    public int ammo;
    public Inventory_Manager inventoryManager;


    //Health bar
    [SerializeField]
    private int healthBarWidth = 100;
    private int healthBarHeight = 100;
    private RectTransform healthBarRect;

    void Start()
    {

        // Set health bar stuffs
        healthBarRect = healthBar.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(healthBarWidth, healthBarHeight);
        SetMaxHealth(healthBarHeight);

        // Set ammo stuffs
        SetLightAmmo();
        SetMedAmmo();
        SetHevAmmo();
        SetMagAmmo();

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

    // Setting light ammo
    public void SetLightAmmo()
    {
        // Get ammo amount from inventory
        Ammo = inventoryManager.GetAmmo();
        // Manipulate ammo text
        lightAmmoTxt.text = "Light " + Ammo[0].ToString() + "/100";
    }

    // Setting medium ammo
    public void SetMedAmmo()
    {
        // Get ammo amount from inventory
        Ammo = inventoryManager.GetAmmo();
        // Manipulate ammo text
        medAmmoTxt.text = "Medium " + Ammo[1].ToString() + "/100";
    }

    // Setting heavy ammo
    public void SetHevAmmo()
    {
        // Get ammo amount from inventory
        Ammo = inventoryManager.GetAmmo();
        // Manipulate ammo text
        hevAmmoTxt.text = "Heavy " + Ammo[2].ToString() + "/100";
    }

    // Setting amount in mag
    public void SetMagAmmo()
    {
        // Use Weapon Class to set ammo amount
    }

    // Setting upgrade
    public void SetUpgrade()
    {
        // Manipulate upgrade text
    }
}
