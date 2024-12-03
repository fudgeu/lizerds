using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHP { get; private set; }  // Current health, starts at 0% (no damage)
    
    // Track the last attacker who hit this player
    public Transform lastAttacker;
    
    public delegate void PlayerHealthChanged(float newHP);
    public event PlayerHealthChanged OnHealthChanged;
    
    private void Start()
    {
        CurrentHP = 0;  // Start with no damage
    }

    // Method to add damage and track the last attacker
    public void AddDamage(float damage, Transform attacker)
    {
        CurrentHP += damage;
        Debug.Log($"Player {name} HP: {CurrentHP}%"); // Log player health whenever damage is added
        
        // Record the attacker
        lastAttacker = attacker;
        
        OnHealthChanged?.Invoke(CurrentHP);
    }
}
