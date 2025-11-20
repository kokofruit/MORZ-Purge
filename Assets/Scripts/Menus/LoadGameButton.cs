using UnityEngine;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance.CheckForSaveFile())
            gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
        else
            gameObject.GetComponent<Button>().interactable = false;
    }

    void ButtonPressed() {
        GameManager.instance.LoadGame();
    }
}
