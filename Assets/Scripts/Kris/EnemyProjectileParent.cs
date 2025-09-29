// Main Contribtor: Kris Herbert
// Reviewer: 
// Description: A parent class for all projectiles to use to allow the ranged enemies to shoot the player and deal damage to the player.

using UnityEngine;

public class EnemyProjectileParent : MonoBehaviour
{
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxTime;
    protected Rigidbody _rigidbody;
    protected Player_Controller _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = FindAnyObjectByType<Player_Controller>();
        Invoke("RemoveProjectile", _maxTime);
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Hit Player");
            ProjectileDamage();
            RemoveProjectile();
        }
        else
        {
            print("Missed Player");
            RemoveProjectile();
        }
    }

    protected void RemoveProjectile()
    {
        Destroy(gameObject);
    }

    protected void ProjectileDamage()
    {
        _controller.SubtractHealth(_damage);
    }

    public void AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }
}
