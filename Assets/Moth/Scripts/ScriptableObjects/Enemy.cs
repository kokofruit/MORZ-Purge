using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    // The health of the enemy
    [SerializeField] public int baseHealth;

    // The base damage of the enemy
    [SerializeField] public float baseDamage;

    // The amount of time before it can attack again
    [SerializeField] public float attackCooldown;

    // The movement speed of the enemy
    [SerializeField] public float moveSpeed;

    // The maximum distance the enemy can roam to at once
    [SerializeField] public float roamingRange;
}
