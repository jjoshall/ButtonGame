using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 3f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private float timer;
    private bool isFading;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        startColor = spriteRenderer.color;
        timer = 0f;
        isFading = true;
        spriteRenderer.color = startColor; // Reset color when enabled
    }

    private void Update() {
        if (!isFading) return;

        timer += Time.deltaTime;
        float t = timer / fadeDuration;
        float alpha = Mathf.Lerp(1f, 0f, t);

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= fadeDuration) {
            isFading = false;
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
