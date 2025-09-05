using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XP : MonoBehaviour
{
    public static XP Instance { get; private set; }

    [SerializeField] private Slider xpBar;
    [SerializeField] private int lvlNum = 1;
    [SerializeField] private TextMeshProUGUI lvlTxt;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    private void Update() {
        if (xpBar.value >= 100) {
            Debug.Log("Level up");
            lvlNum++;
            NextLevel();
        }
    }

    public void AddXP(int amount) {
        xpBar.value += amount;
    }

    private void NextLevel() {
        xpBar.value = 0;
        lvlTxt.text = "LVL " + lvlNum.ToString();
    }
}
