// Main Contributors: Moth Harper and Kris Herbert 
// Reviewer: 
// Description: Controls the basic enemy behavior via a state machine

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // The toggle for Moth's makeshift debug mode
    [SerializeField] protected bool DEBUG_MODE;

    // HEALTH
    [Header("Health Variables")]
    // The base health of the enemy
    [SerializeField] protected int _baseHealth;
    // The current health of the enemy
    protected int _health;

    // ATTACKING
    [Header("Attack Variables")]
    // The base damage of the enemy
    [SerializeField] protected int _baseDamage;
    // The damage applied to the player, calculated by multiplying the base damage by the global multiplier
    protected float _calculatedDamage
    {
        get { return _baseDamage * 1; } // TODO: REPLACE WITH GLOBAL MODIFIER
    }
    // How close the enemy needs to be to the player to attack
    [SerializeField] float _attackDistance;
    // The amount of time before it can attack again
    [SerializeField] protected int _attackCooldown;

    // MOVEMENT
    [Header("Movement Variables")]
    // The movement speed of the enemy
    [SerializeField] protected int _moveSpeed;
    // The maximum distance the enemy can roam to at once
    [SerializeField] protected int _roamingRange;

    // LINE OF SIGHT
    [Header("Line of Sight Variables")]
    // the eye level for the enemy to "see" from
    [SerializeField] protected Transform _eyeTransform;
    // how often the enemy checks if it can see the player
    [SerializeField, Min(0.001f)] float _sightCheckingInterval;
    // tracks whether the enemy can currently see the player
    protected bool _lineOfSight;

    // STATE MACHINE VARIABLES
    [Header("State Machine Variables")]
    // How long the enemy idles for
    [SerializeField] protected int _idleDuration;
    // How long the enemy can roam for at most
    [SerializeField] protected int _maxRoamingDuration;
    // How many times an enemy can attempt to find a spot to wander to
    [SerializeField] protected int _pathfindingAttempts;
    // The timers keeping track of how long the enemy is in it's current state
    protected float _idleTimer;
    protected float _roamingTimer;
    protected float _attackingTimer;
    // The enum of possible states
    protected enum EnemyState { idle, roaming, chasing, attacking, }
    // The current state of the enemy
    protected EnemyState _enemyState = EnemyState.idle;
    // tracks whether the enemy is currently alive
    protected bool _alive = true;

    // COMPONENTS
    protected NavMeshAgent _navMeshAgent;
    // The transform of the player
    protected Transform _playerTransform;

    #region FUNCTIONS

    // UNITY LIFECYCYLE FUNCTIONS
    protected virtual void Start()
    {
        // Cache components
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Cache player transform
        _playerTransform = FindAnyObjectByType<Player_Controller>().transform;

        // Set stats
        _navMeshAgent.speed = _moveSpeed;

        // Start checking for line of sight
        StartCoroutine(LineOfSight());
    }

    protected virtual void Update()
    {
        // Run the expected behavior each frame
        switch (_enemyState)
        {
            case EnemyState.idle:
                DoIdle();
                break;
            case EnemyState.roaming:
                DoRoaming();
                break;
            case EnemyState.chasing:
                DoChasing();
                break;
            case EnemyState.attacking:
                DoAttacking();
                break;
            default:
                break;
        }
    }

    // STATE MACHINE BEHAVIOR FUNCTIONS
    /** Moth Harper
     * Behavior for idling state */
    protected virtual void DoIdle()
    {
        // if the player is visible, set the state to chasing
        if (_lineOfSight)
        {
            _enemyState = EnemyState.chasing;
            _navMeshAgent.stoppingDistance = _attackDistance;
            return;
        }
        // if the idle timer is over, roam or restart idling
        if (_idleTimer <= 0)
        {
            // if a random spot is found, get ready to roam towards it
            if (RandomSpot(out Vector3 _destination))
            {
                // set navmeshagent's destination
                _navMeshAgent.SetDestination(_destination);
                // set the state timer to the maximum roaming time
                _roamingTimer = _maxRoamingDuration;
                // change the state
                _enemyState = EnemyState.roaming;

                // print debug statement
                if (DEBUG_MODE) print(gameObject.name + ": Set state to roaming");
                if (DEBUG_MODE) print(gameObject.name + ": Set destination to " + _destination);
            }
            // if not, restart the idle state
            else
            {
                _idleTimer = _idleDuration;

                // print debug statement
                if (DEBUG_MODE) print(gameObject.name + ": Restarted idle");
            }
            return;
        }

        // countdown idle timer
        _idleTimer -= Time.deltaTime;
    }

    /** Moth Harper*
     * Check if enemy can see player */
    IEnumerator LineOfSight()
    {
        while (_alive)
        {
            // find the direction to the target
            Vector3 direction = _playerTransform.position - _eyeTransform.position;
            // find the distance to the target
            float distance = direction.magnitude;

            // set lineOfSight to false by default
            _lineOfSight = false;

            // debug ray
            if (DEBUG_MODE) Debug.DrawRay(_eyeTransform.position, direction);

            // raycast towards the target
            if (Physics.Raycast(_eyeTransform.position, direction, out RaycastHit hit, distance + 1f))
            {
                // if raycast hits something, see if it's the player
                if (hit.collider.CompareTag("Player"))
                {
                    // if it's the player, set the lineOfSight boolean to true
                    _lineOfSight = true;
                }
            }

            // Wait to repeat
            if (DEBUG_MODE) print(gameObject.name + ": Line of Sight: " + _lineOfSight);
            yield return new WaitForSeconds(_sightCheckingInterval);
        }
    }

    /** Moth Harper
     * Find a random spot to navigate to
     * Based on Unity documentation */
    bool RandomSpot(out Vector3 result)
    {
        // attempt to sample a random spot on the navmesh
        for (int i = 0; i < _pathfindingAttempts; i++)
        {
            // find a random spot within a random sphere centered around current position
            Vector3 randomSpot = transform.position + Random.insideUnitSphere * _roamingRange;
            // sample the navmesh at that spot
            if (NavMesh.SamplePosition(randomSpot, out NavMeshHit hit, 1f, _navMeshAgent.areaMask))
            {
                // if a spot is successfully found, output it and return true
                result = hit.position;
                return true;
            }
        }

        // If no successes, output zero and return false
        result = Vector3.zero;
        return false;
    }

    /** Moth Harper
     * Behavior for roaming state */
    protected virtual void DoRoaming()
    {
        // if the player is visible, set the state to chasing
        if (_lineOfSight)
        {
            _enemyState = EnemyState.chasing;
            _navMeshAgent.stoppingDistance = _attackDistance;
            return;
        }
        // If done navigating or if navigating for too long (in case of being stuck), return to idle mode
        if ((_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) | (_roamingTimer <= 0))
        {
            // set the state timer to idling time with some random variation
            _idleTimer = _idleDuration * Random.Range(0.75f, 1.25f);
            // set the state
            _enemyState = EnemyState.idle;

            // print debug statement
            if (DEBUG_MODE) print(gameObject.name + ": Set state to idle");
            if (DEBUG_MODE) print(gameObject.name + ": Idling for duration of " + _idleTimer);
            return;
        }

        // countdown roaming timer
        _roamingTimer -= Time.deltaTime;
    }

    /** Kris Herbert and Moth Harper
     * Behavior for chasing state */
    protected virtual void DoChasing()
    {
        /** Moth Harper and Kris Herbert
         * if close enough to player and not on cooldown, attack them */
        if ((Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance) && (_attackingTimer <= 0))
        {
            print("attack mode!");
            _attackingTimer = _attackCooldown;
            _enemyState = EnemyState.attacking;
            return;
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

    // behavior for attacking state
    // Specified in subclasses
    protected virtual void DoAttacking()
    {

    }

    // OTHER FUNCTIONS
    /** Kris Herbert
     * Function to deal damage to the enemy when the player shoots an enemy. */
    public void EnemyDamage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _alive = false;
            // TODO: TRIGGER DEATH SEQUENCE
        }
    }

    /** Kris Herbert
     * Function to deal damage to the player when an enemy hits the player. */
    protected void PlayerDamage()
    {
        print(_calculatedDamage);
        // TODO: call function in player
    }


    #endregion
}
