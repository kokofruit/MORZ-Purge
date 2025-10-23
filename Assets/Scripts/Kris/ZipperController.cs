// Contributor: Kris Herbert
// Reviewer: 
// Description: Controller script used for the Zipper Enemy.
using UnityEngine;

public class ZipperController : FlyingEnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Calls for the start that is used within FlyingEnemyController which calls on EnemyController

    //NOTE: FlyingEnemyController Script could be used for this enemy but to have things better organized and seperated this ZipperController will be used.
    void Start()
    {
        base.Start();
    }
}
