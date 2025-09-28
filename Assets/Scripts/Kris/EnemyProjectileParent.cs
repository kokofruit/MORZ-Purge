// Main Contribtors: Kris Herbert
// Reviewer: 
// Description: 

using UnityEngine;

public class EnemyProjectileParent : MonoBehaviour
{
    [SerializeField] protected float _projectileForce;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxTime;
    protected Player_Controller _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: detect collision with player; apply damage; add timer to projectile
        _controller = FindAnyObjectByType<Player_Controller>();
        Invoke("RemoveProjectile", _maxTime);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        print("Hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            ProjectileDamage();
            RemoveProjectile();
        }
        else if (collision.gameObject)
        {
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

    public void AddForce()
    {

    }
}
