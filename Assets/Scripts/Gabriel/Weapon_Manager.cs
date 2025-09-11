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
        yield return new WaitForSecondsRealtime(weapon.cooldown);
        weapon.Reload();
        weapon.SetCoolingStatus(false);
        Debug.Log("Cooldown Ended");
    }
}

public class Weapon
{
    public Weapon(int magSize, int range, FireSelect fireSelect, float fireRate, float cooldown)
    {
        this.magSize = magSize;
        this.range = range;
        this.fireSelect = fireSelect;
        this.fireRate = fireRate;
        this.cooldown = cooldown;
    }

    public int magSize { get; private set; }
    public int ammo { get; private set; }
    public int range { get; private set; }
    public enum FireSelect { single, burst, auto }
    public FireSelect fireSelect { get; private set; }
    public float fireRate { get; private set; }
    public float cooldown { get; private set; }
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
        ammo = magSize;
    }
    public void BeginCooldown()
    {
        Weapon_Manager.instance.BeginCooldown(this);
    }
};