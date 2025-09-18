using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeTemplate", menuName = "Scriptable Objects/UpgradeTemplate")]
public class UpgradeTemplate : ScriptableObject
{
    public float AMOUNT;
    public WeaponTemplate.AmmoType AMMO_TYPE;
    public WeaponTemplate.Stage STAGE;
    public enum UpgradeType { damage, fireRate, magSize, cooldown }
    public UpgradeType UPGRADE_TYPE;
}
