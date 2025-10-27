// Contributor: Kris Herbert
// Secondary Contributor: Mark Klitsch
// Reviewer: 
// Description: Controller script used for the Stinger Enemy.
using UnityEngine;

public class StingerController : MeleeEnemyContoller
{
    [SerializeField] AudioClip _attackAudio;
    [SerializeField] AudioClip _damageAudio;
    [SerializeField] AudioClip _deathAudio;

    protected override void Start()
    {
        base.Start();
    }

    protected override void InitialAttack()
    {
        base.InitialAttack();
        SoundManager.instance.PlayFXAudio(_attackAudio, transform);
    }

}
