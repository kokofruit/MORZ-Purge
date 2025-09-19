using UnityEngine;

public class ChomperController : MeleeEnemyContoller
{
    protected override void DoAttacking()
    {
        base.DoAttacking();
        if (_lineOfSight)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
    }
}
