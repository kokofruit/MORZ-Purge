using System.Collections;
using UnityEngine;

public class CubeOfWinning : MonoBehaviour
{
    // variables
    private DialogueManager DialogueManager;
    private bool available = false;
    private int index;

    private void Start()
    {
        DialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    public void MakeAvailable()
    {
        available = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && available)
        {
            GameManager.instance?.GoToNextLevel();
        }
        else
            DialogueManager.OnDisplay(8);
    }
}
