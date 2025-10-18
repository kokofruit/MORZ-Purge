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
    private int _playerLives;
    private int _currentLevel;
    private Inventory savedInventory;

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
        // TODO Load();
        _playerLives = 3;
        _currentLevel = 1;
    }

    public void StartLevel()
    {
        PickupSpawnerManager.instance.SpawnPickups();
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        if (savedInventory == null)
            Inventory_Manager.instance.StartNewInventory();
        else
            Inventory_Manager.instance.SetInventory(savedInventory);
    }

    public void PlayerDied()
    {
        _playerLives--;
        if (_playerLives <= 0)
        {
            savedInventory = null;
            // TODO Clear out player's data file
        }

        Scene_Manager.instance.LoadDeathScreen();
    }

    public void SaveInventory()
    {
        savedInventory = Inventory_Manager.instance.GetInventory();
    }

    public void RestartLevel()
    {
        Scene_Manager.instance.RestartScene();
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }


    
    // void Load() {
    //     if (File.Exists(Application.persistentDataPath + "/player.save")) {

    //         BinaryFormatter bf = new BinaryFormatter();

    //         FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);

    //         SaveState playerData = (SaveState)bf.Deserialize(afile);

    //         afile.Close();

    //         if (playerData.inventory != null) {
    //             inventory = playerData.inventory;
    //         }

    //         Check the players loaded inventory and adjust the dynamic room values.
    //         CheckInventory();

    //         Room room = NavigationManager.instance.GetRoomFromName(playerData.currentRoom);
    //         if (room != null) {
    //             NavigationManager.instance.SwitchRooms(room);
    //         }
    //     }
    //     else {
    //         NavigationManager.instance.ResetGame();
    //     }
    // }

    // public void Save() {
    //     SaveState playerState = new SaveState();
    //     playerState.currentRoom = NavigationManager.instance.currentRoom.name;
    //     playerState.inventory = inventory;

    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream afile = File.Create(Application.persistentDataPath + "/player.save");
    //     Debug.Log(Application.persistentDataPath);

    //     bf.Serialize(afile, playerState);
    //     afile.Close();
    // }

    // Hiii moth addition here. This will generate and set a random seed for the game.
    private void SetRandomSeed()
    {
        // Generate a random value from 0 to 255
        int randomSeed = Random.Range(0, 256);
        // Set random seed for the "run"
        Random.InitState(randomSeed);
    }
}
