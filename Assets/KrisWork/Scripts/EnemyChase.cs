using UnityEngine;

public class EnemyChase : EnemyController
{
    private int playerDistance = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position  (playerDistance + 10))
        {
            _enemyState = EnemyState.chasing;
        }
    }
}
