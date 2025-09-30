// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Manages the player's inventory during runtime.

using UnityEngine;
using UnityEngine.UI;

public class Inventory_Manager : MonoBehaviour
{
    // Static instance for other scripts to reference
    public static Inventory_Manager instance;
    // Definitions for the maximum capacity for each ammo type in the player's inventory
    public int LIGHT_AMMO_CAP;
    public int MEDIUM_AMMO_CAP;
    public int HEAVY_AMMO_CAP;
    // The gun that the player will start the game with
    public WeaponTemplate starterGun;
    // Class to hold the upgrade values
    public class upVal { public float[] upgradeValues = { 0, 0, 0, 0 }; };
    // Variable to store the instantiated player inventory
    public Inventory playerInventory;

    // stuff to make better mayhaps
    public Image gunImage;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

    }

    public void StartNewInventory()
    {
        playerInventory = new Inventory();

        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light] = LIGHT_AMMO_CAP;
        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium] = MEDIUM_AMMO_CAP;
        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy] = HEAVY_AMMO_CAP;

        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Light] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light];
        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Medium] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium];
        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Heavy] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy];

        playerInventory.AddWeapon(starterGun);

        HUDController.instance.SetAmmo(playerInventory.ammo);
    }

    public void SetInventory(Inventory inventory)
    {
        playerInventory = inventory;

        HUDController.instance.SetAmmo(playerInventory.ammo);
        Weapon_Action_Controller.instance.currentWeapon = playerInventory.GetWeapon(0);
    }

    public Inventory GetInventory()
    {
        return playerInventory;
    }

    public void ChangeWeapon(int inc, int start, ref Weapon w)
    {
        int val = start;
        val += inc;
        if (val > 2)
            val = 0;
        else if (val < 0)
            val = 2;
        if (playerInventory.GetWeapon(val) == null)
            ChangeWeapon(inc, val, ref w);
        else
        {
            w = playerInventory.GetWeapon(val);
            gunImage.sprite = w.SPRITE;
            HUDController.instance.SetMagAmmo(w.ammo);
        }
    }
}

public class Inventory
{
    public int[] ammo = new int[3];
    public int[] AMMO_CAPS = new int[3];
    private Weapon[] Weapons = new Weapon[3];
    private Inventory_Manager.upVal[] upgrades = new Inventory_Manager.upVal[9];

    public Inventory()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = new Inventory_Manager.upVal();
        }
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
        ammo[(int)type] += amount;
        ammo[(int)type] = Mathf.Clamp(ammo[(int)type], 0, AMMO_CAPS[(int)type]);
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
        Inventory_Manager.instance.gunImage.sprite = weapon.SPRITE;
        Weapon_Action_Controller.instance.currentWeapon = GetWeapon((int)weapon.AMMO_TYPE);
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

        if (upgrade.STAGE == WeaponTemplate.Stage.all)
        {
            for (int i = (int)upgrade.AMMO_TYPE * 3; i < upgradeIndex; i++)
            {
                upgrades[i].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
            }
        }
        else
        {
            upgrades[upgradeIndex].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
        }
        
        Weapons[(int)upgrade.AMMO_TYPE]?.AddUpgrades(upgrades[(int)upgrade.AMMO_TYPE * 3 + (int)Weapons[(int)upgrade.AMMO_TYPE].STAGE].upgradeValues);
    }
}
