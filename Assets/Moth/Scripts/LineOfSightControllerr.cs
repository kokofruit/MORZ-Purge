// Moth Harper
// Adding line of sight

using System.Collections;
using UnityEngine;

public class LineOfSightControllerr : EnemyController
{
    // the eye level for the enemy to "see" from
    [SerializeField] Transform _eyeLevel;
    // tracks whether the enemy can currently see the player
    [SerializeField] bool _lineOfSight;
    // tracks whether the enemy is currently alive
    [SerializeField] bool _alive = true;
    // how often the enemy checks if it can see the player
    [SerializeField, Min(0.001f)] float _sightCheckingInterval;

    protected override void Start()
    {
        base.Start();
        // start routinely checking for line of sight
        StartCoroutine(LineOfSight(_eyeLevel.position, Camera.main.transform.position));
    }

    protected override void DoIdle()
    {
        base.DoIdle();

        // if the player is visible, set the state to chasing
        if (_lineOfSight)
        {
            _enemyState = EnemyState.chasing;
        }
    }

    IEnumerator LineOfSight(Vector3 start, Vector3 end)
    {
        while (_alive)
        {
            // find the direction to the target
            Vector3 _direction = end - start;
            // find the distance to the target
            float _distance = _direction.magnitude;

            // set lineOfSight to false by default
            _lineOfSight = false;

            // raycast towards the target
            if (Physics.Raycast(start, _direction, out RaycastHit _hit, _distance + 1f))
            {
                // debug ray
                if (DEBUG_MODE) Debug.DrawRay(start, _direction);

                // if raycast hits something, see if it's the player
                if (_hit.collider.CompareTag("MainCamera")){
                    // if it's the player, set the lineOfSight boolean to true
                    _lineOfSight = true;
                }
            }

            // Set to false and wait to repeat
            if (DEBUG_MODE) print(gameObject.name + ": Line of Sight: " + _lineOfSight);
            yield return new WaitForSeconds(_sightCheckingInterval);
        }
    }
}
