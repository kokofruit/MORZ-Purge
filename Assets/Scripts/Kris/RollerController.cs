// Contributor: Kris Herbert
// Secondary Contributor: Mark Klitsch
// Reviewer: 
// Description: Controller script used for the Roller Enemy.
using UnityEngine;

public class RollerController : RangedEnemyController
{
    protected override void Start()
    {
        base.Start();
    }

    // Overrides the Rollers InitialAttack to use the RangedEnemeyController InitialAttack
    // Playes enemies attack sound
    protected override void InitialAttack()
    {
        base.InitialAttack();
        SoundManager.instance.PlayFXAudio(_attackAudio, transform);
    }
}
