// Contributor: Kris Herbert
// Secondary Contributor: Mark Klitsch
// Reviewer: 
// Description: Controller script used for the Zipper Enemy.
using UnityEngine;

public class ZipperController : FlyingEnemyController
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
