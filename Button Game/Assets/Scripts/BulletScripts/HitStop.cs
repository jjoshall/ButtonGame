using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private Coroutine currentCoroutine;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this; // no DontDestroyOnLoad
    }

    private void OnDestroy() {
        if (Instance == this) Instance = null;
    }

    public void DoHitStop(float duration) {
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
            Time.timeScale = 1f; // Ensure time is reset before starting a new hit stop
        }

        currentCoroutine = StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration) {
        Time.timeScale = 0f; // Stop time
        yield return new WaitForSecondsRealtime(duration); // Wait for the specified duration
        Time.timeScale = 1f; // Resume time
        currentCoroutine = null;
    }
}
