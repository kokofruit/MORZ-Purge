using UnityEngine;
using UnityEngine.UI;

public class LoadTitleScreenButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() {
        MenuInputController.instance.ClearActiveWindow();
        Scene_Manager.instance.LoadTitleScreen();
    }
}
