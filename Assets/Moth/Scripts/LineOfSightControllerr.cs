using UnityEngine;

public class LineOfSightControllerr : EnemyController
{
    [SerializeField] Transform _eyeLevel;

    protected override void DoIdle()
    {
        base.DoIdle();
        if (LineOfSight(_eyeLevel.position, Camera.main.transform.position, out RaycastHit _hitPoint))
        {
            print(_hitPoint.collider.gameObject.CompareTag("Player"));
        }
    }

    bool LineOfSight(Vector3 start, Vector3 end, out RaycastHit hitPoint)
    {
        // find the direction to the target
        Vector3 _direction = end - start;
        // find the distance to the target
        float _distance = _direction.magnitude;

        // raycast towards the target
        if (Physics.Raycast(start, _direction, out RaycastHit _hit, _distance + 1f))
        {
            // if successful, return true and output a hitpoint
            Debug.DrawRay(start, _direction);
            hitPoint = _hit;
            return true;
        }

        // if not successful, output an empty raycasthit and return false
        hitPoint = new RaycastHit();
        return false;
    }
}
