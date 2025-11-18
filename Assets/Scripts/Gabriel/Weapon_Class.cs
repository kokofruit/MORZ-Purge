// Main Contributor: Phillip Cano
// Secondary Contributor: Gabriel heiser
// Reviewer: 
// Description: Object that represents an in game weapon the player can wield, constructed based on the weapon templates

using UnityEngine;

[System.Serializable]
public class Weapon
{
    // Constructor for the weapon class
    public Weapon(WeaponTemplate weapon, float[] upgradeValues)
    {
        RANGE = weapon.RANGE;
        FIRE_SELECT = weapon.FIRE_SELECT;
        STAGE = weapon.STAGE;
        AMMO_TYPE = weapon.AMMO_TYPE;
        AOE_RADIUS = weapon.AOE_RADIUS;

        BASE_DAMAGE = weapon.DAMAGE;
        BASE_FIRE_RATE = weapon.FIRE_RATE;
        BASE_MAG_SIZE = weapon.MAG_SIZE;
        BASE_COOLDOWN = weapon.COOLDOWN;

        ammo = BASE_MAG_SIZE;
        AddUpgrades(upgradeValues);

        HUDController.instance.LoadMagazineDisplay(this);
    }

    public void AddUpgrades(float[] upgradeValues)
    {
        damage = BASE_DAMAGE + upgradeValues[0];
        fireRate = BASE_FIRE_RATE + upgradeValues[1];
        magSize = BASE_MAG_SIZE + (int)upgradeValues[2];
        cooldown = BASE_COOLDOWN - upgradeValues[3];
    }

    private int BASE_MAG_SIZE;
    // The maximum number of bullets the weapon can hold in its magazine
    public int magSize { get; private set; }
    // The number of actual bullets currently in the weapons magazine
    public int ammo { get; private set; }
    private float BASE_DAMAGE;
    public float damage { get; private set; }
    // The effective distance the weapon will be able to hit its target
    public int RANGE { get; private set; }
    // The type of fire behavior the weapon has
    public WeaponTemplate.FireSelect FIRE_SELECT { get; private set; }
    // The type of ammo the weapon accepts
    public int AOE_RADIUS;
    public WeaponTemplate.AmmoType AMMO_TYPE { get; private set; }
    // 
    public WeaponTemplate.Stage STAGE { get; private set; }
    private float BASE_FIRE_RATE;
    // The number of bullets the weapon will fire in a second
    public float fireRate { get; private set; }
    private float BASE_COOLDOWN;
    // The length of the reload cooldown in seconds
    public float cooldown { get; private set; }
    // Stores the cooling state of the weapon
    private bool _isCooling;
    // Stores the time that the weapons last cooldown began
    private float _coolStartTime;


    // Method to remove 1 bullet from the current magazine
    public void SubtractAmmo()
    {
        ammo -= 1;
    }

    // Method to set the weapon's cooling status
    public void SetCoolingStatus(bool status)
    {
        _isCooling = status;
    }

    public void SetCoolingStatus(bool status, float coolStartTime)
    {
        SetCoolingStatus(status);
        _coolStartTime = coolStartTime;
    }

    // Method to get the weapon's current cooling status
    public bool GetCooldownStatus()
    {
        return _isCooling;
    }

    public float GetCooldownStartTime()
    {
        return _coolStartTime;
    }

    // Method to reload the weapon's magazine
    public void Reload()
    {
        ammo = InventoryManager.instance.playerInventory.SubtractAmmo(AMMO_TYPE, magSize);
        HUDController.instance.UpdateAmmo(AMMO_TYPE);
    }
};