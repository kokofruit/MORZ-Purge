// Contributor: Kris Herbert
// Reviewer: 
// Descritpion: 
using UnityEngine;

public class SuckerController : FlyingEnemyController
{
    [SerializeField] protected int _lifeGained;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    protected override void DoAttacking()
    {
        base.DoAttacking();
        LifeDrain();
        if (_lineOfSight && (Vector3.Distance(transform.position, _playerTransform.position) > _attackDistance))
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
        else
        {
            _navMeshAgent.ResetPath();
        }
    }

    protected void LifeDrain()
    {
        if (_health < _baseHealth && _health != 0)
        {
            _health += _lifeGained;
        }
    }
}
