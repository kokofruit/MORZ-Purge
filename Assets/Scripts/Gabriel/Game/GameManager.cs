// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioMixer audioMixer;
    public bool clearDataOnStart = false;
    private int _playerLives;
    private int _currentLevel;
    private int _difficulty;
    [SerializeField]
    private int _startingLives = 3;
    [SerializeField]
    private int _startingLevel = 1;
    [SerializeField]
    private int _startingDifficulty = 1;


    /************************************** MonoBehavior Methods ***********************************/
    #region MonoBehavior Methods
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(instance);
    }

    void Start()
    {        
        if (clearDataOnStart)
        {
            ClearSaveFile();
        }
    }
    #endregion

    /************************************** Saving / Loading Methods ***********************************/
    #region Saving / Loading Methods
    public bool CheckForSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            return true;
        }
        return false;
    }

    public void Save()
    {
        SaveState playerState = new SaveState();
        playerState.playerLives = _playerLives;
        playerState.currentLevel = _currentLevel;

        playerState.inventory = InventoryManager.instance.GetInventory();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream afile = File.Create(Application.persistentDataPath + "/player.save");

        bf.Serialize(afile, playerState);
        afile.Close();
    }

    private void GameLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();

            SaveState playerData = (SaveState)bf.Deserialize(afile);

            afile.Close();

            _playerLives = playerData.playerLives;
            _currentLevel = playerData.currentLevel;

            InventoryManager.instance.SetInventory(playerData.inventory);
        }

        else
        {
            _playerLives = _startingLives;
            _currentLevel = _startingLevel;
            _difficulty = _startingDifficulty;
        }
    }

    void PlayerLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();

            SaveState playerData = (SaveState)bf.Deserialize(afile);

            afile.Close();

            InventoryManager.instance.SetInventory(playerData.inventory);

            HUDController.instance.LoadMagazineDisplay(WeaponActionController.instance.currentWeapon);
        }

        else
        {
            InventoryManager.instance.StartNewInventory();
        }
    }

    void ClearSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            File.Delete(Application.persistentDataPath + "/player.save");
        }
        else Debug.Log("No current save files.");
    }
    #endregion

    /************************************** Game Management Methods ***********************************/
    #region Game Management Methods
    public void SetAudioLevels(float MainVol, float MusicVol, float SFXVol)
    {
        audioMixer.SetFloat("volume", MainVol);
    }
    
    public void StartNewGame()
    {
        ClearSaveFile();
        GameLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void LoadGame()
    {
        GameLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void StartLevel()
    {
        PickupSpawnerManager.instance?.SpawnPickups();
        PlayerLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void RestartLevel()
    {
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void GoToNextLevel()
    {
        if (_currentLevel < 2)
            _currentLevel++;
        Save();
        Scene_Manager.instance.LoadNextLevel();
    }

    public void PlayerDied()
    {
        _playerLives--;

        Debug.Log(_playerLives);

        if (_playerLives <= 0)
        {
            ClearSaveFile();
            GameLoad();
            Scene_Manager.instance.LoadLoseScreen();
        }
        else
        {
            Scene_Manager.instance.LoadDeathScreen();
        }
    }

    public void SetStartingDifficulty(int diffVal)
    {
        _startingDifficulty = diffVal;
    }

    public int GetStartingDifficulty()
    {
        return _startingDifficulty;
    }

    public int GetDifficulty()
    {
        return _difficulty + 1;
    }

    public void PauseGame(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            PlayerController.instance.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerController.instance.GetComponent<PlayerInput>().enabled = true;
        }
    }
}
#endregion
