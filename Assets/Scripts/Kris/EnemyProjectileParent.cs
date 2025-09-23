// Main Contribtors: Kris Herbert
// Reviewer: 
// Description: 

using UnityEngine;

public class EnemyProjectileParent : MonoBehaviour
{
    protected float _projectileForce;
    protected GameObject _enemyProjectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: detect collision with player; apply damage; 
    }

    protected void OnTriggerEnter(Collider collision)
    {
        print("Hit");
        if(collision.gameObject.CompareTag("Player"))
        {

        }
    }

}
