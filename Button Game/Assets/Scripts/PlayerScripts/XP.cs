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

    private void Update() {
        if (xpBar.value >= 100) {
            lvlNum++;
            NextLevel();
        }
    }

    public void AddXP(int amount) {
        xpBar.value += amount;
    }

    private void NextLevel() {
        SoundEffectManager.Instance.PlaySoundFXClip(lvlUpSound, playerPos, 1f);

        ObjectPoolManager.SpawnObject(
            lvlUpEffect,
            playerPos.transform.position,
            Quaternion.identity,
            ObjectPoolManager.PoolType.ParticleSystems
        );

        xpBar.value = 0;
        lvlTxt.text = "LVL " + lvlNum.ToString();

        CameraShake.Instance.Shake(camShakeDuration, camShakeMagnitude);
    }
}
