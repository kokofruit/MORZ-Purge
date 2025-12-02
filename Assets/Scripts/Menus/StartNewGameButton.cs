using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartNewGameButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() 
    {
        StartCoroutine(Timer());
    }

    // Leave time to display story
    IEnumerator Timer()
    {
        DialogueManager.instance.DisplayStory();
        yield return new WaitForSeconds(18f);
        MenuInputController.instance.ClearActiveWindow();
        GameManager.instance.StartNewGame();
    }
}
