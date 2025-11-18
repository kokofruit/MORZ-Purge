// Main Contributors: Moth Harper
// Reviewer: 
// Description: Controls the chomper enemy by modifying the melee class's attacking state

using UnityEngine;

public class ChomperController : MeleeEnemyContoller
{
    // Pursue the player while in cooldown
    protected override void AttackCooldown()
    {
        if (_lineOfSight && (Vector3.Distance(transform.position, _playerTransform.position) > _attackDistance))
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
        else
        {
            _navMeshAgent.ResetPath();
        }
    }
}
