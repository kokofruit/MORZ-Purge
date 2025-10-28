using UnityEngine;

public class CubeOfDeath : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerDied();
        }
    }
}
