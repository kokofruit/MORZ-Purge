// Main Contributors: Moth Harper
// Reviewer: 
// Description: Controls melee enemy behavior by defining how they attack

using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContoller : EnemyController
{

    protected override void InitialAttack()
    {
        // damage player
        PlayerDamage();
        // print debug
        if (DEBUG_MODE) print(gameObject.name + ": Attack!");
    }
    
}
