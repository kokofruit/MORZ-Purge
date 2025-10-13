using UnityEngine;
using UnityEngine.UI;

public class StartNewGameButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() {
        GameManager.instance.StartNewGame();
    }
}
