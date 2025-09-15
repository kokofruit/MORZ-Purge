// Main Contributor: Phillip Cano
// Secondary Contributor: Gabriel heiser
// Reviewer: 
// Description: Object that represents an in game weapon the player can wield, constructed based on the weapon templates

public class Weapon
{
    // Constructor for the weapon class
    public Weapon(WeaponTemplate weapon)
    {
        MAG_SIZE = weapon.MAG_SIZE;
        RANGE = weapon.RANGE;
        FIRE_SELECT = weapon.FIRE_SELECT;
        FIRE_RATE = weapon.FIRE_RATE;
        COOLDOWN = weapon.COOLDOWN;
    }

    // The maximum number of bullets the weapon can hold in its magazine
    public int MAG_SIZE { get; private set; }
    // The number of actual bullets currently in the weapons magazine
    public int ammo { get; private set; }
    // The effective distance the weapon will be able to hit its target
    public int RANGE { get; private set; }
    // The type of fire behavior the weapon has
    public WeaponTemplate.FireSelect FIRE_SELECT { get; private set; }
    // The type of ammo the weapon accepts
    public WeaponTemplate.AmmoType AMMO_TYPE { get; private set; }
    // The number of bullets the weapon will fire in a second
    public float FIRE_RATE { get; private set; }
    // The length of the reload cooldown in seconds
    public float COOLDOWN { get; private set; }
    // Stores the cooling state of the weapon
    private bool _isCooling;

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

    // Method to get the weapon's current cooling status
    public bool GetCooldownStatus()
    {
        return _isCooling;
    }

    // Method to reload the weapon's magazine
    public void Reload()
    {
        if (Inventory_Manager.instance.GetAmmo(AMMO_TYPE) < MAG_SIZE)
        {
            ammo = Inventory_Manager.instance.GetAmmo(AMMO_TYPE);
        }
        else
        {
            ammo = MAG_SIZE;
        }
        Inventory_Manager.instance.SubtractAmmo(AMMO_TYPE, MAG_SIZE);
    }

    // Method to start the cooldown for this weapon
    public void BeginCooldown()
    {
        Weapon_Manager.instance.BeginCooldown(this);
    }
};