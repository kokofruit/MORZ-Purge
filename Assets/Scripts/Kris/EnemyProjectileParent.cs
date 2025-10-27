// Main Contribtor: Kris Herbert
// Reviewer: 
// Description: A parent class for all projectiles to use that allows the ranged enemies to shoot the player and deal damage to the player.

using UnityEngine;

public class EnemyProjectileParent : MonoBehaviour
{
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxTime;
    protected Rigidbody _rigidbody;
    protected PlayerController _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        // Finds the PlayerController class and allows other functions to call functions from that class that affects the player.
        _controller = FindAnyObjectByType<PlayerController>();
        // Sets a timer on when the projectile copy is removed fromn the game based on a preset time.
        Invoke("RemoveProjectile", _maxTime);
    }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // OnCollisionEnter checks to see if the cloned projectile has hit the player or any other object other than the player.
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // print("Hit Player");
            ProjectileDamage();
            RemoveProjectile();
        }
        else
        {
            // print("Missed Player");
            RemoveProjectile();
        }
    }

    // A function that removes the clone of a projectile.
    protected void RemoveProjectile()
    {
        Destroy(gameObject);
    }

    // A function that takes health away from the player using a function from the PlayerController class.
    protected void ProjectileDamage()
    {
        _controller.SubtractHealth(_damage);
    }

    // A function that adds force to the projectile when shot. It is called in the RangedEnemyController class.
    public void AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }
}
