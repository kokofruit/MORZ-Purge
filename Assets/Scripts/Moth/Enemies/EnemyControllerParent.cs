// Main Contributors: Moth Harper, Kris Herbert 
// Secondary Contributor: Mark Klitsch, Vin Lettich
// Reviewer: Gabriel Heiser - previous version
// Description: Controls the basic enemy behavior via a state machine

using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerParent : MonoBehaviour, IDamageable
{
    #region ONE MILLION VARIABLES
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
    // the maximum distance the enemy can see the player from; for performance reasons
    [SerializeField] protected float _maxSightDistance;
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
    // Time duration of the attack wind up
    [SerializeField] protected float _windUpTime;
    // boolean to track whether the enemy is on attack cooldown
    protected bool _isOnCooldown;
    // just for keeping track of the state, in case it is needed
    private Coroutine _coroutineState;


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
    #endregion

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

        // Start state machine
        //ChangeState(nameof(IdleBehavior));
        SetState(IdleBehavior);
    }

    #region NAVIGATION AND SIGHT FUNCTIONS
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

            // it is assumed by default that line of sight does not exist
            _lineOfSight = false;


            // If invisibility shield is activated, line of sight does not exist
            if (shieldUpActivated)
            {
                _lineOfSight = false;
            }
            // If player is beyond max seeing distance, line of sight does not exist
            else if (distance > _maxSightDistance)
            {
                _lineOfSight = false;
            }
            // Raycast towards the target. if nothing is hit before the player, line of sight does exist
            else if (Physics.Raycast(_eyeTransform.position, direction, out RaycastHit hit, distance + 1f, _layerMask))
            {
                // if raycast hits something, see if it's the player
                if (hit.collider.CompareTag("Player"))
                {
                    // if it's the player, line of sight exists
                    _lineOfSight = true;
                }
            }

            // Wait to repeat
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
    #endregion

    #region STATE MACHINE BEHAVIOR FUNCTIONS
    protected void ChangeState(string functionName)
    {
        print(typeof(EnemyControllerParent).GetMethod("ChangeState", System.Reflection.BindingFlags.FlattenHierarchy));
        if (this.GetType().GetMethod(functionName) != null)
        {
            if (DEBUG_MODE) print("Changing state to " + functionName);
            // stop old state
            StopAllCoroutines();
            // start new
            StartCoroutine(nameof(LineOfSight));
            _coroutineState = StartCoroutine(functionName);
        }
        else
        {
            Debug.LogError("The requested state is an invalid coroutine!");
        }
    }

    protected void SetState(Func<IEnumerator> coroutine)
    {
        // error proofing
        if (coroutine.Method == null)
        {
            Debug.LogError("That coroutine doesn't exist, man.");
            return;
        }

        print(nameof(coroutine));

        //// stop old state
        //StopAllCoroutines();
        //// start new
        //StartCoroutine(nameof(LineOfSight));
        //_coroutineState = StartCoroutine(nameof(coroutine));
    }

    /** Moth Harper
     * Behavior for idling state */
    protected virtual IEnumerator IdleBehavior()
    {
        // START IDLE
        // change the animation
        _animator.SetBool("isWalking", false);

        // DURING IDLE
        float idleTimer = _idleDuration * UnityEngine.Random.Range(0.75f, 1.25f);
        while (idleTimer > 0f)
        {
            // change state if line of sight exists
            if (_lineOfSight)
            {
                ChangeState(nameof(ChasingBehavior));
            }

            // decrease timer
            idleTimer -= Time.deltaTime;
            // loop next frame
            yield return null;
        }

        // END IDLE
        // if a random spot is found, get ready to roam towards it
        if (RandomSpot(transform.position, _roamingRange, out Vector3 _destination))
        {
            // set navmeshagent's destination
            _navMeshAgent.SetDestination(_destination);
            // change the state
            ChangeState(nameof(RoamingBehavior));
        }
        // if not, restart the idle state
        else
        {
            // Start new coroutine
            ChangeState(nameof(IdleBehavior));
        }
    }

    /** Moth Harper
     * Behavior for roaming state */
    protected virtual IEnumerator RoamingBehavior()
    {
        // START ROAMING
        // change animator
        _animator.SetBool("isWalking", true);

        // DURING ROAMING
        // destination has already been set, so just go towards it
        float roamingTimer = _maxRoamingDuration;
        while (roamingTimer > 0 && _navMeshAgent.remainingDistance > 0)
        {
            // change state if line of sight exists
            if (_lineOfSight)
            {
                ChangeState(nameof(ChasingBehavior));
            }

            // decrease timer
            roamingTimer -= Time.deltaTime;
            // loop next frame
            yield return null;
        }

        // END ROAMING
        // clear the navmeshagent's path
        _navMeshAgent.ResetPath();
        // If done navigating or if navigating for too long (in case of being stuck), return to idle mode
        ChangeState(nameof(IdleBehavior));
    }

    /** Kris Herbert and Moth Harper
     * Behavior for chasing state */
    protected virtual IEnumerator ChasingBehavior()
    {
        // START CHASING
        // change animator
        _animator.SetBool("isWalking", true);

        while (true)
        {
            /** Moth Harper and Kris Herbert
            * if close enough to player and not on cooldown, attack them */
            if ((Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance) && !_isOnCooldown)
            {
                // END CHASING - CAN ATTACK
                // clear path
                _navMeshAgent.ResetPath();
                // change state
                ChangeState(nameof(AttackingBehavior));
            }
            /**
            * Kris Herbert
            * _lineOfSight uses a raycast to check if it can see the player
            * if true than it will change EnemyState to start chasing the player
            * if it's false then it will return to the idle EnemyState.
            */
            else if (!_lineOfSight)
            {
                // END CHASING - NO LINE OF SIGHT
                ChangeState(nameof(IdleBehavior));
            }
            else
            {
                // DO CHASING
                this._navMeshAgent.SetDestination(_playerTransform.position);
                yield return null;
            }
        }
    }

    /** Moth Harper and probably Kris Herbert, I can't remember
    * Behavior for attacking state*/
    protected virtual IEnumerator AttackingBehavior()
    {
        // START ATTACK
        // start cooldown so enemy does not attack twice
        _isOnCooldown = true;

        // WIND UP BEHAVIOR
        // play attack sound for actual attack
        SoundManager.instance.PlayFXAudio(_attackAudio, transform, pitchFluctuation: 0.2f);
        // change animation
        yield return new WaitForSeconds(_windUpTime);

        // ACTUAL ATTACK
        AttackHit();

        // COOLDOWN
        float attackingTimer = _attackCooldown;
        while (attackingTimer > 0)
        {
            AttackCooldown();
            attackingTimer -= Time.deltaTime;
            yield return null;
        }

        // END ATTACK
        // allow attacks again
        _isOnCooldown = false;
        // change states
        ChangeState(nameof(ChasingBehavior));
    }

    // Actual attack beahvior
    protected virtual void AttackHit()
    {
        // if still close enough to player, deal damage
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackDistance) PlayerDamage();
    }

    // Cooldown behavior
    protected virtual void AttackCooldown()
    {

    }

    #endregion

    #region OTHER FUNCTIONS
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
        // make a satisfying bug pop, then destroy it afterwards
        GameObject bugsplosion = Instantiate(_bugDeathExplosion.gameObject, transform.GetChild(0).position, quaternion.identity);
        Destroy(bugsplosion, 0.5f);
        // spawn a pickup, possibly
        if (PickupSpawnerManager.instance.SpawnFromEnemyDeath(out GameObject pickup))
        {
            Instantiate(pickup, transform.position, quaternion.identity);
        }
        // stop coroutines and destroy the bug
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