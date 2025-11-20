using UnityEngine;
using UnityEngine.UI;

public class RetryLevel : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() {
        GameManager.instance.RestartLevel();
    }
}
