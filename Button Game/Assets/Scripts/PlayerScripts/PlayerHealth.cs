using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Maximum health of the player
    [SerializeField] private Image healthBar; // Reference to the UI health bar image
    [SerializeField] private TextMeshProUGUI deathText; // Reference to the death text UI element
    [SerializeField] private Button restartButton; // Reference to the restart button UI element

    public void TakeDamage(float damage) {
        if (damage <= 0)
            return;

        healthBar.fillAmount -= damage / maxHealth;

        if (healthBar.fillAmount <= 0f) {
            healthBar.fillAmount = 0f; // Ensure health does not go below zero

            // Optionally, handle player death here
            Time.timeScale = 0f; // Pause the game
            deathText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

            restartButton.onClick.AddListener(() => {
                Time.timeScale = 1f; // Resume the game
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
            });
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
