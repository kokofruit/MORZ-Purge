// Main Contributors: Moth Harper
// Reviewer: 
// Description: Controls melee enemy behavior by defining how they attack

using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContoller : EnemyControllerParent
{

    protected override void AttackHit()
    {
        base.AttackHit();

        // print debug
        if (DEBUG_MODE) print(gameObject.name + ": Attack!");
    }
    
}
