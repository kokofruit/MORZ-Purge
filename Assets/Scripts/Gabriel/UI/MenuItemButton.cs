
using UnityEngine;
using UnityEngine.UI;

public class MenuItemButton : MonoBehaviour
{
    public GameObject menuItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        if (!menuItem.activeInHierarchy) MenuInputController.instance.SetActiveWinow(menuItem);
        else MenuInputController.instance.OnEscape();
    }
}
