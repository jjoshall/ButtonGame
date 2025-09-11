using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] upgrades;
    [SerializeField] private Transform leftSlot;
    [SerializeField] private Transform rightSlot;
    private GameObject leftUpgrade;
    private GameObject rightUpgrade;

    private void OnEnable() {
        // Pause the game
        Time.timeScale = 0f;

        // Pick 2 unique random upgrades
        int leftIndex = Random.Range(0, upgrades.Length);
        int rightIndex;
        do {
            rightIndex = Random.Range(0, upgrades.Length);
        } while (rightIndex == leftIndex);

        // Assign chosen upgrades
        leftUpgrade = upgrades[leftIndex];
        rightUpgrade = upgrades[rightIndex];

        // Position them at the slot transforms
        leftUpgrade.transform.position = leftSlot.position;
        rightUpgrade.transform.position = rightSlot.position;

        // Enable only these two
        foreach (var upgrade in upgrades) upgrade.SetActive(false);

        leftUpgrade.SetActive(true);
        rightUpgrade.SetActive(true);
    }

    private void OnDisable() {
        Time.timeScale = 1f;

        if (leftUpgrade != null) leftUpgrade.SetActive(false);
        if (rightUpgrade != null) rightUpgrade.SetActive(false);

        leftUpgrade = null;
        rightUpgrade = null;
    }
}
