// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    void Start()
    {
        if (clearDataOnStart)
        {
            ClearSaveFile();
        }
    }

    public void LoadGame()
    {
        GameLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void StartNewGame()
    {
        ClearSaveFile();
        GameLoad();
        Scene_Manager.instance.LoadLevel(_currentLevel);
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

            InventoryManager.instance.SetInventory(playerData.inventory);

            HUDController.instance.DisplayWeaponAmmo(WeaponActionController.instance.currentWeapon.ammo);
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

        Debug.Log(CheckForSaveFile());
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
}
