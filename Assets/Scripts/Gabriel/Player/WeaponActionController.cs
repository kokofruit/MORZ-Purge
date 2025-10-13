// Main Contributor: Gabriel Heiser
// Secondary Contributor: Phillip Cano
// Reviewer: 
// Description: Handles the player input for weapon behaviors and translates them into gameplay actions.

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponActionController : MonoBehaviour
{
    public static WeaponActionController instance;
    public Weapon currentWeapon;

    public Image hitMarker;
    private float _hitMarkerDisplayTime = .05f;
    // Weapon controller runtime variables
    private PlayerController _player;
    private bool _isAttacking;
    private float _nextShotTime;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store a reference to the player controller script
        _player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon != null)
        {
            // Start the weapon's cooldown if it is out of ammo
            if (currentWeapon.ammo <= 0 && !currentWeapon.GetCooldownStatus())
            {
                if (InventoryManager.instance.playerInventory.GetAmmo(currentWeapon.AMMO_TYPE) > 0)
                    BeginCooldown(currentWeapon);
            }
            // Check if the player is attacking, if the next shot it ready to fire, and the gun is not cooling down.
            else if (_isAttacking && Time.time >= _nextShotTime && !currentWeapon.GetCooldownStatus())
            {
                RaycastHit hit;
                // Fire a "Bullet" (Raycast) in the direction the player is looking and get out the first object hit
                Physics.Raycast(_player.head.position, _player.head.forward, out hit, currentWeapon.RANGE);
                // Check to make sure the bullet hit something
                if (hit.collider != null)
                {
                    if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.Explosive)
                    {
                        Collider[] c = Physics.OverlapSphere(hit.point, 5);
                        foreach (Collider e in c)
                        {
                            if (e.tag == "Enemy")
                            {
                                e.GetComponent<EnemyController>().EnemyDamage(currentWeapon.damage);
                                StartCoroutine("DisplayHit");
                            }
                            //make explosive
                        }
                    }
                    else if (hit.collider.tag == "Enemy")
                    {
                        // Display the hitmarker image
                        StartCoroutine("DisplayHit");
                        hit.collider.GetComponent<EnemyController>().EnemyDamage(currentWeapon.damage);
                    }
                }
                // Remove a bullet from the weapons magazine
                currentWeapon.SubtractAmmo();
                // Reflect that change on HUD
                HUDController.instance.DisplayWeaponAmmo(currentWeapon.ammo);

                // Determine the time when the next bullet will be avaible to fire
                _nextShotTime = Time.time + (1f / currentWeapon.fireRate);
            }
            // If the gun is single fire and the player is attacking but cannot fire, set attacking to false to avoid weapon misfire
            else if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.Single)
                _isAttacking = false;

            else if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.Explosive)
                _isAttacking = false;
        }
        else
        {
            Debug.LogError("Current weapon is not set to an instance of an object.");
        }
    }

    // Displays the hitmarker
    public IEnumerator DisplayHit()
    {
        hitMarker.enabled = true;
        yield return new WaitForSecondsRealtime(_hitMarkerDisplayTime);
        hitMarker.enabled = false;
    }

    //Starts reload cooldown coroutine for given weapon
    public void BeginCooldown(Weapon weapon)
    {
        StartCoroutine(Cooldown(weapon));
    }

    public IEnumerator Cooldown(Weapon weapon)
    {
        weapon.SetCoolingStatus(true);
        yield return new WaitForSecondsRealtime(weapon.cooldown);
        weapon.Reload();
        weapon.SetCoolingStatus(false);
        HUDController.instance.DisplayWeaponAmmo(currentWeapon.ammo);
    }

    //Sets held weapon to one from the inventory
    public void GetWeapon(int inc, int start) {
        InventoryManager.instance.ChangeWeapon(inc,start , ref currentWeapon);
    }

    // Handles player attack input action
    public void OnAttack(InputValue input)
    {
        float attackState = input.Get<float>();

        if (attackState == 1)
            _isAttacking = true;
        else
            _isAttacking = false;
    }

    public void OnScroll(InputValue input)
    {
        //0 is no scroll and therefore ignored
        if (input.Get<float>() == 0)
            return;
        GetWeapon((int)input.Get<float>(), (int)currentWeapon.AMMO_TYPE);
    }

    public void OnReload()
    {
        if (currentWeapon != null)
            BeginCooldown(currentWeapon);
    }
}