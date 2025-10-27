// Contributor: Kris Herbert
// Reviewer: 
// Descritpion: Controller for the flying enemy Sucker
using UnityEngine;

public class SuckerController : FlyingEnemyController
{
    [SerializeField] protected int _lifeGained;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Uses the Start from the FlyingEnemyController
    protected override void Start()
    {
        base.Start();
    }

    // Ovverrides the Sucker's InitialAttack to use the FlyingEnemyController's InitialAttack function but then also call the LifeDrain function after.
    protected override void InitialAttack()
    {
        base.InitialAttack();
        LifeDrain();   
    }

    // This function checks to see if the Sucker enemy's life is less than it's base health and if it is it will gain an amount of health.
    protected void LifeDrain()
    {
        // Checks if the current health and the gained health will be less then the Sucker's base health and that it's health is not 0.
        // If the health and health to be gained is less than base health and health is not 0 it will then gain a pretermined amount of health.
        if ((_health + _lifeGained) <= _baseHealth && _health != 0)
        {
            if (DEBUG_MODE) print(_health);
            if (DEBUG_MODE) print("Gained " + _lifeGained + " Life");
            _health += _lifeGained;
            if (DEBUG_MODE) print(_health);
        }
    }
}
