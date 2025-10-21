
using System.Collections.Generic;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    public static MenuInputController instance;
    public GameObject pauseMenu;
    private Stack<GameObject> stack;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        stack = new Stack<GameObject>();
    }

    public void SetActiveWinow(GameObject window)
    {
        window.SetActive(true);
        stack.Push(window);
    }

    public void ClearActiveWindow()
    {
        if (stack.Count > 0) {
            GameObject window = stack.Pop();
            window.SetActive(false);
        }
    }

    public void OnEscape()
    {
        if (PlayerController.instance == null) {
            if (stack.Count > 0) stack.Pop()?.SetActive(false);
        }

        else {
            if (!pauseMenu.activeInHierarchy)
            {
                GameManager.instance.PauseGame(true);
                instance.SetActiveWinow(pauseMenu);
            }
            else if (stack.Count > 0) ClearActiveWindow();

            if (stack.Count == 0) GameManager.instance.PauseGame(false);
        }
    }
}
