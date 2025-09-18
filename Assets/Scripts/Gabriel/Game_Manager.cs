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
    private int playerLives;

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
        // Load();
        playerLives = 3;
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

    public void PlayerDied()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            // TODO Clear out player's data file
        }
        else
            Scene_Manager.instance.LoadDeathScene();
    }

    public void RestartLevel()
    {
        Scene_Manager.instance.RestartScene();
    }
}
