// Main Contributors: Moth Harper, Kris Herbert, and Mark Klitsch
// Reviewer: Gabriel Heiser
// Description: Controls the basic enemy behavior via a state machine

using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    // Static instance of the enemy for other scripts to reference
    public static EnemyController instance;

    // The toggle for Moth's makeshift debug mode
    [SerializeField] protected bool DEBUG_MODE;

    // HEALTH
    [Header("Health Variables")]
    // The base health of the enemy
    [SerializeField] protected int _baseHealth;
    // The current health of the enemy
    protected float _health;
    // The explosion created when the bug dies
    [SerializeField] protected ParticleSystem _bugDeathExplosion;

    // ATTACKING
    [Header("Attack Variables")]
    // The base damage of the enemy
    [SerializeField] protected int _baseDamage;
    // The damage applied to the player, calculated by multiplying the base damage by the global multiplier
    protected float _calculatedDamage
    {
        get { return _baseDamage * GameManager.instance.GetDifficulty() / 2; }
    }
    // How close the enemy needs to be to the player to attack
    [SerializeField] protected float _attackDistance;
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
    // the layermask for raycasting to the player, allows enemies to see through certain objects
    LayerMask _layerMask = 1;
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
    protected enum EnemyState { idle, roaming, chasing, attacking }
    // The current state of the enemy
    [SerializeField] protected EnemyState _enemyState = EnemyState.idle;

    // COMPONENTS
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    // The transform of the player
    protected Transform _playerTransform;

    // SOUNDS
    [Header("SFX")]
    [SerializeField] protected AudioClip _attackAudio;
    [SerializeField] protected AudioClip _damageAudio;
    [SerializeField] protected AudioClip _deathAudio;
    // INVISIBILITY SHIELD
    private static bool shieldUpActivated = false;

    #region FUNCTIONS

    // void Awake()
    // {
    //     if (instance == null)
    //         instance = this;
    //     else
    //         Destroy(gameObject);
    // }

    // UNITY LIFECYCYLE FUNCTIONS
    protected virtual void Start()
    {
        // Set enemy health to the base health
        _health = _baseHealth;

        // Cache components
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        // Cache player transform
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;

        // Get the animator
        _animator = transform.GetComponentInChildren<Animator>();

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
            _animator.SetBool("isWalking", true);
            return;
        }
        // if the idle timer is over, roam or restart idling
        if (_idleTimer <= 0)
        {
            // if a random spot is found, get ready to roam towards it
            if (RandomSpot(transform.position, _roamingRange, out Vector3 _destination))
            {
                // set navmeshagent's destination
                _navMeshAgent.SetDestination(_destination);
                // set the state timer to the maximum roaming time
                _roamingTimer = _maxRoamingDuration;
                // change the state
                _enemyState = EnemyState.roaming;
                _animator.SetBool("isWalking", true);

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
        while (true)
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
            if (Physics.Raycast(_eyeTransform.position, direction, out RaycastHit hit, distance + 1f, _layerMask))
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
    protected bool RandomSpot(Vector3 sphereCenter, float sphereSize, out Vector3 result)
    {
        // attempt to sample a random spot on the navmesh
        for (int i = 0; i < _pathfindingAttempts; i++)
        {
            // find a random spot within a random sphere centered around current position
            Vector3 randomSpot = transform.position + UnityEngine.Random.insideUnitSphere * _roamingRange;
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
            _animator.SetBool("isWalking", true);
            return;
        }
        // If done navigating or if navigating for too long (in case of being stuck), return to idle mode
        if ((_navMeshAgent.remainingDistance <= 0) | (_roamingTimer <= 0))
        {
            // clear the navmeshagent's path
            _navMeshAgent.ResetPath();
            // set the state timer to idling time with some random variation
            _idleTimer = _idleDuration * UnityEngine.Random.Range(0.75f, 1.25f);
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
        // If invisibility shield is activated, interrupt the DoChasing process
        if(!shieldUpActivated)
        {
            /** Moth Harper and Kris Herbert
         * if close enough to player and not on cooldown, attack them */
            if ((Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance) && (_attackingTimer <= 0))
            {
                // clear path
                _navMeshAgent.ResetPath();
                // set attack timer
                _attackingTimer = _attackCooldown;
                // change state
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
                _enemyState = EnemyState.idle;
            }
        }
    }

    // behavior for attacking state
    // Specified in subclasses
    protected virtual void DoAttacking()
    {
        // If attack state was just initiated, attack the player
        if (_attackingTimer == _attackCooldown)
        {
            // Initial attack beahvior
            InitialAttack();
        }
        // If the cooldown is up, return to chasing
        else if (_attackingTimer <= 0)
        {
            _enemyState = EnemyState.chasing;
            _animator.SetBool("isWalking", true);
            return;
        }
        // Decrease the attacking timer
        _attackingTimer -= Time.deltaTime;
        
        // Cooldown behavior
        AttackCooldown();
    }

    // Initial attack beahvior
    protected virtual void InitialAttack()
    {
        SoundManager.instance.PlayFXAudio(_attackAudio, transform, pitchFluctuation: 0.2f);
        PlayerDamage();
        _animator.SetTrigger("triggerAttack");
    }

    // Cooldown behavior
    protected virtual void AttackCooldown()
    {

    }

    // OTHER FUNCTIONS
    /** Kris Herbert
     * Function to deal damage to the enemy when the player shoots an enemy. */
    // Moth Harper expansion: attack to damageable interface
    // Mark Klitsch expansion: added sound to when the enemy takes damage/dying
    void IDamageable.TakeDamage(float damage)
    {
        _health -= damage / (GameManager.instance.GetDifficulty() / 2 + 0.5f);
        // Play sound when taking damage
        SoundManager.instance.PlayFXAudio(_damageAudio, transform, pitchFluctuation: 0.2f);

        if (_health <= 0)
        {
            // Play sound when dying
            SoundManager.instance.PlayFXAudio(_deathAudio, transform.position, pitchFluctuation: 0.2f);
            Die();
        }
    }

    // Moth Harper
    // In event of enemy death
    public void Die()
    {
        GameObject bugsplosion = Instantiate(_bugDeathExplosion.gameObject, transform.GetChild(0).position, quaternion.identity);
        int randInt = UnityEngine.Random.Range(0, 50);
        if (randInt < PickupSpawnerManager.instance.tempPickupObjects.Length)
        {
            Instantiate(PickupSpawnerManager.instance.tempPickupObjects[randInt], transform.position, transform.rotation);
        }
        Destroy(bugsplosion, 0.5f);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    /** Kris Herbert
     * Function to deal damage to the player when an enemy hits the player. */
    protected void PlayerDamage()
    {
        if (DEBUG_MODE) print(gameObject.name + "Damaged player by: " + _calculatedDamage);
        _playerTransform.GetComponent<PlayerController>().SubtractHealth(_calculatedDamage);
    }

    /* Vin Lettich
     * Functions to deal with invisibility shield (interrupting the DoChasing for 10s) */
    public static void ActivateUpgrade(int upgradeType)
    {
        shieldUpActivated = true;
    }

    public static void DeactivateUpgrade(int upgradeType)
    {
        shieldUpActivated = false;
    }


    #endregion
}
