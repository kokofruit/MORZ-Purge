using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadDeathScreen()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
