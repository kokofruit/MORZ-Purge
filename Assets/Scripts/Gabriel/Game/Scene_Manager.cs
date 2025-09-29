// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Controls the player's movement between scenes

using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    // Static reference to the scene manager
    public static Scene_Manager instance;

    // Scene manager singleton
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(instance);
    }

    // Load the death screen when the player dies
    public void LoadDeathScreen()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    // Load the win screen when the player reaches the end of the game
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }

    // Load the title screen when the game starts or the player returns to the title screen
    public void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    // Load one of the main gameplay levels
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    // Restart the current scene the player is in
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
