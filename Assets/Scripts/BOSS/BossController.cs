// Main Contributors: Mark Klitsch, Moth Harper
// Reviewer: Vin, Phil
// Description: Basic behavior for the boss parent

/*
 * ATTACK DAMAGE
 * is each attack going to have the same damage or their own uniqe damage?
 * 
 * SPLIT STAGE 1
 * eventually split stage one into its own script
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamageable
{
    // HEALTH & DAMAGE
    [SerializeField] private float _health;
    [SerializeField] private float _baseContactDamage;
    private float _currentContactDamage;

    // PHASE INFO
    [Header("Phase Variables")]
    [SerializeField] private float _phaseTwoTrigger;
    [SerializeField] private float _phaseThreeTrigger;
    private int _phaseIndex;

    // CHARGE ATTACK
    [Header("Charge Attack")]
    [SerializeField] private float _chargeSpeed;
    [SerializeField] private float _chargeContactDamage;
    [SerializeField] private AudioClip _chargeAudio;

    // GLOB ATTACK
    [Header("Glob Attack")]
    [SerializeField] private float _globForce;
    [SerializeField] private GameObject _globPrefab;
    [SerializeField] private Transform _globSource;
    [SerializeField] private AudioClip _globAudio;

    // SOUNDS
    [Header("SFX")]
    [SerializeField] private AudioClip _attackAudio;
    [SerializeField] private AudioClip _damageAudio;
    [SerializeField] private AudioClip _deathAudio;

    // DEBUG TEXT
    // for development purposes only
    [SerializeField] private TMP_Text _debugText;

    // ATTACK ARRAY
    private List<Func<IEnumerator>> _attacks = new();

    // COMPONENTS
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _playerTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache own components
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        
        // Cache player transform
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;

        // set contact damage
        _currentContactDamage = _baseContactDamage;

        // TEMPORARY
        Invoke(nameof(StartPhaseOne), 3f);
    }

    #region Phase Changes
    public void StartPhaseOne()
    {
        // set phase index
        _phaseIndex = 1;

        // Add phase one attacks to attack list
        _attacks.Add(ChargeAttack);
        // _attacks.Add(BodySlam);
        _attacks.Add(ShootGlob);

        // Start attack cycle
        ChooseNextAttack();
    }

    public void StartPhaseTwo()
    {
        // set phase index
        _phaseIndex = 2;

        // Add phase two attacks to attack list
        _attacks.Add(TendrilSweep);
        _attacks.Add(SpawnBugs);
    }

    public void StartPhaseThree()
    {
        // set phase index
        _phaseIndex = 3;

        // Add phase three attacks to attack list
        _attacks.Add(LaunchEggs);
        _attacks.Add(TendrilBarrage);
    }
    #endregion

    // Choose a random attack and then execute it
    void ChooseNextAttack()
    {
        /*
         * PHASE 1
         * charge attack
         * body slam
         * shoot glob
         * 
         * PHASE 2
         * tentacle sweep
         * spawn bugs
         * 
         * PHASE 3
         * launch eggs
         * tentacle barrage
         */

        // choose a random attack
        Func<IEnumerator> nextAttack = _attacks[UnityEngine.Random.Range(0, _attacks.Count)];
        // run attack
        StartCoroutine(nextAttack.Method.Name);

        // display for debug
        _debugText.SetText(nextAttack.Method.Name);
    }

    #region Phase One Attacks
    private IEnumerator ChargeAttack()
    {
        /*
         * boss lunges at player
         * deal set player damage
         * when hit wall/player reset back
         */
        float timeBeforeNextAttack = 3f;
        // for performance reasons. higher number -> better performance. lower number -> more precise destination
        float destinationIterationModifier = 2.5f; 

        // find direction towards player
        Vector3 direction = _playerTransform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        direction = direction.normalized;

        // iteratively search for farthest reachable point in that direction
        Vector3 destinationCandidate = transform.position;
        while (NavMesh.SamplePosition(destinationCandidate + (direction * destinationIterationModifier), out NavMeshHit hit, 0.5f, _navMeshAgent.areaMask))
        {
            destinationCandidate = hit.position;
        }

        // set speed
        _navMeshAgent.speed = _chargeSpeed;
        // set contact damage
        _currentContactDamage = _chargeContactDamage;

        // set destination to that direction
        _navMeshAgent.SetDestination(destinationCandidate);

        // TODO: start charge noise. like a growl maybe?
        
        // do nothing until finished charging
        while (Vector3.Distance(transform.position, destinationCandidate) > 0f)
        {
            yield return null;
        }

        // TODO: collision noise?

        // once done charging, reset speed
        _navMeshAgent.speed = 0f;
        // reset contact damage
        _baseContactDamage = _chargeContactDamage;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator BodySlam()
    {
        /*
         * boss slams area near player
         * deal set player damage
         * reset after slam
         */
        float timeBeforeNextAttack = 3f;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator ShootGlob()
    {
        /*
         * boss shoots glob at player
         * deal set player damage
         */
        int globAmount = 3;
        float timeBetweenGlobs = 0.5f;
        float timeBeforeNextAttack = 3f;

        // play sound
        SoundManager.instance.PlayFXAudio(_globAudio, transform, pitchFluctuation: 0.2f);

        // spawn three projectiles
        for (int projectileCount = 0; projectileCount < globAmount; projectileCount++)
        {
            // instantiate projectile
            EnemyProjectileParent projectile = Instantiate(_globPrefab, _globSource.position, Quaternion.identity).GetComponent<EnemyProjectileParent>();

            // calculate direction towards player
            Vector3 direction = _playerTransform.position - transform.position;
            // apply force
            projectile.AddForce(direction.normalized * _globForce);

            // wait before globbing again
            yield return new WaitForSeconds(timeBetweenGlobs);
        }

        // cooldown and then choose next attack (subtract glob time since it was just waited)
        yield return new WaitForSeconds(timeBeforeNextAttack - timeBetweenGlobs);
        ChooseNextAttack();
    }

    #endregion
    
    #region Phase Two Attacks

    private IEnumerator TendrilSweep()
    {
        float timeBeforeNextAttack = 3f;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator SpawnBugs()
    {
        float timeBeforeNextAttack = 3f;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion
    
    #region Phase Three Attacks

    private IEnumerator LaunchEggs()
    {
        float timeBeforeNextAttack = 3f;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator TendrilBarrage()
    {
        float timeBeforeNextAttack = 3f;

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion

    #region Damage (Taking and Giving)
    public void TakeDamage(float damage)
    {
        // subtract health
        _health -= damage / (GameManager.instance.GetDifficulty() / 2 + 0.5f);

        // Play sound when taking damage
        // possibly play randomly instead of every hit
        //SoundManager.instance.PlayFXAudio(_damageAudio, transform, pitchFluctuation: 0.2f);

        // trigger the next phase if needed
        if ((_phaseIndex == 1) && (_health <= _phaseTwoTrigger))
        {
            StartPhaseTwo();
        }
        if ((_phaseIndex == 2) && (_health <= _phaseThreeTrigger))
        {
            StartPhaseThree();
        }
        // die if health is below zero
        else if ((_phaseIndex == 3) && (_health <= _phaseThreeTrigger))
        {
            Die();
        }
    }

    // In the event of Boss death
    public void Die()
    {
        // Play sound on death
        SoundManager.instance.PlayFXAudio(_deathAudio, transform, pitchFluctuation: 0.2f);

        // stop function executions and destroy self
        StopAllCoroutines();
        Destroy(gameObject);
    }

    // damage the player
    private void PlayerDamage(float baseDamage)
    {
        _playerTransform.GetComponent<PlayerController>().SubtractHealth(baseDamage * GameManager.instance.GetDifficulty() / 2);
    }

    // Damage player on collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDamage(_currentContactDamage);
        }
    }
    
    #endregion
}
