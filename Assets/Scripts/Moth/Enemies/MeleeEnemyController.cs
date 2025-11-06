// Main Contributors: Moth Harper
// Reviewer: 
// Description: Controls melee enemy behavior by defining how they attack

using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContoller : EnemyController
{

    protected override void InitialAttack()
    {
        base.InitialAttack();

        // print debug
        if (DEBUG_MODE) print(gameObject.name + ": Attack!");
    }
    
}
