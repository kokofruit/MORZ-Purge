// Main Contibutor: Kris Herbert
// Reviewer: 
// Description: A child of the EnemyController that adds features for a ranged enemy for the player to fight and avoid.
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected float _projectileForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void DoAttacking()
    {
        /**
         *  When the EnemyState changes to attacking it will check if the enemies attack timer is the same as its cooldown
         *  timer and if it is, will call ShootProjectile() to attack the player.
         *  
         *  It will also check when the attack timer isn't equal to the cooldown and will then start chasing the player while
         *  waiting to fire another shot.
         */
        if (_attackingTimer == _attackCooldown)
        {
            ShootProjectile();
            print("shots fired");
        }
        else if( _attackingTimer <= 0)
        {
            _enemyState = EnemyState.chasing;
            return;
        }
        
        _attackingTimer -= Time.deltaTime;
    }

    // A function that is used to create a copy of the enemy's projectile prefab and has the projectile move forwards from where the enemy shot.
    protected void ShootProjectile()
    {
        EnemyProjectileParent projectile = Instantiate(_projectilePrefab, transform.forward + _eyeTransform.position, Quaternion.identity).GetComponent<EnemyProjectileParent>();
        Vector3 direction = _playerTransform.position - projectile.transform.position;
        projectile.AddForce(direction.normalized * _projectileForce);
    }
}
