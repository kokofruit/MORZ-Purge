//// Kris Herbert
//// 9/18/25-
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        
    }

    protected override void DoAttacking()
    {
        // TODO: Have enemy shoot the player and trigger cooldown, then have them move to idle if player leaves range
    }
}
