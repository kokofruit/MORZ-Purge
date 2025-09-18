using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeTemplate", menuName = "Scriptable Objects/UpgradeTemplate")]
public class UpgradeTemplate : ScriptableObject
{
    public float Amount;
    public WeaponTemplate.AmmoType Type;
    public WeaponTemplate.Stage Stage;
    public enum UpgradeType { damage, fireRate, magSize, cooldown }
    UpgradeType upgradeType;
}
