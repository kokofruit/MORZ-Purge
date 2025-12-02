using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTemplate", menuName = "Scriptable Objects/DialogueTemplate")]
public class DialogueTemplate : ScriptableObject
{
    public string dialogueText;
    public float displayDuration;
    public AudioClip voiceLine;
}
