// Moth Harper and Kris Herbert
// Controls the basic enemy behavior via a state machine

using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // The toggle for Moth's makeshift debug mode
    [SerializeField] protected bool DEBUG_MODE;

    // The Enemy scriptable object that defines the stats of the enemy
    [SerializeField] protected Enemy _enemyObject;

    #region STATE MACHINE VARIABLES
    // How long the enemy idles for
    [SerializeField] protected int _idleTime;

    // How long the enemy can roam for at most
    [SerializeField] protected int _maxRoamingTime;

    // How many times an enemy can attempt to find a spot to wander to
    [SerializeField] protected int _pathfindingAttempts;

    // The enum of possible states
    protected enum EnemyState { idle, roaming, chasing, attacking, }

    // The current state of the enemy
    protected EnemyState _enemyState = EnemyState.idle;
    #endregion

    #region INTERNAL VARIABLES
    // The damage applied to the player, calculated by multiplying the base damage by the global multiplier
    protected float _calculatedDamage
    {
        get { return _enemyObject.baseDamage * 1; } // TODO: REPLACE WITH GLOBAL MODIFIER
    }

    // The timer keeping track of how long the enemy is in it's current state
    protected float _stateTimer;

    // Components
    protected NavMeshAgent _navMeshAgent;
    #endregion

    #region FUNCTIONS

    protected virtual void Start()
    {
        // Cache components
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Set stats
        _navMeshAgent.speed = _enemyObject.moveSpeed;
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
    // behavior for idle state
    protected virtual void DoIdle()
    {
        // if the idle timer is over, roam or restart idling
        if (_stateTimer <= 0)
        {
            // if a random spot is found, get ready to roam towards it
            if (RandomSpot(out Vector3 _destination))
            {
                // set navmeshagent's destination
                _navMeshAgent.SetDestination(_destination);
                // set the state timer to the maximum roaming time
                _stateTimer = _maxRoamingTime;
                // change the state
                _enemyState = EnemyState.roaming;

                // print debug statement
                if (DEBUG_MODE) print(gameObject.name + ": Set state to roaming");
                if (DEBUG_MODE) print(gameObject.name + ": Set destination to " + _destination);
            }
            // if not, restart the idle state
            else
            {
                _stateTimer = _idleTime;

                // print debug statement
                if (DEBUG_MODE) print(gameObject.name + ": Restarted idle");
            }
            return;
        }

        // countdown idle timer
        _stateTimer -= Time.deltaTime;
    }

    // Find a random spot to navigate to
    // Based on Unity documentation
    bool RandomSpot(out Vector3 result)
    {
        // attempt to sample a random spot on the navmesh
        for (int i = 0; i < _pathfindingAttempts; i++)
        {
            // find a random spot within a random sphere centered around current position
            Vector3 _randomSpot = transform.position + Random.insideUnitSphere * _enemyObject.roamingRange;
            // sample the navmesh at that spot
            if (NavMesh.SamplePosition(_randomSpot, out NavMeshHit _hit, 1f, _navMeshAgent.areaMask))
            {
                // if a spot is successfully found, output it and return true
                result = _hit.position;
                return true;
            }
        }

        // If no successes, output zero and return false
        result = Vector3.zero;
        return false;
    }

    // behavior for roaming state
    protected virtual void DoRoaming()
    {
        // If done navigating or if navigating for too long (in case of being stuck), return to idle mode
        if ((_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) | (_stateTimer <= 0))
        {
            // set the state timer to idling time with some random variation
            _stateTimer = _idleTime * Random.Range(0.75f, 1.25f);
            // set the state
            _enemyState = EnemyState.idle;

            // print debug statement
            if (DEBUG_MODE) print(gameObject.name + ": Set state to idle");
            if (DEBUG_MODE) print(gameObject.name + ": Idling for duration of " + _stateTimer);
            return;
        }

        // countdown roaming timer
        _stateTimer -= Time.deltaTime;
    }

    // behavior for chasing state
    protected virtual void DoChasing()
    {

    }

    // behavior for attacking state
    protected virtual void DoAttacking()
    {

    }

    
    #endregion
}
