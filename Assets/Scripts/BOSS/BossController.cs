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
using UnityEngine.Events;
using UnityEngine.AI;
using Unity.Mathematics;

public class BossController : BossBody
{


    // PHASE INFO
    [Header("Phase Variables")]
    [SerializeField] private int _startHealth = 30;
    [SerializeField] private int _phaseIndex = 1;
    [SerializeField] private float _attackSpeed = 5f;
    [SerializeField] private BreakableController _phaseDoor;
    private bool _isDying=false;

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

    //TENDRIL BARRAGE
    [Header("Tendril Barrage")]
    [SerializeField] private GameObject _tendrilBarragePrefab;
    [SerializeField] private float _tendrilBarrageSpawnRadius = 5f;
    [SerializeField] private float _tendrilBarrageNavSampleRange = 3f;
    [SerializeField] private AudioClip _tendrilBarrageAudio;

    //SPAWN ATTACK
    [Header("Spawn Attack")]
    [SerializeField] private SpawnTable _spawnTable;
    [SerializeField] private float _SpawnerSpawnRadius = 5f;
    [SerializeField] private float _SpawnerNavSampleRange = 3f;
    [SerializeField] private int _spawnAmount = 10;

    //EGG SPAWN ATTACK
    [Header("Egg_Spawn Attack")]
    [SerializeField] private GameObject _eggPrefab;
    [SerializeField] private float _eggSpawnRadius = 5f;
    [SerializeField] private float _eggNavSampleRange = 3f;
    [SerializeField] private int _eggSpawnAmount = 10;

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
    [SerializeField] private Animator _spriteAnimator;
    private Animator _animator;
    private Transform _playerTransform;
    private BillboardController9000 _billboardController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _startHealth;
        _die = new UnityEvent();
        _die.AddListener(Die);
        // Cache own components
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _billboardController = GetComponent<BillboardController9000>();

        // Cache player transform
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;

