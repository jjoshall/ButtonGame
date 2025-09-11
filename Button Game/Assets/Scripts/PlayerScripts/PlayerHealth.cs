using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Maximum health of the player
    private float currentHealth; // Current health of the player
    [SerializeField] private Image healthBar; // Reference to the UI health bar image
    [SerializeField] private TextMeshProUGUI deathText; // Reference to the death text UI element
    [SerializeField] private Button restartButton; // Reference to the restart button UI element
    [SerializeField] private AudioClip[] hurtSounds; // Array of hurt sound effects
    [SerializeField] private AudioClip deathSound; // Death sound effect

    private void Start() {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage) {
        if (damage <= 0)
            return;

        SoundEffectManager.Instance.PlayRandomSoundFXClip(hurtSounds, transform, 1f);

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        //healthBar.fillAmount -= damage / maxHealth;

        if (currentHealth <= 0f) {
            currentHealth = 0f; // Ensure health does not go below zero

            SoundEffectManager.Instance.PlaySoundFXClip(deathSound, transform, 1f);

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

        currentHealth += amount / maxHealth;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth; // Ensure health does not exceed maximum
        }
    }

    private void UpdateHealthUI() {
        if (healthBar != null) {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void UpgradeMaxHealth(int upgradeAmount) {
        maxHealth += upgradeAmount;
        currentHealth += upgradeAmount;
        UpdateHealthUI();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Max Health: " + maxHealth.ToString() + " Current Health: " + currentHealth.ToString());
        }
    }
}
