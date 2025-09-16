// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Handles the player input for weapon behaviors and translates them into gameplay actions.

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon_Action_Controller : MonoBehaviour
{
    public WeaponTemplate starterGun;
    public Image hitMarker;
    public float hitMarkerDisplayTime = .05f;

    // Weapon controller runtime variables
    private Player_Controller player;
    private Weapon currentWeapon;
    private bool _isAttacking;
    private float _nextShotTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store a reference to the player controller script
        player = GetComponent<Player_Controller>();

        // Build the test weapon and fill its mag
        currentWeapon = new Weapon(starterGun);
        currentWeapon.Reload();
    }

    // Update is called once per frame
    void Update()
    {
        // Start the weapon's cooldown if it is out of ammo
        if (currentWeapon.ammo <= 0 && !currentWeapon.GetCooldownStatus())
        {
            if (Inventory_Manager.instance.GetAmmo(currentWeapon.AMMO_TYPE) > 0)
                currentWeapon.BeginCooldown();
        }
        // Check if the player is attacking, if the next shot it ready to fire, and the gun is not cooling down.
        else if (_isAttacking && Time.time >= _nextShotTime && !currentWeapon.GetCooldownStatus())
        {
            RaycastHit hit;
            // Fire a "Bullet" (Raycast) in the direction the player is looking and get out the first object hit
            Physics.Raycast(player.head.position, player.head.forward, out hit, currentWeapon.RANGE);
            // Check to make sure the bullet hit something
            if (hit.collider != null)
            {
                // Display the hitmarker image
                StartCoroutine("DisplayHit");
                Debug.Log("Object Hit:" + hit.collider.gameObject.name);
            }
            // Remove a bullet from the weapons magazine
            currentWeapon.SubtractAmmo();
            Debug.Log("Ammo remaining: " + currentWeapon.ammo);

            // Determine the time when the next bullet will be avaible to fire
            _nextShotTime = Time.time + (1f / currentWeapon.FIRE_RATE);
        }
        // If the gun is single fire and the player is attacking but cannot fire, set attacking to false to avoid weapon misfire
        else if (currentWeapon.FIRE_SELECT == WeaponTemplate.FireSelect.Single)
            _isAttacking = false;
    }

    // Displays the hitmarker
    public IEnumerator DisplayHit()
    {
        hitMarker.enabled = true;
        yield return new WaitForSecondsRealtime(hitMarkerDisplayTime);
        hitMarker.enabled = false;
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
        Debug.Log("" + input.Get<float>());
    }
}