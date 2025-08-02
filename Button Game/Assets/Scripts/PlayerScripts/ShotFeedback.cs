using System.Collections;
using UnityEngine;

public class ShotFeedback : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player transform

    [Header("Recoil Settings")]
    [SerializeField] private float recoilScale = 1.2f; // Scale of the recoil effect
    [SerializeField] private float recoilDuration = 0.1f; // Duration of the recoil effect
    [SerializeField] private AnimationCurve recoilCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Curve for the recoil effect

    private Coroutine recoilCoroutine; // Reference to the recoil coroutine

    public void PlayRecoil() {
        if (recoilCoroutine != null) {
            StopCoroutine(recoilCoroutine); // Stop any existing recoil coroutine
        }

        recoilCoroutine = StartCoroutine(RecoilRoutine());
    }

    private IEnumerator RecoilRoutine() {
        Vector3 originalScale = player.localScale; // Store the original scale of the player
        float timer = 0f; // Timer for the recoil effect

        while (timer < recoilDuration) {
            timer += Time.deltaTime; // Increment the timer
            float t = timer / recoilDuration; // Calculate the normalized time

            float scaleAmount = Mathf.Lerp(1f, recoilScale, recoilCurve.Evaluate(t)); // Calculate the scale amount based on the curve
            player.localScale = originalScale * scaleAmount; // Apply the scale to the player

            yield return null; // Wait for the next frame
        }

        timer = 0f;

        while (timer < recoilDuration) {
            timer += Time.deltaTime; // Increment the timer
            float t = timer / recoilDuration; // Calculate the normalized time
            float scaleAmount = Mathf.Lerp(recoilScale, 1f, recoilCurve.Evaluate(t)); // Calculate the scale amount to return to original scale
            player.localScale = originalScale * scaleAmount; // Apply the scale to the player
            yield return null; // Wait for the next frame
        }

        player.localScale = originalScale; // Ensure the player scale is reset to original
    }
}
