using UnityEngine;

/*script to move health bar*/

public class HealthController : MonoBehaviour
{
    //variables
    public float Health, MaxHealth, Width, Height;

    //health bar to adjust
    [SerializeField]
    private RectTransform healthBar;

    //setting max health
    private void SetMaxHealth(float maxHealth)  
    {
        MaxHealth = maxHealth;
    }

    //setting health
    private void SetHealth(float health)
    {
        Health = health;
    }
}
