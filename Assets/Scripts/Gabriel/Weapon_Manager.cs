using UnityEngine;
using System.Collections;

public class Weapon_Manager : MonoBehaviour
{
    public static Weapon_Manager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void BeginCooldown(Weapon weapon)
    {
        StartCoroutine(Cooldown(weapon));
    }

    public static IEnumerator Cooldown(Weapon weapon)
    {
        weapon.SetCoolingStatus(true);
        Debug.Log("Cooldown Started");
        yield return new WaitForSecondsRealtime(weapon.COOLDOWN);
        weapon.Reload();
        weapon.SetCoolingStatus(false);
        Debug.Log("Cooldown Ended");
    }
}

public class Weapon
{
    public Weapon(WeaponTemplate weapon)
    {
        MAG_SIZE = weapon.MAG_SIZE;
        RANGE = weapon.RANGE;
        FIRE_SELECT = weapon.FIRE_SELECT;
        FIRE_RATE = weapon.FIRE_RATE;
        COOLDOWN = weapon.COOLDOWN;
    }

    public int MAG_SIZE { get; private set; }
    public int ammo { get; private set; }
    public int RANGE { get; private set; }
    public WeaponTemplate.FireSelect FIRE_SELECT { get; private set; }
    public float FIRE_RATE { get; private set; }
    public float COOLDOWN { get; private set; }
    public bool _isCooling;

    public void SubtractAmmo()
    {
        ammo -= 1;
    }
    public void SetCoolingStatus(bool status)
    {
        _isCooling = status;
    }
    public bool GetCooldownStatus()
    {
        return _isCooling;
    }
    public void Reload()
    {
        ammo = MAG_SIZE;
        // //reload cooldown here
        // if (playerAmmo < )
        // {
        //     ammo = playerAmmo;
        // }
        // else
        // {
        //     ammo = MagSize;
        // }
        // take MagSize ammo away from player
    }
    public void BeginCooldown()
    {
        Weapon_Manager.instance.BeginCooldown(this);
    }
};