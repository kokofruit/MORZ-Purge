using UnityEngine;

public class EnemyAttackController : LineOfSightControllerr
{
    // the minimum distance the enemy needs to be from the player in order to attack it
    [SerializeField] float _attackDistance;
    // [SerializeField] float _timeToDamage;
    // [SerializeField] float _attackTime;

    protected override void Start()
    {
        base.Start();
         _enemyState = EnemyState.chasing;
    }

    protected override void DoChasing()
    {
        base.DoChasing();

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= _attackDistance)
        {
            print("attack mode!");
            _enemyState = EnemyState.attacking;
            return;
        }
    }

    // protected override void DoAttacking()
    // {
    //     // if the attack cooldown is done, return to cooldown
    //     if (_stateTimer <= 0)
    //     {
    //         _enemyState = EnemyState.chasing;
    //         return;
    //     }
    //     // if the attack cooldown is d
    //     else if (_stateTimer == _attackTime)
    //     {
    //         Attack();
    //     }

    //     _stateTimer -= Time.deltaTime;

    // }

    // // TODO: Replace with player damage
    // protected void Attack()
    // {
    //     print("attack");
    // }
}
