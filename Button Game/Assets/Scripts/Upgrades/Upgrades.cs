using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject upgradeScreen;

    public void ReturnToGame() {
        upgradeScreen.SetActive(false);
    }

    public void HealthUpgradeButton() {
        playerHealth.UpgradeMaxHealth(25);
    }
}
