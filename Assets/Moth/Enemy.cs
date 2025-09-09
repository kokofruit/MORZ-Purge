using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Stats variables
    [SerializeField] protected string _species;
    [SerializeField] protected int _health;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _attackCooldown;
    [SerializeField] protected int _moveSpeed;
    [SerializeField] protected int _wanderingRange;

    // State Machine variables
    protected enum EnemyState { idle, roaming, chasing, attacking, }
    protected EnemyState _enemyState = EnemyState.idle;

    protected int _idleTimer;
    protected Vector3 _roamingDestination;


    // Update Function
    protected virtual void Update()
    {
        switch (_enemyState)
        {
            case EnemyState.idle:
                DoIdle();
                break;
            case EnemyState.roaming:
                DoRoaming();
                break;
            case EnemyState.chasing:
                DoChasing();
                break;
            case EnemyState.attacking:
                DoAttacking();
                break;
            default:
                break;
        }
    }

    // State Machine Functions
    protected virtual void DoIdle()
    {

    }

    protected virtual void DoRoaming()
    {

    }

    protected virtual void DoChasing()
    {

    }

    protected virtual void DoAttacking()
    {
        
    }
}
