using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonController : MonoBehaviour
{
    public enum Difficulty {easy, medium, hard}
    public Difficulty diffVal;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);

        CheckDifficulty();
    }

    public void OnButtonPressed()
    {
        GameManager.instance.SetStartingDifficulty((int)diffVal);
        CheckDifficulty();
    }

    private void CheckDifficulty()
    {
        DifficultyButtonController[] siblings = transform.parent.GetComponentsInChildren<DifficultyButtonController>();

        foreach (DifficultyButtonController b in siblings)
        {
            if (GameManager.instance.GetStartingDifficulty() == (int)b.diffVal)
                button.interactable = false;

            else button.interactable = true;
        }

    }
}
