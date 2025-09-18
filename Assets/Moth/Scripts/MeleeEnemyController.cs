using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyContoller : LineOfSightControllerr
{
    [SerializeField] float _attackTimer;
    [SerializeField] float _attackCooldown;

    protected override void Start()
    {
        base.Start();
        _stateTimer = 0;
        _enemyState = EnemyState.chasing;
    }

    protected override void DoChasing()
    {
        base.DoChasing();
        _stateTimer -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        print("PLEASE");
        if ((_enemyState == EnemyState.chasing) && collision.collider.CompareTag("Player") && (_stateTimer <= 0))
        {
            print("attack!"); // TODO: CHANGE TO DAMAGE PLAYER
            _stateTimer = _attackCooldown;
        }
    }
}
