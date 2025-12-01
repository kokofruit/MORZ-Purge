// Moth Harper
// This script will control breakable objects in the scene. They will explode when damaged by the player.

using Unity.Mathematics;
using UnityEngine;

public class BreakableController : MonoBehaviour, IDamageable
{
    // Current health of the breakable object at any given time
    [SerializeField] float _health = 1f;
    // The particle system for explosions
    [SerializeField] private GameObject _particleSystemPrefab;
    // The material for the particles to sample from
    [SerializeField] Material _particleMaterial;
    // Optional sound to play
    [SerializeField] AudioClip _breakSound;
    // determines whether the chance this breakable has at spawning a pickup; make it zero for no chance
    [SerializeField, Range(0,1)] private float _spawnChance;
    // The selection of items to spawn
    [SerializeField] private SpawnTable _spawnTable;

    // Using the interface, take damage and die if health falls below zero.
    void IDamageable.TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    // When the breakable "dies", it will destroy the gameobject and create particles with a certain texture
    void Die()
    {
        // Instantiate a particle system
        GameObject newExplosion = Instantiate(_particleSystemPrefab, transform.position, quaternion.identity);
        if (newExplosion.TryGetComponent(out ParticleSystem newPartSystem))
        {
            // stop for modification
            newPartSystem.Stop();

            // modify sampling texture
            if (newPartSystem.TryGetComponent(out ParticleSystemRenderer particleSystemRenderer))
            {
                // if supplied a certain material, use that
                if (_particleMaterial != null)
                {
                    particleSystemRenderer.material = _particleMaterial;
                }
                // otherwise, use the first material of the breakable
                else
                {
                    particleSystemRenderer.material = GetComponent<Renderer>().material;
                }
            }

            // start playing
            newPartSystem.Play();
            // destroy when done
            Destroy(newExplosion, newPartSystem.main.duration);
        }

        // Play a sound if provided
        if (_breakSound != null)
        {
            SoundManager.instance.PlayFXAudio(_breakSound, transform.position, pitchFluctuation: 0.2f);
        }

        // Possibly spawn a random pickup at a provided chance
        if (UnityEngine.Random.value <= _spawnChance)
        {
            // retrive a random pickup
            GameObject pickup = _spawnTable.ChooseItem(UnityEngine.Random.value);
            // instantiate pickup
            Instantiate(pickup, transform.position, quaternion.identity);
        }

        // Destroy self
        Destroy(gameObject);
    }
}
