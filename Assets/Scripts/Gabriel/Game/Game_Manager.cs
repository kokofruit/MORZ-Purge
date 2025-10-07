// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public bool clearDataOnStart = false;
    private int _playerLives;
    private int _currentLevel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(instance);
    }

    public void Start()
    {
        // FIX THIS LATER
        // ALSO NEED TO FIX SPRITE PROBLEM BECUASE SPRITES CANNOT NATIVELY BE SAVED
        if (clearDataOnStart)
        {
            ClearSaveFile();
        }

        GameLoad();
    }

    public void StartLevel()
    {
        PickupSpawnerManager.instance?.SpawnPickups();
        PlayerLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
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

    public void Save()
    {
        SaveState playerState = new SaveState();
        playerState.playerLives = _playerLives;
        playerState.currentLevel = _currentLevel;

        playerState.inventory = Inventory_Manager.instance.GetInventory();

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

            Inventory_Manager.instance.SetInventory(playerData.inventory);
        }

        else
        {
            _playerLives = 3;
            _currentLevel = 1;
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

            Inventory_Manager.instance.SetInventory(playerData.inventory);
        }

        else
        {
            Inventory_Manager.instance.StartNewInventory();
        }
    }

    void ClearSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            File.Delete(Application.persistentDataPath + "/player.save");
        }
    }

    public void RestartLevel()
    {
        Scene_Manager.instance.RestartScene();
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
}
