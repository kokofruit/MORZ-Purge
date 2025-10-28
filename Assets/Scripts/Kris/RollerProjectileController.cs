// Contributor: Kris Herbert
// Reviewer: 
// Description: Controller script for the Roller enemy's projectile.
using Unity.VisualScripting;
using UnityEngine;

public class RollerProjectileController : EnemyProjectileParent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Uses the Start and Awake function from the EnemyProjectileParent
    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Overrides the EnemyProectileParent so that it does not remove the instance of the Rollers projectile when it collides with the ground.
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // print("Hit Player");
            ProjectileDamage();
            RemoveProjectile();
        }
        else if (!collision.gameObject.CompareTag("Ground"))
        {
            RemoveProjectile();
        }
    }
}
