using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTemplate", menuName = "Scriptable Objects/Weapon")]
public class WeaponTemplate : ScriptableObject
{
    public Sprite Image;
    public float COOLDOWN;
    public int RANGE;
    public float FIRE_RATE;
    public enum AmmoType { Light, Medium, Heavy };
    public AmmoType AMMO_TYPE; //1-Light 2-Medium 3-Heavy
    public enum FireSelect { Single, Burst, Auto }
    public FireSelect FIRE_SELECT;
    public int MAG_SIZE;
    //put bullet here
}
