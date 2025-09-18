// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Manages the player's inventory during runtime.

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_Manager : MonoBehaviour
{
    public static Inventory_Manager instance;
    public int LIGHT_AMMO_CAP;
    public int MEDIUM_AMMO_CAP;
    public int HEAVY_AMMO_CAP;

    private int[] ammo = new int[3];
    private int[] AMMO_CAPS = new int[3];
    private Weapon[] Weapons = new Weapon[3];

    // stuff
    public Image[] gunImages;

    //delete later
    public WeaponTemplate[] gunTemplates=new WeaponTemplate[3];
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //delete later
        foreach(WeaponTemplate e in gunTemplates) {
            if (e != null)
            {
                temp(e);
                gunImages[(int)e.AMMO_TYPE].enabled = false;
            }
        }

        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light] = LIGHT_AMMO_CAP;
        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium] = MEDIUM_AMMO_CAP;
        AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy] = HEAVY_AMMO_CAP;

        ammo[(int)WeaponTemplate.AmmoType.Light] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Light];
        ammo[(int)WeaponTemplate.AmmoType.Medium] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Medium];
        ammo[(int)WeaponTemplate.AmmoType.Heavy] = AMMO_CAPS[(int)WeaponTemplate.AmmoType.Heavy];
    
        
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
        if (ammo[(int)type] < amount) {
            ammo[(int)type] = 0;
            return ammo[(int)type];
        }
        else {
            ammo[(int)type] -= amount;
            return amount;
        }
    }

    //temp add gun method
    public void temp(WeaponTemplate w) {

            Weapons[(int)w.AMMO_TYPE] = new Weapon(w);
            gunImages[(int)w.AMMO_TYPE].sprite=w.Image;
        
        //add chnge weapon stuff here stuff
    }
    public Weapon ChangeWeapon(int inc, int start) {
        //theres alot here that is temperary and will break later so shushy
        gunImages[start].enabled = false;
        start += inc;
        while (Weapons[start]==null) { 
            start++;
            Mathf.Repeat(start, 2);
            Debug.Log(start);
        }
        gunImages[start].enabled = true;
        return Weapons[start];
    }
}
