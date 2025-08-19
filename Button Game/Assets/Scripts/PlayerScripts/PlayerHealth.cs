using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Maximum health of the player
    [SerializeField] private Image healthBar; // Reference to the UI health bar image
    //[SerializeField] private float currentHealth; // Current health of the player

    public void TakeDamage(float damage) {
        if (damage <= 0)
            return;

        healthBar.fillAmount -= damage / maxHealth;

        if (healthBar.fillAmount <= 0f) {
            healthBar.fillAmount = 0f; // Ensure health does not go below zero
            // Optionally, handle player death here
            Debug.Log("Player has died.");
        }
    }

    public void Heal(float amount) {
        if (amount <= 0)
            return;

        healthBar.fillAmount += amount / maxHealth;
        if (healthBar.fillAmount > 1f) {
            healthBar.fillAmount = 1f; // Ensure health does not exceed maximum
        }
    }
}
