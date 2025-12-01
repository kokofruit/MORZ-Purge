// Main Contributors: Phillip Cano, Domenic Cannella
// Description: Health and Damage for boss

using UnityEngine;
using UnityEngine.Events;


public class BossBody : MonoBehaviour,IDamageable
{
    // HEALTH & DAMAGE
    [SerializeField] private static float _health;
    [SerializeField] protected float _baseContactDamage;
    protected float _currentContactDamage;

    protected static UnityEvent _die;

    private void PlayerDamage(float baseDamage)
    {
        PlayerController.instance.SubtractHealth(baseDamage * GameManager.instance.GetDifficulty() / 2);
    }

    protected virtual void Start()
    {
        // set contact damage
        _currentContactDamage = _baseContactDamage;
    }

    // Damage player on collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDamage(_currentContactDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        // subtract health
        _health -= damage / (GameManager.instance.GetDifficulty() / 2 + 0.5f);

        if (_health < 0)
        {
            _die.Invoke();
        }
        // Play sound when taking damage
        // possibly play randomly instead of every hit
        //SoundManager.instance.PlayFXAudio(_damageAudio, transform, pitchFluctuation: 0.2f);

        // trigger the next phase if needed
        //if ((_phaseIndex == 1) && (_health <= _phaseTwoTrigger))
        /*{
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
        }*/
    }
}
