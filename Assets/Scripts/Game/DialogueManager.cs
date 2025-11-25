
// Main Contributors: Vin + Kris
// Reviewer: 
// Description: Controls story dialogue that appears on screen

using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;
using UnityEngine.SceneManagement;

/*
 * TODO:
 * End level dialogue when player kills all (or a certain number) of aliens
 * Wrong way dialogue (waiting for boundary implementation)
 */

public class DialogueManager : MonoBehaviour
{
    // public references
    public static DialogueManager instance;
    public List<DialogueTemplate> dialogChoices;
    public List<UpgradeTemplate> upgradeChoices;
    public TMP_Text dialogBox;
    // BUG COUNTER
    public int bugDeathCount = 0;
    // Cap for bugs needed to progress level
    public int bugDeathCap = 50;

    // private variables
    private DialogueTemplate dialogText;
    private UpgradeTemplate upgradeText;
    private int index;
    private HUDController HUDController;
    private CubeOfWinning CubeOfWinning;

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
        // On scene load, display start of level dialogue
        OnDisplay(GetStartCurrentScene());

        // Get HUDController
        HUDController = FindAnyObjectByType<HUDController>();

        // Get CubeOfWinning
        CubeOfWinning = FindAnyObjectByType<CubeOfWinning>();
    }

    // Send the dialogue option to the coroutine to be displayed
    public void OnDisplay(int index)
    {
        dialogText = dialogChoices[index];
        StartCoroutine(TypeDialog(dialogText.dialogueText));
    }

    public void UpgradeDisplay(int index)
    {
        upgradeText = upgradeChoices[index];
        StartCoroutine(TypeDialog(upgradeText.upgradeInfoText));
    }

    public int GetStartCurrentScene()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Set the dialogue index to be displayed based off of what the current scene is
        // CHANGE SCENE NAMES IN FINAL BUILD
        if (currentScene.name == "Level 1")// change level names to which level the main branch calls them
            index = 0;
        else if (currentScene.name == "Level 2")// change level names to which level the main branch calls them
            index = 2;
        else if (currentScene.name == "Level 3")// change level names to which level the main branch calls them
            index = 4;
        else if (currentScene.name == "Boss")// change level names to which level the main branch calls them
            index = 6;
        return index;
    }

    public int GetEndCurrentScene()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Set the dialogue index to be displayed based off of what the current scene is
        // CHANGE SCENE NAMES IN FINAL BUILD
        if (currentScene.name == "Level 1")// change level names to which level the main branch calls them
            index = 1;
        else if (currentScene.name == "Level 2")// change level names to which level the main branch calls them
            index = 3;
        else if (currentScene.name == "Level 3")// change level names to which level the main branch calls them
            index = 5;
        else if (currentScene.name == "Boss")// change level names to which level the main branch calls them
            index = 7;
        return index;
    }

    // Coroutine to type out given dialogue
    IEnumerator TypeDialog(string message)
    {
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
        HUDController.SetBugDeathCount(bugDeathCount);
        // If bug deaths hit cap
        if (bugDeathCount >= bugDeathCap)
        {
            // ALLOW PLAYER TO ACCESS NEXT LEVEL
            CubeOfWinning.MakeAvailable();
        }
    }
}
