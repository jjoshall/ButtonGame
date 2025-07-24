using UnityEngine;

public class BulletCollisonHandler : MonoBehaviour
{
    private float lifetime = 2f;

    private void OnEnable() {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
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
}
