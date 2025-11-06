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
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    // HEALTH
    [SerializeField] private float _health;

    // ATTACK ARRAY
    protected Action[] _actions;

    // SOUNDS
    [Header("SFX")]
    [SerializeField] protected AudioClip _attackAudio;
    [SerializeField] protected AudioClip _damageAudio;
    [SerializeField] protected AudioClip _deathAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        yield return new WaitForSeconds(3);
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
         * boss lunges at player
         * deal set player damage
         * when hit wall/player reset back
         */
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
        //GameObject bugsplosion = Instantiate(_bugDeathExplosion.gameObject, transform.GetChild(0).position, quaternion.identity);
        
        // PLay sound on death
        SoundManager.instance.PlayFXAudio(_deathAudio, transform, pitchFluctuation: 0.2f);
        //Destroy(bugsplosion, 0.5f);
        StopAllCoroutines();
        Destroy(gameObject);
    }


}
