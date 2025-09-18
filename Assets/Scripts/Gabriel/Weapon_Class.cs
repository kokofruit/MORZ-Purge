// Main Contributor: Phillip Cano
// Secondary Contributor: Gabriel heiser
// Reviewer: 
// Description: Object that represents an in game weapon the player can wield, constructed based on the weapon templates

public class Weapon
{
    // Constructor for the weapon class
    public Weapon(WeaponTemplate weapon, float[] upgradeValues)
    {
        ChangeWeapon(weapon, upgradeValues);
    }

    public void ChangeWeapon(WeaponTemplate weapon, float[] upgradeValues)
    {
        damage = weapon.DAMAGE + upgradeValues[0];
        magSize = weapon.MAG_SIZE + (int)upgradeValues[2];
        RANGE = weapon.RANGE;
        FIRE_SELECT = weapon.FIRE_SELECT;
        fireRate = weapon.FIRE_RATE + upgradeValues[1];
        cooldown = weapon.COOLDOWN - upgradeValues[3];
        ammo = magSize;
    }

    // The maximum number of bullets the weapon can hold in its magazine
    public int magSize { get; private set; }
    // The number of actual bullets currently in the weapons magazine
    public int ammo { get; private set; }
    public float damage { get; private set; }
    // The effective distance the weapon will be able to hit its target
    public int RANGE { get; private set; }
    // The type of fire behavior the weapon has
    public WeaponTemplate.FireSelect FIRE_SELECT { get; private set; }
    // The type of ammo the weapon accepts
    public WeaponTemplate.AmmoType AMMO_TYPE { get; private set; }
    // The number of bullets the weapon will fire in a second
    public float fireRate { get; private set; }
    // The length of the reload cooldown in seconds
    public float cooldown { get; private set; }
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
        ammo = Inventory_Manager.instance.SubtractAmmo(AMMO_TYPE, magSize);
    }

    // Method to start the cooldown for this weapon
    public void BeginCooldown()
    {
        Weapon_Manager.instance.BeginCooldown(this);
    }
};