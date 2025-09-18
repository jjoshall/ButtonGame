using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XP : MonoBehaviour
{
    public static XP Instance { get; private set; }

    [SerializeField] private Slider xpBar;
    [SerializeField] private int lvlNum = 1;
    [SerializeField] private TextMeshProUGUI lvlTxt;
    [SerializeField] private ParticleSystem lvlUpEffect;
    [SerializeField] private Transform playerPos;
    [SerializeField] private float camShakeDuration = 0.3f;
    [SerializeField] private float camShakeMagnitude = 0.2f;
    [SerializeField] private AudioClip lvlUpSound;


    [SerializeField] private int baseXPRequired = 100;
    [SerializeField] private float xpGrowthRate = 1.10f;
    private int currentXP = 0;
    private int xpRequired;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start() {
        xpRequired = baseXPRequired;
        xpBar.maxValue = xpRequired;
        xpBar.value = 0;
        lvlTxt.text = "LVL " + lvlNum.ToString();
    }

    public void AddXP(int amount) {
        currentXP += amount;
        xpBar.value = currentXP;

        if (currentXP >= xpRequired) {
            NextLevel();
        }
    }

    private void NextLevel() {
        lvlNum++;

        // Carry over excess XP to the next level
        currentXP -= xpRequired;

        // Scale the XP required for the next level
        xpRequired = Mathf.RoundToInt(xpRequired * xpGrowthRate);

        xpBar.maxValue = xpRequired;
        xpBar.value = currentXP;
        lvlTxt.text = "LVL " + lvlNum.ToString();

        SoundEffectManager.Instance.PlaySoundFXClip(lvlUpSound, playerPos, 1f);

        ObjectPoolManager.SpawnObject(
            lvlUpEffect,
            playerPos.transform.position,
            Quaternion.identity,
            ObjectPoolManager.PoolType.ParticleSystems
        );

        CameraShake.Instance.Shake(camShakeDuration, camShakeMagnitude);

        UpgradeManager.Instance.GrantUpgrade(lvlNum, playerPos.gameObject);
    }

    public int GetLevel() {
        return lvlNum;
    }
}
