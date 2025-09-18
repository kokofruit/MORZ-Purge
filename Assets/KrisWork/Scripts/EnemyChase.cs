// Kris Herbert
// 9/11/25-9/16/25
using UnityEngine;

public class EnemyChase : EnemyController
{
    // Temporary variables to test if the enemy will chase the player.
    public bool _lineOfSight;
    public Transform playerLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _enemyState = EnemyState.chasing;
    }

    protected override void DoChasing()
    {
        /**
        * _lineOfSight uses a raycast to check if it can see the player
        * if true than it will change EnemyState to start chasing the player
        * if it's flase then it will return to the idle EnemyState.
        */

        if (_lineOfSight == true)
        {
            this._navMeshAgent.SetDestination(playerLocation.position);
        }
        else
        {
            _enemyState = EnemyState.idle;
        }
    }
}
