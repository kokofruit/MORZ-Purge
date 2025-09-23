using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyController : EnemyController
{
    [Header("Flying Enemy Variables")]
    [SerializeField] protected float _flyingHeight;
    [SerializeField] protected float _swoopDistance;
    [SerializeField] protected float _swoopSpeed;

    protected override void Start()
    {
        base.Start();

        // Set the enemy to hover above the ground
        _navMeshAgent.baseOffset = _flyingHeight;
    }

    /** Kris Herbert and Moth Harper
     * Behavior for chasing state
     * Customized for flying enemies */
    protected override void DoChasing()
    {
        // Calculate the distance between the enemy and player
        float playerDistance = Vector3.Distance(transform.position, _playerTransform.position);

        // if the enemy is close to swoop, lower to the ground
        if (playerDistance <= _swoopDistance)
        {
            _navMeshAgent.baseOffset = Mathf.MoveTowards(_navMeshAgent.baseOffset, 0, _swoopSpeed * Time.deltaTime);

            /** Moth Harper and Kris Herbert
            * if close enough to player and not on cooldown, attack them */
            if ((playerDistance <= _attackDistance) && (_attackingTimer <= 0))
            {
                _navMeshAgent.stoppingDistance = 0;
                _attackingTimer = _attackCooldown;
                _enemyState = EnemyState.attacking;
                return;
            }
        }

        /**
        * Kris Herbert
        * _lineOfSight uses a raycast to check if it can see the player
        * if true than it will change EnemyState to start chasing the player
        * if it's false then it will return to the idle EnemyState.
        */

        if (_lineOfSight == true)
        {
            this._navMeshAgent.SetDestination(_playerTransform.position);
        }
        else
        {
            _idleTimer = _idleDuration;
            _navMeshAgent.stoppingDistance = 0;
            _enemyState = EnemyState.idle;
        }
    }

    protected override void DoAttacking()
    {
        // if attack state was just initiated, attack the player
        if (_attackingTimer == _attackCooldown)
        {
            PlayerDamage();
            if (DEBUG_MODE) print(gameObject.name + ": Attack!");
            Flee();
        }
        // if the cooldown is up, return to chasing
        else if (_attackingTimer <= 0)
        {
            _navMeshAgent.stoppingDistance = _attackDistance;
            _enemyState = EnemyState.chasing;
            return;
        }
        // rise back up
        //_navMeshAgent.baseOffset = Mathf.MoveTowards(_navMeshAgent.baseOffset, _flyingHeight, _swoopSpeed * Time.deltaTime);
        
        // decrease the attacking timer
        _attackingTimer -= Time.deltaTime;
    }

    void Flee()
    {
        Vector3 direction = _eyeTransform.position - _playerTransform.position;
        // run away from player
        if (RandomSpot(transform.position + direction * 3, 0.5f, out Vector3 hit))
        {
            _navMeshAgent.SetDestination(hit);
        }
    }
}
