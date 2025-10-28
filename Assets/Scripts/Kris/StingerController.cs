// Contributor: Kris Herbert
// Reviewer: 
// Description: Controller script used for the Stinger Enemy.
using UnityEngine;

public class StingerController : MeleeEnemyContoller
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Calls for the start that is used within MeleeEnemyController which calls on EnemyController

    //NOTE: MeleeEnemyController Script could be used for this enemy but to have things better organized and seperated this StingerController will be used.
    void Start()
    {
        base.Start();
    }
}
