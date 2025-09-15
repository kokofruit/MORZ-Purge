// Main Contributor: Gabriel Heiser
// Secondary Contributor: Phillip Cano
// Reviewer: 
// Description: Manages weapon behaviors during gameplay. Provides methods to weapons that allow them to
// tap into MonoBehavior resources.

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
