using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private Weapon[] Weapons;
    int currWeaponSlot;
    Weapon currWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currWeaponSlot = 0;
        currWeapon= Weapons[currWeaponSlot];
    }

    // Update is called once per frame
    void Update()
    {
        //temp switch
        if (Input.GetKeyDown(KeyCode.Q)) {
            currWeaponSlot++;
            if (currWeaponSlot >= Weapons.Length) {
                currWeaponSlot = 0;
            }
            currWeapon = Weapons[currWeaponSlot];
        }
    }


    //still needs implementation for full auto
    // private class Weapon {
    //     private int ammo;
    //     private int MAG_SIZE;
    //     private WeaponTemplate STATS;

    //     Weapon(WeaponTemplate S) {
    //         Stats = S;//might be best to transfer all the stats over intead of keeping the template but for now it stays like this
    //         ammo = S.MagSize;
    //         MagSize = S.MagSize;
    //     }

    //     public void Fire() {
    //         ammo--;
    //         //shooty stuff here
    //     }

    //     //discards unused rounds right now
    //     //change later if we want to
    //     public void Reload()
    //     {
    //         //reload cooldown here
    //         if (playerAmmo < MagSize)
    //         {
    //             ammo = playerAmmo;
    //         }
    //         else
    //         {
    //             ammo = MagSize;
    //         }
    //         // take MagSize ammo away from player
    //     }
    // }
}
