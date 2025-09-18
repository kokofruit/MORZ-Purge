using UnityEngine;

// Main Contributor: Vin
// Secondary Contributor: 
// Reviewer: 
// Description: Controller to show health

public class HealthController : MonoBehaviour
{
    //Variables
    public float Health, MaxHealth, Width, Height;

    //Health bar
    [SerializeField]
    private RectTransform healthBar;

    //Setting max health
    public void SetMaxHealth(float maxHealth)  
    {
        MaxHealth = maxHealth;
    }

    //Setting health
    public void SetHealth(float health)
    {
        Health = health;
    }
}
