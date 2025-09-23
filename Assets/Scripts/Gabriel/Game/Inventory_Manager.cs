// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Manages the player's inventory during runtime.

using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    public static Inventory_Manager instance;
    public int LIGHT_AMMO_CAP;
    public int MEDIUM_AMMO_CAP;
    public int HEAVY_AMMO_CAP;
    public WeaponTemplate starterGun;
    public class upVal { public float[] upgradeValues = { 0, 0, 0, 0 }; };

    private int[] ammo = new int[3];
    private int[] AMMO_CAPS = new int[3];
    private Weapon[] Weapons = new Weapon[3];
    private upVal[] upgrades = new upVal[9];

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light] = LIGHT_AMMO_CAP;
        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium] = MEDIUM_AMMO_CAP;
        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy] = HEAVY_AMMO_CAP;

        ammo[(int)WeaponTemplate.AmmoType.Light] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light];
        ammo[(int)WeaponTemplate.AmmoType.Medium] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium];
        ammo[(int)WeaponTemplate.AmmoType.Heavy] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = new upVal();
        }

        AddWeapon(starterGun);
        Weapon_Action_Controller.instance.currentWeapon = Weapons[0];
    }

    public Weapon[] GetLoadout()
    {
        return Weapons;
    }

    public int[] GetAmmo()
    {
        return ammo;
    }

    public int GetAmmo(WeaponTemplate.AmmoType type)
    {
        return ammo[(int)type];
    }

    public void AddAmmo(WeaponTemplate.AmmoType type, int amount)
    {
        if (ammo[(int)type] > AMMO_CAPS[(int)type])
            ammo[(int)type] = AMMO_CAPS[(int)type];
        else
            ammo[(int)type] += amount;
    }

    public int SubtractAmmo(WeaponTemplate.AmmoType type, int amount)
    {
        if (ammo[(int)type] < amount)
        {
            ammo[(int)type] = 0;
            return ammo[(int)type];
        }
        else
        {
            ammo[(int)type] -= amount;
            return amount;
        }
    }

    public void AddWeapon(WeaponTemplate weapon)
    {
        Weapons[(int)weapon.AMMO_TYPE] = new Weapon(weapon, GetUpgrades(weapon));
    }

    public float[] GetUpgrades(WeaponTemplate weapon)
    {
        return upgrades[3 * (int)weapon.AMMO_TYPE + (int)weapon.STAGE].upgradeValues;
    }

    public Weapon GetWeapon(int index)
    {
        return Weapons[index];
    }

    public void AddUpgrade(UpgradeTemplate upgrade)
    {
        int upgradeIndex = (int)upgrade.AMMO_TYPE * 3 + (int)upgrade.STAGE;
        
        if (upgrade.STAGE == WeaponTemplate.Stage.all) {
            for (int i = (int)upgrade.AMMO_TYPE * 3; i < upgradeIndex; i++)
            {
                upgrades[i].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
            }
        }
        else {
            upgrades[upgradeIndex].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
        }

        foreach (Weapon weapon in Weapons)
        {
            if (weapon.AMMO_TYPE == upgrade.AMMO_TYPE)
            {
                weapon.AddUpgrades(upgrades[upgradeIndex].upgradeValues);
            }
        }
    }
}
