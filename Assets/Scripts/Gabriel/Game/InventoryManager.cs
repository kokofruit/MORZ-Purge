// Main Contributor: Gabriel Heiser
// Secondary Contributor: Philll
// Reviewer: 
// Description: Manages the player's inventory during runtime.

using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Static instance for other scripts to reference
    public static InventoryManager instance;
    // Definitions for the maximum capacity for each ammo type in the player's inventory
    public int LIGHT_AMMO_CAP;
    public int MEDIUM_AMMO_CAP;
    public int HEAVY_AMMO_CAP;
    // The gun that the player will start the game with
    public WeaponTemplate starterGun;
    // Class to hold the upgrade values: [damage, fire rate, mag size, cooldown]
    [System.Serializable]
    public class upVal { public float[] upgradeValues = { 0, 0, 0, 0, 0 }; };
    // Variable to store the instantiated player inventory
    public Inventory playerInventory;

    void Awake()
    {
        instance = this;
    }

    public void StartNewInventory()
    {
        playerInventory = new Inventory();

        //Set maximum ammo for the three ammo types
        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light] = LIGHT_AMMO_CAP;
        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium] = MEDIUM_AMMO_CAP;
        playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy] = HEAVY_AMMO_CAP;
        //Set ammo ammount to max ammo
        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Light] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light];
        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Medium] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium];
        playerInventory.ammo[(int)WeaponTemplate.AmmoType.Heavy] = playerInventory.AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy];
        //adds a starter gun
        playerInventory.AddWeapon(starterGun);
        
        HUDController.instance.DisplayInventoryAmmo(playerInventory.ammo);
    }

    public void SetInventory(Inventory inventory)
    {
        playerInventory = inventory;

        WeaponActionController.instance.currentWeapon = playerInventory.GetWeapon(0);
        
        HUDController.instance.SetWeaponImage(3 * (int)playerInventory.GetWeapon(0).STAGE + (int)playerInventory.GetWeapon(0).AMMO_TYPE);
        HUDController.instance.SetIconBG((int)playerInventory.GetWeapon(0).AMMO_TYPE);
        HUDController.instance.LoadMagazineDisplay(playerInventory.GetWeapon(0));
        HUDController.instance.DisplayInventoryAmmo(playerInventory.ammo);

        for (int i = 0; i < 3; i++)
            if (playerInventory.GetWeapon(i) != null) HUDController.instance.SetWeaponIcon((int)playerInventory.GetWeapon(i).AMMO_TYPE, (int)playerInventory.GetWeapon(i).STAGE);
    }

    public Inventory GetInventory()
    {
        return playerInventory;
    }

    //Method to find and set to the next available gun in inventorys
    public void ChangeWeapon(int inc, int start, ref Weapon w)
    {
        int val = start;
        val += inc;

        while (val != start) {
            //loops back around the three slots here
            if (val > 2)
                val = 0;
            else if (val < 0)
                val = 2;

            if (playerInventory.GetWeapon(val) != null) {
                //sets gun here
                w = playerInventory.GetWeapon(val);

                HUDController.instance.SetWeaponImage(3 * (int)w.STAGE + (int)w.AMMO_TYPE);
                HUDController.instance.SetIconBG((int)w.AMMO_TYPE);
                HUDController.instance.SetWeaponIcon((int)w.AMMO_TYPE, (int)w.STAGE);
                HUDController.instance.LoadMagazineDisplay(WeaponActionController.instance.currentWeapon);
                break;
            }
            //increment here
            val -= inc;
        }
    }
}

[System.Serializable]
public class Inventory
{
    //All weapon items are stored [Light, Medium, Heavy]
    public int[] ammo = new int[3];
    public int[] AMMO_CAPS = new int[3];

    private Weapon[] Weapons = new Weapon[3];
    private InventoryManager.upVal[] upgrades = new InventoryManager.upVal[9];

    public Inventory()
    {
        //upgrades are stored for each affected individual gun
        //[3 Light, 3 Medium, 3 Heavy]
        //Identifiable with 3*AmmoType+Stage
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = new InventoryManager.upVal();
        }
    }

    //returns the three gun slots
    public Weapon[] GetLoadout()
    {
        return Weapons;
    }
    //returns the three ammo slots
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

    //Used for reloading
    public int SubtractAmmo(WeaponTemplate.AmmoType type, int amount)
    {
        if (ammo[(int)type] < amount)
        {
            //when reloading more bullets than you have set ammo zero and return what remained
            amount = ammo[(int)type];
            ammo[(int)type] = 0;
            return amount;
        }
        else
        {
            //otherwise do it normally
            ammo[(int)type] -= amount;
            return amount;
        }
    }

    public void AddWeapon(WeaponTemplate weapon)
    {
        //replaces whatever is in the slot
        Weapons[(int)weapon.AMMO_TYPE] = new Weapon(weapon, GetUpgrades(weapon));
        HUDController.instance.SetUpgrades();
        HUDController.instance.SetWeaponImage(3 * (int)weapon.STAGE + (int)weapon.AMMO_TYPE);
        HUDController.instance.SetIconBG((int)weapon.AMMO_TYPE);
        HUDController.instance.SetWeaponIcon((int)weapon.AMMO_TYPE, (int)weapon.STAGE);
        WeaponActionController.instance.currentWeapon = GetWeapon((int)weapon.AMMO_TYPE);
    }

    public float[] GetUpgrades(WeaponTemplate weapon)
    {
        return upgrades[3 * (int)weapon.STAGE + (int)weapon.AMMO_TYPE].upgradeValues;
    }

    public void GetUpgradeSlots(ref float[] slots) {
        foreach (Weapon w in Weapons)
        {
            if(w!=null)
            slots[(int)w.AMMO_TYPE] = upgrades[(int)w.AMMO_TYPE*3+(int)w.STAGE].upgradeValues[4];
        }
    }
    public Weapon GetWeapon(int index)
    {
        return Weapons[index];
    }

    public void AddUpgrade(UpgradeTemplate upgrade)
    {
        //see constructor for explanation on math
        int upgradeIndex = (int)upgrade.AMMO_TYPE * 3 + (int)upgrade.STAGE;
        //4 Stage types:All loop through all stages of an ammo type, anything else apply to that stage of that ammo type 
        if (upgrade.STAGE == WeaponTemplate.Stage.all)
        {
            for (int i = (int)upgrade.AMMO_TYPE; i < upgradeIndex; i += 3)
            {
                //adds upgrades
                upgrades[i].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
                //determines which of the 2 all upgrades are added
                upgrades[i].upgradeValues[4] += upgrade.SLOT==1 ? 2:1;
            }
        }
        else
        {
            //adds upgrades
            upgrades[upgradeIndex].upgradeValues[(int)upgrade.UPGRADE_TYPE] += upgrade.AMOUNT;
            //unique upgrade slot covered
            upgrades[upgradeIndex].upgradeValues[4] += 4;
        }

        // applies Upgrades to the player's current weapons
        Weapons[(int)upgrade.AMMO_TYPE]?.AddUpgrades(upgrades[(int)upgrade.AMMO_TYPE * 3 + (int)Weapons[(int)upgrade.AMMO_TYPE].STAGE].upgradeValues);
        HUDController.instance.SetUpgrades();
        HUDController.instance.LoadMagazineDisplay(WeaponActionController.instance.currentWeapon);
    }
}
