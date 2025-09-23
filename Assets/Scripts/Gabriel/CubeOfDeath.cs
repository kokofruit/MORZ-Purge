using UnityEngine;

public class CubeOfDeath : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game_Manager.instance.PlayerDied();
        }
    }
}
