using Unity.VisualScripting;
using UnityEngine;

public class RollerProjectileController : EnemyProjectileParent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }

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
