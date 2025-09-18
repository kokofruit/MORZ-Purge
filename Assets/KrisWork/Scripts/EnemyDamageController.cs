// Kris Herbert
// 9/16/25-9/17/25
using UnityEngine;

public class EnemyDamageController : EnemyController
{
    public int _health;
    // Temporary Variable for testing, can be replaced by finished and existing variables.
    public int playerHeath = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _health = _enemyObject.baseHealth;
    }

    // Function to deal damage to the enemy when the player shoots an enemy.
    protected void EnemyDamage(int damage)
    {
        _health -= damage;
    }
    
    // Function to deal damage to the player when an enemy hits the player.
    protected void PlayerDamage(int damage)
    {

    }
}
