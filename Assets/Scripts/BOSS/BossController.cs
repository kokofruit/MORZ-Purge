// Main Contributors: Mark Klitsch
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
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    // HEALTH
    [SerializeField] private float _health;

    // ATTACK ARRAY
    protected List<Action> _actions = new();

    // GLOB ATTACK
    [Header("Glob Attack")]
    [SerializeField] protected float _globForce;
    [SerializeField] protected GameObject _globPrefab;
    [SerializeField] protected AudioClip _globAudio;

    // SOUNDS
    [Header("SFX")]
    [SerializeField] protected AudioClip _attackAudio;
    [SerializeField] protected AudioClip _damageAudio;
    [SerializeField] protected AudioClip _deathAudio;

    // COMPONENTS
    private Transform _playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache player transform
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;

        // Add actions to list
        //_actions.Add(ChargeAttack);
        //_actions.Add(BodySlam);
        _actions.Add(ShootGlob);

        // TEMPORARY
        BossAwake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossAwake()
    {
        StartCoroutine(AttackCycle());
    }

    // waiting time in between attacks and cycling between the different attacks
    IEnumerator AttackCycle()
    {
        /*
         * PHASE 1
         * charge attack
         * body slam
         * shoot glob
         * 
         * PHASE 2
         * tenticle sweep
         * launch eggs
         * 
         * PHASE 3
         * Spawn bugs
         * tentical barrage
         */
        while (true)
        {
            Action nextAttack = _actions[UnityEngine.Random.Range(0, _actions.Count)];
            print(nextAttack);
            nextAttack.Invoke();

            yield return new WaitForSeconds(3);
        }
    }

    protected void ChargeAttack()
    {
        /*
         * boss lunges at player
         * deal set player damage
         * when hit wall/player reset back
         */
    }

    protected void BodySlam()
    {
        /*
         * boss slams area near player
         * deal set player damage
         * reset after slam
         */
    }

    protected void ShootGlob()
    {
        /*
         * boss shoots glob at player
         * deal set player damage
         */

        // calculate direction towards player
        Vector3 direction = _playerTransform.position - transform.position;

        // instantiate projectile
        EnemyProjectileParent projectile = Instantiate(_globPrefab, direction.normalized + transform.position, Quaternion.identity).GetComponent<EnemyProjectileParent>();

        // apply force
        projectile.AddForce(direction.normalized * _globForce);

        // play sound
        SoundManager.instance.PlayFXAudio(_globAudio, transform, pitchFluctuation: 0.2f);
    }

    public void TakeDamage(float damage) 
    {
        _health -= damage / (GameManager.instance.GetDifficulty() / 2 + 0.5f);

        // Play sound when taking damage
        // possibly play randomly instead of every hit
        //SoundManager.instance.PlayFXAudio(_damageAudio, transform, pitchFluctuation: 0.2f);

        if (_health <= 0)
        {
            Die();
        }
    }

    // In the event of Boss death
    public void Die()
    {
        // Play sound on death
        SoundManager.instance.PlayFXAudio(_deathAudio, transform, pitchFluctuation: 0.2f);
        //Destroy(bugsplosion, 0.5f);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected void PlayerDamage(int damage)
    {
        _playerTransform.GetComponent<PlayerController>().SubtractHealth(damage);
    }

}
