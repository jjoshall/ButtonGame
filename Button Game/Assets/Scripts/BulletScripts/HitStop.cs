using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void DoHitStop(float duration) {
        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration) {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f; // Stop time
        yield return new WaitForSecondsRealtime(duration); // Wait for the specified duration
        Time.timeScale = originalTimeScale; // Resume time
    }
}
