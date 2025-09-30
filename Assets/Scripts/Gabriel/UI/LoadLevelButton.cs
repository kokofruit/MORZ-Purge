using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed() {
        Scene_Manager.instance.LoadLevel(Game_Manager.instance.GetCurrentLevel());
    }
}
