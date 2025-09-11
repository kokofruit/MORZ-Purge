using UnityEngine;

public class LineOfSightControllerr : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LineOfSight(this.transform.position, Camera.main.transform.position);
    }

    bool LineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 _direction = end - start;

        //if (Physics.Raycast(start, _direction, ))
        //{
        //    return true;
        //}

        Debug.DrawRay(start, _direction);

        return false;
    }
}
