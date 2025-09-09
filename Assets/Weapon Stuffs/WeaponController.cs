using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Weapon[] Weapons;
    int currWeaponSlot;
    Weapon currWeapon;
    Image GunImage;
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
}
