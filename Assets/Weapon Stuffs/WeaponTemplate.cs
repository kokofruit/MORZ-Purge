using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTemplate", menuName = "Scriptable Objects/Weapon")]
public class WeaponTemplate : ScriptableObject
{
    public Sprite Image;
    public int ReloadCooldown;
    public float FireRate;
    public int AmmoType; //1-Light 2-Medium 3-Heavy
    public int AmmoCap;
    public int MagSize;
    //put bullet here
}
