// Main Contributors: Moth Harper
// Reviewer: 
// Description: Controls the chomper enemy by modifying the melee class's attacking state

using UnityEngine;

public class ChomperController : MeleeEnemyContoller
{
    // Pursue the player while in cooldown
    protected override void DoAttacking()
    {
        base.DoAttacking();
        if (_lineOfSight)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
    }
}
