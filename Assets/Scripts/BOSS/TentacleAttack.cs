// Main Contributors: Domenic Cannella, Phllip Cano
// Description: Makes the tentacles disappear after going through its animation

using UnityEngine;
using System.Collections;

public class DestroyAfterAnimationTime : BossBody
{
    public AnimationClip animationClip;

    protected override void Start()
    {
        base.Start();
        if (animationClip != null)
        {
            StartCoroutine(SelfDestructAfterAnimation(animationClip.length));
        }
        else
        {
            Debug.LogWarning("Animation Clip not assigned to DestroyAfterAnimationTime script on " + gameObject.name);
        }
    }

    IEnumerator SelfDestructAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}