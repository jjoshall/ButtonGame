using System;
using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Movement")]
    private GameObject target;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Zig Zag Movement")]
    [SerializeField] private bool useZigZag = false;
    [SerializeField] private float zigZagAmplitude = 1f;   // side-to-side distance
    [SerializeField] private float zigZagFrequency = 5f;   // how fast to zig-zag
    private float zigzagTimer;

    [Header("Enemy Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private int hitsToKill = 1;
    private int currentHits;

    [Header("Hit Feedback")]
    [SerializeField] private float hitScale = 1.2f;       // how big to scale up
    [SerializeField] private float scaleDuration = 0.1f;  // how fast to return
    [SerializeField] private float flashDuration = 0.1f;  // how long to flash white
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Knockback")]
    [SerializeField] private float knockbackStrength = 2f;
    [SerializeField] private float knockbackTime = 0.1f;

    [SerializeField] private Vector3 originalScale;
    [SerializeField] private Color originalColor;
    private bool isKnockedBack = false;

    private void OnEnable() {
        currentHits = hitsToKill;

        transform.localScale = originalScale;
        if (spriteRenderer != null) {
            spriteRenderer.color = originalColor;
        }

        target = PlayerMovement.Instance;
        isKnockedBack = false;
        zigzagTimer = 0f;
    }

    // Enemy movement
    private void Update() {
        if (isKnockedBack || target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        Vector2 moveDir = direction;

        if (useZigZag) {
            Vector2 perp = new Vector2(-direction.y, direction.x); // Perpendicular vector
            float offset = Mathf.Sin(zigzagTimer * zigZagFrequency) * zigZagAmplitude;
            moveDir = (direction + perp * offset).normalized;
            zigzagTimer += Time.deltaTime;
        }

        // Move and rotate
        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damage); // Adjust damage as needed

                ObjectPoolManager.ReturnObjectToPool(gameObject); // Return enemy to pool after hitting player
            }
        }
    }

    public void TakeHit(Vector2 hitDirection) {
        // Start feedback effects
        StopAllCoroutines();
        StartCoroutine(HitFeedback());
        StartCoroutine(ApplyKnockback(hitDirection));

        currentHits--;
    }

    private IEnumerator HitFeedback() {
        // Flash white
        if (spriteRenderer != null) {
            spriteRenderer.color = Color.white;
        }

        // Scale up
        transform.localScale = originalScale * hitScale;

        // Wait
        yield return new WaitForSeconds(flashDuration);

        // Restore color
        if (spriteRenderer != null) {
            spriteRenderer.color = originalColor;
        }

        // Smoothly scale back down
        float t = 0f;
        while (t < scaleDuration) {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t / scaleDuration);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    private IEnumerator ApplyKnockback(Vector2 hitDirection) {
        isKnockedBack = true;

        Vector3 startPos = transform.position;
        Vector3 knockbackTarget = startPos + (Vector3)(hitDirection.normalized * knockbackStrength);

        float elapsed = 0f;
        while (elapsed < knockbackTime) {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, knockbackTarget, elapsed / knockbackTime);
            yield return null;
        }

        isKnockedBack = false;
    }

    public int ReturnHitsToKill() {
        return currentHits;
    }
}
