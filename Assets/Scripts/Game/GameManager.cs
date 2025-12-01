// Main Contributor: Gabriel Heiser
// Secondary Contributor: 
// Reviewer: 
// Description: Input manager for the main player object

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
            DeleteSaveFile();
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
        //track the lives left and level
        playerState.playerLives = _playerLives;
        playerState.currentLevel = _currentLevel;
        //Save the inventory
        playerState.inventory = InventoryManager.instance.GetInventory();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream afile = File.Create(Application.persistentDataPath + "/player.save");

        bf.Serialize(afile, playerState);
        afile.Close();
    }

    private void LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            //set lives, level, and difficulty if there is save data
            //open file
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();

            SaveState playerData = (SaveState)bf.Deserialize(afile);

            //close file
            afile.Close();

            //set lives and level
            _playerLives = playerData.playerLives;
            _currentLevel = playerData.currentLevel;
        }

        else
        {
            //set lives, level, and difficulty if there is no save data
            _playerLives = _startingLives;
            _currentLevel = _startingLevel;
            _difficulty = _startingDifficulty;
        }
    }

    void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            //set inventory to saved data
            //open file
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();

            SaveState playerData = (SaveState)bf.Deserialize(afile);
            
            //close file
            afile.Close();

            InventoryManager.instance.SetInventory(playerData.inventory);
        }

        else
        {
            //create new inventory if no save data
            InventoryManager.instance.StartNewInventory();
        }
    }

    void DeleteSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            //delete save if exists
            File.Delete(Application.persistentDataPath + "/player.save");
        }
        else Debug.Log("No current save files.");
    }
    #endregion

    /************************************** Game Management Methods ***********************************/
    #region Game Management Methods
    
    public void StartNewGame()
    {
        DeleteSaveFile();
        LoadGameData();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void LoadGame()
    {
        LoadGameData();
        Scene_Manager.instance.LoadLevel(_currentLevel);
    }

    public void StartLevel()
    {
        Debug.Log("sahdkasjhd");
        // spawn pickups in the level; pass in level index but convert 1-3 to 0-2 for list index reasons
        PickupSpawnerManager.instance?.SpawnPickups(_currentLevel - 1);
        Time.timeScale = 1;
        LoadPlayerData();
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
        //increments to the next level
        if (_currentLevel < 2)
            _currentLevel++;
        //saves information
        Save();
        //loads next level
        Scene_Manager.instance.LoadNextLevel();
    }

    public void PlayerDied()
    {
        _playerLives--;

        Debug.Log("Player Lives: " + _playerLives);

        //Lose game
        if (_playerLives <= 0) {
            DeleteSaveFile();
            Scene_Manager.instance.LoadLoseScreen();
        }
        //continue
        else {
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
        //release mouse
        if (state) Cursor.lockState = CursorLockMode.None;
        //lock mouse to screen
        else Cursor.lockState = CursorLockMode.Locked;

        // Play pause menu music when paused
        if (state) SoundManager.instance.PlayMenuMusic();
        // Return to the last song playing before the pause menu music played
        else SoundManager.instance.ReturnToLastSong();

        //set time to move
        Time.timeScale = state ? 0 : 1;
        //turn player input on or off
        PlayerController.instance.GetComponent<PlayerInput>().enabled = !state;
    }
}
#endregion
