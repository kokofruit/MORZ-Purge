using UnityEngine;

public class EnemyAttackController : LineOfSightControllerr
{
    // the minimum distance the enemy needs to be from the player in order to attack it
    [SerializeField] float _attackDistance;

    protected override void DoChasing()
    {
        base.DoChasing();
    }

    protected override void DoAttacking()
    {
        base.DoAttacking();
    }
}
