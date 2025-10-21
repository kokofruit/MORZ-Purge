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
        DifficultyButtonController[] diffButtons = transform.parent.GetComponentsInChildren<DifficultyButtonController>();

        foreach (DifficultyButtonController diffButton in diffButtons)
        {
            if (GameManager.instance.GetStartingDifficulty() == (int)diffButton.diffVal)
                diffButton.GetComponent<Button>().interactable = false;
            
            else diffButton.GetComponent<Button>().interactable = true;
        }
    }
}
