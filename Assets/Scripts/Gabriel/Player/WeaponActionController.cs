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
    private bool _isAttacking;
    private float _nextShotTime;

    // Variable for ammo upgrade
    private bool ammoUpActivated = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
                HUDController.instance.AnimateRecoil();
                
                RaycastHit hit;
                // Fire a "Bullet" (Raycast) in the direction the player is looking and get out the first object hit
                Physics.Raycast(PlayerController.instance.head.position, PlayerController.instance.head.forward, out hit, currentWeapon.RANGE, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

                // Check to make sure the bullet hit something
                if (hit.collider != null)
                {
                    Debug.Log("" + hit.collider.gameObject.name);
                    if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.AOE)
                    {
                        Collider[] c = Physics.OverlapSphere(hit.point, currentWeapon.AOE_RADIUS);
                        foreach (Collider o in c)
                        {
                            // Hi, Moth addition here. I'm leaving the old stuff as comments, Just In Case(tm).
                            // if hit object within explosion radius has a damageable interface, deal damage
                            if (o.TryGetComponent(out IDamageable damageableInterface))
                            {
                                damageableInterface.TakeDamage(currentWeapon.damage);
                            }

                            // if (e.tag == "Enemy")
                            // {
                            //     e.GetComponent<EnemyController>().EnemyDamage(currentWeapon.damage);
                            //     StartCoroutine("DisplayHit");
                            // }
                            //make explosive
                        }
                    }
                    // Hello again, Moth addition once more. I'm still leaving the old stuff as comments.
                    // if hit object has a damageable interface, deal damage
                    else if (hit.collider.TryGetComponent(out IDamageable damageableInterface))
                    {
                        damageableInterface.TakeDamage(currentWeapon.damage);
                        // display the hitmarker image
                        StartCoroutine(nameof(DisplayHit));
                    }
                    // Old Stuff:
                    // else if (hit.collider.tag == "Enemy")
                    // {
                    //     // Display the hitmarker image
                    //     StartCoroutine("DisplayHit");
                    //     hit.collider.GetComponent<EnemyController>().EnemyDamage(currentWeapon.damage);
                    // }
                }
                // Ignores all of this if unlimited ammo upgrade is active
                if(!ammoUpActivated)
                {
                    // Remove a bullet from the weapons magazine
                    currentWeapon.SubtractAmmo();
                    // Reflect that change on HUD
                    HUDController.instance.LoadMagazineDisplay(currentWeapon);
                }

                // Determine the time when the next bullet will be avaible to fire
                _nextShotTime = Time.time + (1f / currentWeapon.fireRate);
            }
            // If the gun is single fire and the player is attacking but cannot fire, set attacking to false to avoid weapon misfire
            else if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.Single || currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.AOE)
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
        weapon.SetCoolingStatus(true, Time.time);
        HUDController.instance.LoadMagazineDisplay(currentWeapon);
        yield return new WaitForSecondsRealtime(weapon.cooldown);
        weapon.Reload();
        weapon.SetCoolingStatus(false);
        HUDController.instance.LoadMagazineDisplay(currentWeapon);
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
        InventoryManager.instance.ChangeWeapon((int)input.Get<float>(), (int)currentWeapon.AMMO_TYPE , ref currentWeapon);
    }

    /* Vin Lettich
     * Functions to deal with unlimited ammo upgrade
    /*************************/
    public void ActivateUpgrade(int upgradeType)
    {
        ammoUpActivated = true;
    }

    public void DeactivateUpgrade(int upgradeType)
    {
        ammoUpActivated = false;
    }
    /*************************/

    public void OnReload()
    {
        if (currentWeapon != null && !currentWeapon.GetCooldownStatus())
            BeginCooldown(currentWeapon);
    }
}