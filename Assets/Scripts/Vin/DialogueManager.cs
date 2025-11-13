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
    public List<DialogueTemplate> dialogChoices;
    public TMP_Text dialogBox;

    // private variables
    private DialogueTemplate dialogText;
    private int index;

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

    void Awake()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Set the dialogue index to be displayed based off of what the current scene is
        // CHANGE SCENE NAMES IN FINAL BUILD
        if (currentScene.name == "Kris Level 1")
            index = 0;
        else if (currentScene.name == "Kris Level 2")
            index = 2;
        else if (currentScene.name == "Kris Level 3")
            index = 4;
        else if (currentScene.name == "Boss")
            index = 6;

        // On scene load, display start of level dialogue
        OnDisplay(index);
    }

    // Send the dialogue option to the coroutine to be displayed
    public void OnDisplay(int index)
    {
        dialogText = dialogChoices[index];
        StartCoroutine(TypeDialog(dialogText.dialogueText));
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


}
