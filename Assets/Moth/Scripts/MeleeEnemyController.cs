using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContoller : EnemyController
{
    protected override void DoAttacking()
    {
        // if attack state was just initiated, attack the player
        if (_attackingTimer == _attackCooldown)
        {
            PlayerDamage();
            print("attack");
        }
        // if the cooldown is up, return to chasing
        else if (_attackingTimer <= 0)
        {
            _enemyState = EnemyState.chasing;
            return;
        }
        // decrease the attacking timer
        _attackingTimer -= Time.deltaTime;
    }
    
}
