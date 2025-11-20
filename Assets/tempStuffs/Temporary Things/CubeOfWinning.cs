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
            StartCoroutine(Timer());
        }
        else
            // BUG: REPEATING TEXT
            DialogueManager.OnDisplay(8);
    }

    IEnumerator Timer()
    {
        // Get the current scene for which index
        index = DialogueManager.GetEndCurrentScene();
        DialogueManager.OnDisplay(index);
        yield return new WaitForSeconds(10f);
        GameManager.instance?.GoToNextLevel();
    }
}
