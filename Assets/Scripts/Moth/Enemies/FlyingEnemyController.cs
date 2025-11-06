// Main Contributor: Moth Harper
// Secondary Contributor: Kris Herbert 
// Reviewer: 
// Description: Controls the flying enemies by modifying chasing and attacking behavior

using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyController : EnemyController
{
    [Header("Flying Enemy Variables")]
    // the normal height the enemy flies at, controls the base offset
    [SerializeField] protected float _flyingHeight;
    // how close the enemy needs to be before it swoops
    [SerializeField] protected float _swoopDistance;
    // how quickly the enemy descends/ascends
    [SerializeField] protected float _swoopSpeed;

    // Temporary testing audio for the sound
    //[SerializeField] AudioClip _testSound;

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
            _enemyState = EnemyState.idle;
        }
    }

    protected override void InitialAttack()
    {
        base.InitialAttack();

        // damage player
        PlayerDamage();
        // sound testing
        //SoundManager.instance.PlayFXAudio(_testSound, transform);
        // print debug statement
        if (DEBUG_MODE) print(gameObject.name + ": Attack!");
        // flee from player
        Flee();
    }

    protected override void AttackCooldown()
    {
        // rise back up
        _navMeshAgent.baseOffset = Mathf.MoveTowards(_navMeshAgent.baseOffset, _flyingHeight, _swoopSpeed * Time.deltaTime);
    }

    protected virtual void Flee()
    {
        Vector3 direction = _eyeTransform.position - _playerTransform.position;
        // run away from player
        if (RandomSpot(_playerTransform.position, _swoopDistance, out Vector3 hit))
        {
            _navMeshAgent.SetDestination(hit);
        }
    }
}
