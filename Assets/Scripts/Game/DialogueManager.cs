
// Main Contributors: Vin + Kris
// Reviewer: 
// Description: Controls story dialogue that appears on screen

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    // public references
    public static DialogueManager instance;
    public List<DialogueTemplate> dialogChoices;
    public List<UpgradeTemplate> upgradeChoices;
    public TMP_Text dialogBox;
    public GameObject storyBackground;
    // BUG COUNTER
    public int bugDeathCount = 0;
    // Cap for bugs needed to progress level
    public int bugDeathCap;

    // private variables
    private DialogueTemplate dialogText;
    private UpgradeTemplate upgradeText;
    private int index;
    private HUDController HUDController;
    private CubeOfWinning CubeOfWinning;
    private bool boxCleared = true;
    private int currScene;
    
    // Indexes for dialogChoices
    /*
     * 0 - Level 1 Start
     * 1 - Level 1 End
     * 2 - Level 2 Start
     * 3 - Level 2 End
     * 4 - Level 3 Start
     * 5 - Level 3 End
     * 6 - Boss Start
     * 7 - Boss End
     * 8 - Out of Bounds
     * 9 - Beginning Story
     */

    /** Indexes for upgrades
     * 0 - AP Rounds
     * 1 - Big Ammo
     * 2 - Heay Recoil Reducer
     * 3 - Light Extended Mag
     * 4 - Light Mag Grip
     * 5 - LMG Improved Mechanics
     * 6 - Load Assist
     * 7 - Medium Extended Mag
     * 8 - Medium Mag Grip
     * 9 - Pistol Hollow Point Rounds
     * 10 - Powerful Rockets
     * 11 - Shotgun Slug Shells
     * 12 - SMG Hollow Point Rounds
     * 13 - SMG Improved Mechanics
     * 14 -  Voltage Amp
     */

    void Awake()
    {
        // Used for UpgradePickup text display.
        instance = this;

        // On scene load, display start of level dialogue
        currScene = GetStartCurrentScene();
        if (currScene != 9)
        {
            OnDisplay(currScene);
        }

        // Get HUDController
        HUDController = FindAnyObjectByType<HUDController>();

        // Get CubeOfWinning
        CubeOfWinning = FindAnyObjectByType<CubeOfWinning>();

        // Reset bug death tracker each time scene loads
        HUDController.SetBugDeathCount(bugDeathCount, bugDeathCap);
    }

    // Send the dialogue option to the coroutine to be displayed
    public void OnDisplay(int index)
    {
        dialogText = dialogChoices[index];
        // makes sure text box is cleared instead of overwriting dialogue
        if (boxCleared)
        {
            if (dialogText.voiceLine != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = dialogText.voiceLine;
                audioSource.Play();
            }
            StartCoroutine(TypeDialog(dialogText.dialogueText));
        }
    }

    // Sends the picked up upgrade description to coroutine to be displayed.
    public void UpgradeDisplay(int index)
    {
        upgradeText = upgradeChoices[index];
        StartCoroutine(TypeDialog(upgradeText.upgradeInfoText));
    }

    // Gets the scene and displays start dialogue
    public int GetStartCurrentScene()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Set the dialogue index to be displayed based off of what the current scene is
        if (currentScene.name == "Level 1")
        {
            // storyBackground.SetActive(false);
            index = 0;
            bugDeathCap = 50;   // bugDeathCap starts lower at level 1
        }
        else if (currentScene.name == "Level 2")
        {
            index = 2;
            bugDeathCap = 60;   // bugDeathCap starts 10 higher at level 2
        }
        else if (currentScene.name == "Level 3")
        {
            index = 4;
            bugDeathCap = 70;   // bugDeathCap starts 10 higher at level 3
        }
        else if (currentScene.name == "Level 4")
        {
            index = 6;
            bugDeathCap = 80;   // bugDeathCap starts 10 higher at Boss level
        }
        else if (currentScene.name == "TitleScreen")
        {
            index = 9;
        }
            return index;
    }

    // Gets the scene and displays end dialogue
    public int GetEndCurrentScene()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Set the dialogue index to be displayed based off of what the current scene is
        if (currentScene.name == "Level 1")
            index = 1;
        else if (currentScene.name == "Level 2")
            index = 3;
        else if (currentScene.name == "Level 3")
            index = 5;
        else if (currentScene.name == "Level 4")
            index = 7;
        return index;
    }

    // Coroutine to type out given dialogue
    IEnumerator TypeDialog(string message)
    {
        boxCleared = false;
        // Slight delay when player loads in scene cuz it looks nice
        yield return new WaitForSeconds(.5f);
        foreach (char c in message)
        {
            // Append the character to the message
            dialogBox.text += c;
            // Wait for a randomized short moment before appending the next character
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }
        // Wait to reset the dialogue box
        yield return new WaitForSeconds(2f);
        // Reset the dialogue box
        dialogBox.text = "";
        boxCleared = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.CompareTag("Player"))
       {
           index = 8;
           OnDisplay(index);
       }
    }

    public void SetBugDeathCounter()
    {
        bugDeathCount++;
        HUDController.SetBugDeathCount(bugDeathCount, bugDeathCap);
        // If bug deaths hit cap, and it's not the boss level bc bug deaths on boss level do not count
        if (bugDeathCount == bugDeathCap && currScene != 6)
        {
            // ALLOW PLAYER TO ACCESS NEXT LEVEL
            CubeOfWinning.MakeAvailable();
            // Get current scene end level dialogue
            index = GetEndCurrentScene();
            OnDisplay(index);
        }
    }

    // When boss dies, display this text
    public void BossDied()
    {
        index = 7;
        OnDisplay(index);
    }

    // Display story after new game is pressed
    public void DisplayStory()
    {
        storyBackground.SetActive(true);
        OnDisplay(index);
    }
}
