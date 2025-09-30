using UnityEngine;

public class CubeOfWinning : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Scene_Manager.instance?.LoadWinScreen();
        }
    }
}
