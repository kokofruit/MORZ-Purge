// Main Contributors: Mark Klitsch, Moth Harper, Domenic Cannella
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

public class BossController : BossBody
{
    

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

    //TENDRIL SLAM
    [Header("Tendril Slam")]
    [SerializeField] private GameObject _tendrilPrefab;
    [SerializeField] private float _tendrilSpawnRadius = 5f;
    [SerializeField] private float _tendrilNavSampleRange = 3f;
    [SerializeField] private AudioClip _tendrilSlamAudio;

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

        // TEMPORARY
        Invoke(nameof(StartPhaseOne), 3f);
    }

    #region Phase Changes
    public void StartPhaseOne()
    {
        // set phase index
        _phaseIndex = 1;

        // Add phase one attacks to attack list
        //_attacks.Add(ChargeAttack);
        // _attacks.Add(BodySlam);
       // _attacks.Add(ShootGlob);


        _attacks.Add(TendrilSlam);
        // Start attack cycle
        ChooseNextAttack();
    }

    public void StartPhaseTwo()
    {
        // set phase index
        _phaseIndex = 2;

        // Add phase two attacks to attack list
        _attacks.Add(TendrilSlam);
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
    
    // TODO: function for boss physically moving between phase locations
    // maybe block off area for boss and maybe player somehow?
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
         * cause stalactites to fall
         * deal set player damage
         * reset after slam
         */
        float timeBeforeNextAttack = 3f;

        // "slam" in place probably because oh boy trajectory is something
        // hurt within a radius
        // spawn stalactites at random locations. amount can be static or based on phase
            // maybe make a random location function that can be used for this, spawn bugs, and tendril barrage
        // stalactites will have their own script to fall, hurt player on contact while falling, then break/destroy after colliding with the ground


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

    private IEnumerator TendrilSlam()
    {
        float timeBeforeNextAttack = 3f;

        //Play audio
       // SoundManager.instance.PlayFXAudio(_tendrilSlamAudio, transform, pitchFluctuation: 0.2f);

        //Pick a random position near the player
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * _tendrilSpawnRadius;
        randomOffset.y = 0f;

        Vector3 desiredPosition = _playerTransform.position + randomOffset;

        //Sample position on the NavMesh
        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, _tendrilNavSampleRange, NavMesh.AllAreas))
        {
            //Spawn tendril at valid navmesh point
            GameObject tendril =Instantiate(_tendrilPrefab, hit.position, Quaternion.identity);
            tendril.transform.LookAt(_playerTransform.position);
            tendril.transform.rotation = Quaternion.Euler(-90f, tendril.transform.rotation.eulerAngles.y + 90f, 0f);

            //Set contact damage
            _currentContactDamage = _baseContactDamage;


        }

        yield return new WaitForSeconds(timeBeforeNextAttack);

        ChooseNextAttack();
    }

    private IEnumerator SpawnBugs()
    {
        float timeBeforeNextAttack = 3f;

        // choose random spots around the ground to have enemies spawn at
        // TODO: figure out how they spawn. eggs? fall in? emerge from ground? i will be mildly sad if they just appear
        // spawn pool could be based on phase (2 vs 3)

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion

    #region Phase Three Attacks

    private IEnumerator LaunchEggs()
    {
        float timeBeforeNextAttack = 3f;

        // like glob attack, but eggs have a change of spawning enemies on contact with ground
        // maybe add a preventative measure to make sure nothing spawns too close to the player

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator TendrilBarrage()
    {
        float timeBeforeNextAttack = 3f;
        
        // tendrils appear at random locations on ground
        // they start with just the tip ;) poking out so the player knows where to avoid
        // after a short period, they burst out and do contact damage

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion


    

    // In the event of Boss death
    public void Die()
    {
        // Play sound on death
        SoundManager.instance.PlayFXAudio(_deathAudio, transform, pitchFluctuation: 0.2f);

        // stop function executions and destroy self
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
