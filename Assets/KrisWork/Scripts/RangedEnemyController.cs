// Kris Herbert
// 9/18/25-
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
        // check to see if the player is within a set distance for the enemy to attack
        if(playerLocation.position <= _attackDistance)
        {
            // have the enemy attack the player
            _enemyState = EnemyState.attacking;
        }

        //create a cool down for the enemy so it does not continously shoot the player
    }

    /** 
     *
     *
     *
     */
}
