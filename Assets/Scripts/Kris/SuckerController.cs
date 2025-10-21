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

    protected override void InitialAttack()
    {
        base.InitialAttack();
        LifeDrain();   
    }

    protected void LifeDrain()
    {
        if ((_health + _lifeGained) <= _baseHealth && _health != 0)
        {
            if (DEBUG_MODE) print(_health);
            if (DEBUG_MODE) print("Gained " + _lifeGained + " Life");
            _health += _lifeGained;
            if (DEBUG_MODE) print(_health);
        }
    }
}
