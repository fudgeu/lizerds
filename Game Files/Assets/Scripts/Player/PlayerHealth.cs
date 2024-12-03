using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHP { get; private set; }  // Current health, starts at 0% (no damage)
    
    public delegate void PlayerHealthChanged(float newHP);
    public event PlayerHealthChanged OnHealthChanged;
    
    private void Start()
    {
        CurrentHP = 0;  // Start with no damage
    }

    public void AddDamage(float damage)
    {
        CurrentHP += damage;
        Debug.Log($"Player {name} HP: {CurrentHP}%"); // Log player health whenever damage is added
        OnHealthChanged?.Invoke(CurrentHP);
    }
}
