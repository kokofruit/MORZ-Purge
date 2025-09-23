// Main Contibutor: Kris Herbert
// Reviewer: 
// Description: 
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        
    }

    protected override void DoAttacking()
    {
        // TODO: Have enemy shoot the player and trigger cooldown, then have them move to idle if player leaves range
        if(_attackingTimer == _attackCooldown)
        {
            ShootProjectile();
            print("shots fired");
        }
        else if( _attackingTimer <= 0)
        {
            _enemyState = EnemyState.chasing;
            return;
        }
    }

    // Function to shot the enemy projectile
    protected void ShootProjectile()
    {
        PlayerDamage();
    }
}
