using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite image;
    public float cooldown;
    public int ammoType; //1-Light 2-Medium 3-Heavy
    public int ammoCap;
    public int magSize;
    //put bullet here
}
