using UnityEngine;

public class BulletCollisonHandler : MonoBehaviour
{
    private float lifetime = 2f;
    private float hitStopDuration = 0.05f;

    private void OnEnable() {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            Debug.Log("Hit stop duration: " + hitStopDuration);
            HitStop.Instance.DoHitStop(hitStopDuration);

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public void SetLifetime(float time) {
        lifetime = time;
    }

    public void SetHitStopDuration(float duration) {
        hitStopDuration = duration;
    }
}
