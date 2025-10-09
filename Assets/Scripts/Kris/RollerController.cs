// Contributor: Kris Herbert
// Reviewer: 
// Description: Controller script used for the Roller Enemy.
using UnityEngine;

public class RollerController : RangedEnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Calls for the start that is used within RangedEnemyController which calls on EnemyController

    //NOTE: RangedEnemyController Script could be used for this enemy but to have things better organized and seperated this RollerController will be used.
    void Start()
    {
        base.Start();
    }
}
