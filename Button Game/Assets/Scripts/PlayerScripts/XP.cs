using UnityEngine;
using UnityEngine.UI;

public class XP : MonoBehaviour
{
    public static XP Instance { get; private set; }

    [SerializeField] private Slider xpBar;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void AddXP(int amount) {
        Debug.Log("Added " + amount + " XP");
        xpBar.value += amount;
    }
}
