// Main Contributor: Moth Harper
// Secondary Contributor: Kris Herbert 
// Reviewer: 
// Description: Controls the flying enemies by modifying chasing and attacking behavior

using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyController : EnemyControllerParent
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
    protected override IEnumerator ChasingBehavior()
    {
        // START CHASING
        // change animator
        _animator.SetBool("isWalking", true);
        
        while (true)
        {
            // Calculate the distance between the enemy and player
            float playerDistance = Vector3.Distance(transform.position, _playerTransform.position);

            // PART OF DO CHASING
            // if the enemy is close enough to swoop, lower to the ground
            if (playerDistance <= _swoopDistance)
            {
                _navMeshAgent.baseOffset = Mathf.MoveTowards(_navMeshAgent.baseOffset, 0, _swoopSpeed * Time.deltaTime);
            }

            /**
            * Kris Herbert
            * _lineOfSight uses a raycast to check if it can see the player
            * if true than it will change EnemyState to start chasing the player
            * if it's false then it will return to the idle EnemyState.
            */
            if (!_lineOfSight)
            {
                // END CHASING - NO LINE OF SIGHT
                SetState(IdleBehavior);
                yield return null;
                break;
            }
            /** Moth Harper and Kris Herbert
                * if close enough to player, low enough to ground, and not on cooldown, attack them */
            else if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance && _navMeshAgent.baseOffset <= 1f && !_isOnCooldown)
            {
                // END CHASING - CAN ATTACK
                // clear path
                _navMeshAgent.ResetPath();
                // change state
                SetState(AttackingBehavior);
                yield return null;
                break;
            }
            else
            {
                // OTHER PART OF DO CHASING
                // move towards player
                this._navMeshAgent.SetDestination(_playerTransform.position);
                // wait for next frame
                yield return null;
            }
        }
    }

    protected override void AttackHit()
    {
        base.AttackHit();

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
        // run away from player
        if (RandomSpot(_playerTransform.position, _swoopDistance, out Vector3 hit))
        {
            _navMeshAgent.SetDestination(hit);
        }
    }
}
