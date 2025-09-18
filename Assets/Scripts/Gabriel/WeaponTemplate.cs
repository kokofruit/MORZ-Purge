// Main Contributor: Phillip Cano
// Secondary Contributor: Gabriel heiser
// Reviewer: 
// Description: Scriptable object that stores the attributes for in game weapons.

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTemplate", menuName = "Scriptable Objects/Weapon")]
public class WeaponTemplate : ScriptableObject
{
    public Sprite Image;
    public float DAMAGE;
    public float COOLDOWN;
    public int RANGE;
    public float FIRE_RATE;
    public enum AmmoType { Light, Medium, Heavy };
    public AmmoType AMMO_TYPE; //1-Light 2-Medium 3-Heavy
    public enum Stage { first, second, third, all };
    public Stage STAGE;
    public enum FireSelect { Single, Auto }
    public FireSelect FIRE_SELECT;
    public int MAG_SIZE;
    //put bullet here
}