        switch (_phaseIndex)
        {
            case 1:
                Invoke(nameof(StartPhaseOne), 3f);
                break;
            case 2:
                Invoke(nameof(StartPhaseTwo), 3f);
                break;
            case 3:
                Invoke(nameof(StartPhaseThree), 3f);
                break;
        }

    }

    #region Phase Changes
    public void StartPhaseOne()
    {

        // Add phase one attacks to attack list
        _attacks.Add(ChargeAttack);
         _attacks.Add(BodySlam);
        _attacks.Add(ShootGlob);
        _attacks.Add(SpawnBugs);

        // Start attack cycle
        ChooseNextAttack();
    }

    public void StartPhaseTwo()
    {

        // Add phase two attacks to attack list
        _attacks.Add(TendrilSlam);

        StartPhaseOne();
    }

    public void StartPhaseThree()
    {
        // Add phase three attacks to attack list
        _attacks.Add(LaunchEggs);
        _attacks.Add(TendrilBarrage);

        StartPhaseTwo();
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
    }

    #region Phase One Attacks
    private IEnumerator ChargeAttack()
    {
        /*
         * boss lunges at player
         * deal set player damage
         * when hit wall/player reset back
         */
        float timeBeforeNextAttack = _attackSpeed;

        //play animation
        _animator.SetTrigger("Charge");
        _spriteAnimator.SetBool("chargeBool", true);


        float delay = UnityEngine.Random.Range(0.1f,0.2f);

        yield return new WaitForSeconds(1.8f-delay);
  
        // set charge damage
        _currentContactDamage = _chargeContactDamage;

        _billboardController.enabled = false;
        yield return new WaitForSeconds(.8f+delay);

        _billboardController.enabled = true;


        // reset contact damage
        _currentContactDamage = _baseContactDamage;
        timeBeforeNextAttack -= (2.6f);

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        _animator.ResetTrigger("Charge");
        _spriteAnimator.SetBool("chargeBool", false);

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
        float timeBeforeNextAttack = _attackSpeed;

        //play animation
        _animator.SetTrigger("BodySlam");
        _spriteAnimator.SetTrigger("slamTrigger");

        float delay = UnityEngine.Random.Range(0.1f, 0.2f);

        yield return new WaitForSeconds(2.2f-delay);

        // set charge damage
        _currentContactDamage = _chargeContactDamage;

        _billboardController.enabled = false;
        yield return new WaitForSeconds(.8f+delay);

        _billboardController.enabled = true;

        // reset contact damage
        _currentContactDamage = _baseContactDamage;
        timeBeforeNextAttack -= (3f);

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);

        //reset animation
        _animator.ResetTrigger("BodySlam");
        _spriteAnimator.ResetTrigger("slamTrigger");
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
        float timeBeforeNextAttack = _attackSpeed/2;

        // play sound
        SoundManager.instance.PlayFXAudio(_globAudio, transform, pitchFluctuation: 0.2f);

        //play animation
        _spriteAnimator.SetBool("globBool", true);
        yield return new WaitForSeconds(.5f);

        // spawn three projectiles
        for (int projectileCount = 0; projectileCount < globAmount; projectileCount++)
        {
            _spriteAnimator.SetTrigger("globTrigger");

            // instantiate projectile
            EnemyProjectileParent projectile = Instantiate(_globPrefab, _globSource.position, Quaternion.identity).GetComponent<EnemyProjectileParent>();

            // calculate direction towards player
            //this is a stupid way of fixing an even stupider problem
            //With Love, Mark and Phill(mainly phills(me) stupid brine)
            Vector3 direction = new Vector3(_playerTransform.position.x, _playerTransform.position.y-28, _playerTransform.position.z);
            direction =  direction - transform.position;
           
            // apply force
            projectile.AddForce(direction.normalized * _globForce);

            // wait before globbing again
            yield return new WaitForSeconds(timeBetweenGlobs);
        }

        //reset animation
        _spriteAnimator.SetBool("globBool", false);
        _spriteAnimator.ResetTrigger("globTrigger");
        // cooldown and then choose next attack (subtract glob time since it was just waited)
        yield return new WaitForSeconds(timeBeforeNextAttack - timeBetweenGlobs);
        ChooseNextAttack();
    }

    #endregion

    #region Phase Two Attacks

    private IEnumerator TendrilSlam()
    {
        float timeBeforeNextAttack = _attackSpeed*0.6f;

        //play animation
        _spriteAnimator.SetTrigger("tendrilTrigger");

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

        //reset animation
        _spriteAnimator.ResetTrigger("tendrilTrigger");
        yield return new WaitForSeconds(timeBeforeNextAttack);

        ChooseNextAttack();
    }

    private IEnumerator SpawnBugs()
    { 
        float timeBeforeNextAttack = _attackSpeed / 2;

        // play sound
        //SoundManager.instance.PlayFXAudio(_globAudio, transform, pitchFluctuation: 0.2f);

        //play animation
        _spriteAnimator.SetTrigger("tendrilTrigger");
        yield return new WaitForSeconds(.5f);

        // spawn three projectiles
        for (int spawnCount = 0; spawnCount < _spawnAmount; spawnCount++)
        {
            //Pick a random position near the boss
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * _SpawnerSpawnRadius;
            randomOffset.y = 0f;

            Vector3 desiredPosition = _globSource.position + randomOffset;
            
            //Sample position on the NavMesh
            if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, _SpawnerNavSampleRange, NavMesh.AllAreas))
            {
                //Spawn spawners at valid navmesh point
                Instantiate(_spawnTable.ChooseItem(UnityEngine.Random.Range(0f , 1f)), hit.position, Quaternion.identity);
            }
        }

        //reset animation
        _spriteAnimator.ResetTrigger("tendrilTrigger");
        // cooldown and then choose next attack (subtract glob time since it was just waited)
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion

    #region Phase Three Attacks

    private IEnumerator LaunchEggs()
    {
        float timeBeforeNextAttack = _attackSpeed / 2;

        // play sound
        //SoundManager.instance.PlayFXAudio(_globAudio, transform, pitchFluctuation: 0.2f);

        //play animation
        _spriteAnimator.SetTrigger("tendrilTrigger");
        yield return new WaitForSeconds(.5f);

        // spawn three projectiles
        for (int spawnCount = 0; spawnCount < _eggSpawnAmount; spawnCount++)
        {
            //Pick a random position near the boss
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * _eggSpawnRadius;
            randomOffset.y = 0f;

            Vector3 desiredPosition = _globSource.position + randomOffset;

            //Sample position on the NavMesh
            if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, _eggNavSampleRange, NavMesh.AllAreas))
            {
                //Spawn spawners at valid navmesh point
                Instantiate(_eggPrefab, hit.position, Quaternion.identity);
            }
        }

        //reset animation
        _spriteAnimator.ResetTrigger("tendrilTrigger");
        // cooldown and then choose next attack (subtract glob time since it was just waited)
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    private IEnumerator TendrilBarrage()
    {
        float timeBeforeNextAttack = _attackSpeed * 0.6f;

        // tendrils appear at random locations on ground
        // they start with just the tip ;) poking out so the player knows where to avoid
        // after a short period, they burst out and do contact damage

        //play animation
        _spriteAnimator.SetTrigger("tendrilTrigger");

        //Play audio
        // SoundManager.instance.PlayFXAudio(_tendrilSlamAudio, transform, pitchFluctuation: 0.2f);

        //Pick a random position near the player
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * _tendrilBarrageSpawnRadius;
        randomOffset.y = 0f;

        Vector3 desiredPosition = _playerTransform.position + randomOffset;

        //Sample position on the NavMesh
        if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, _tendrilBarrageNavSampleRange, NavMesh.AllAreas))
        {
            //Spawn tendril at valid navmesh point
            GameObject tendril = Instantiate(_tendrilBarragePrefab, hit.position, Quaternion.identity);
            tendril.transform.LookAt(_playerTransform.position);
            tendril.transform.rotation = Quaternion.Euler(-90f, tendril.transform.rotation.eulerAngles.y + 90f, 0f);

            //Set contact damage
            _currentContactDamage = _baseContactDamage;
        }

        //reset animation
        _spriteAnimator.ResetTrigger("tendrilTrigger");

        // cooldown and then choose next attack
        yield return new WaitForSeconds(timeBeforeNextAttack);
        ChooseNextAttack();
    }

    #endregion


    

    // In the event of Boss death
    public void Die()
    {
        //bool to prevent the Die() method from running a lot
        if (_isDying) return;
        _isDying = true;

        // Play sound on death
        //SoundManager.instance.PlayFXAudio(_deathAudio, transform, pitchFluctuation: 0.2f);

        //Play Death Animation
        _animator.SetTrigger("Death");

        // stop function executions and destroy self after 5 seconds
        StopAllCoroutines();
        Destroy(gameObject,4.7f);

        // put here by VIN
        // if rock is rock of the cosmos, play end dialogue
        if (_phaseDoor.name == "Rockofthecosmos")
            DialogueManager.instance.BossDied();

        //break rock to go to next stage of boss.
        _phaseDoor.Die();

    }
}
