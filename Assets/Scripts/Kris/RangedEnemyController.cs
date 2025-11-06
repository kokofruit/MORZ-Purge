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

    protected override void InitialAttack()
    {
        EnemyProjectileParent projectile = Instantiate(_projectilePrefab, transform.forward + _eyeTransform.position, Quaternion.identity).GetComponent<EnemyProjectileParent>();
        Vector3 direction = _playerTransform.position - projectile.transform.position;
        projectile.AddForce(direction.normalized * _projectileForce);
        SoundManager.instance.PlayFXAudio(_attackAudio, transform, pitchFluctuation: 0.2f);
        _animator.SetTrigger("triggerAttack");
    }
}
