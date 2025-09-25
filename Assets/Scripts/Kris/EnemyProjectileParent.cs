// Main Contribtors: Kris Herbert
// Reviewer: 
// Description: 

using UnityEngine;

public class EnemyProjectileParent : MonoBehaviour
{
    [SerializeField]protected float _projectileForce;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxTime;
    protected int _timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: detect collision with player; apply damage; add timer to projectile
    }

    private void Update()
    {
        
    }

    protected void OnCollisionEnter(Collision collision)
    {
        print("Hit");
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }

}
