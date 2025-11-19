using UnityEngine;
using UnityEngine.UI;

public class CloseApplicationButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() {
        Application.Quit();
    }
}
