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

public class DialogueManager : MonoBehaviour
{
    // public references
    public List<DialogueTemplate> dialogChoices;
    public TMP_Text dialogBox;

    // private variables
    private DialogueTemplate dialogText;
    private int index;

    // Starting dialogues need displayed on scene loads
    // Ending dialogues need displayed when all bugs are dead
    // Wrong way dialogue can be triggered and displayed
    // On Death Maybe ??

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
        if (currentScene.name == "Kris Level 1")
            index = 0;
        else if (currentScene.name == "Kris Level 2")
            index = 2;
        else if (currentScene.name == "Kris Level 3")
            index = 4;
        else if (currentScene.name == "Boss")
            index = 6;

        // On scene load, start dialogue
        OnDisplay(index);
    }

    // Coroutine to type out given dialogue
    IEnumerator TypeDialog(string message)
    {
        yield return new WaitForSeconds(.5f);
        foreach (char c in message)
        {
            // Append the character to the message
            dialogBox.text += c;
            // Wait for a randomized short moment before appending the next character
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
        }

        // Wait to reset the dialogue box
        yield return new WaitForSeconds(3f);
        // Reset the dialogue box
        dialogBox.text = "";
    }

    // Send the dialogue option to the coroutine to be displayed
    private void OnDisplay(int index)
    {
        dialogText = dialogChoices[index];
        StartCoroutine(TypeDialog(dialogText.dialogueText));
    }
}
