using UnityEngine;

public class MedkitFade : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float totalLifetime = 5f;
    [SerializeField] private float blinkStartTime = 2f;   // after this many seconds, start blinking
    [SerializeField] private float baseBlinkSpeed = 0.5f;   // initial blink frequency
    [SerializeField] private float maxBlinkSpeed = 2f;   // fastest blink frequency

    private float spawnTime;
    private Material _material;
    private Color _baseColor;

    private void OnEnable() {
        if (_renderer == null) {
            Debug.LogError("Medkit renderer not assigned");
            return;
        }

        spawnTime = Time.time;
        Invoke(nameof(ReturnToPool), totalLifetime);

        // get material reference
        _material = _renderer.material;
        _baseColor = _material.color;
        SetAlpha(1f);
    }

    private void Update() {
        if (_material == null) return;

        float elapsed = Time.time - spawnTime;

        if (elapsed > blinkStartTime) {
            float blinkElapsed = elapsed - blinkStartTime;
            float blinkDuration = totalLifetime - blinkStartTime;

            // how far through the blinking period we are
            float t = Mathf.Clamp01(blinkElapsed / blinkDuration);

            // frequency ramps from slow to fast
            float currentSpeed = Mathf.Lerp(baseBlinkSpeed, maxBlinkSpeed, t);

            // use blinkElapsed so sine wave starts at phase 0
            float alpha = (Mathf.Sin(blinkElapsed * currentSpeed * Mathf.PI * 2f) + 1f) * 0.5f;

            // bias toward visible
            alpha = Mathf.Pow(alpha, 0.5f);

            SetAlpha(alpha);
        }
    }

    private void SetAlpha(float alpha) {
        Color c = _baseColor;
        c.a = alpha;
        _material.color = c;
    }

    private void OnDisable() {
        CancelInvoke();
        if (_material != null)
            SetAlpha(1f); // reset fully visible
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.GameObjects);
    }
}
