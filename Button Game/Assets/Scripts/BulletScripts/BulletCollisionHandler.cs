using UnityEngine;

public class BulletCollisonHandler : MonoBehaviour
{
    private float lifetime = 2f;
    private float hitStopDuration = 0.3f;

    [SerializeField] private ParticleSystem enemyDeathEffect;
    [SerializeField] private GameObject[] enemyDeathSprites;

    private Collider2D _collider;
    private Rigidbody2D _rb;
    private bool _hasProcessedHit;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        _hasProcessedHit = false;
        if (_collider) _collider.enabled = true;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasProcessedHit || !collision.CompareTag("Enemy")) return;
        _hasProcessedHit = true;

        if (_collider) _collider.enabled = false;
        if (_rb) _rb.linearVelocity = Vector2.zero;

        HitStop.Instance.DoHitStop(hitStopDuration);

        // Add a random amount of xp between 10 and 30
        int randomXP = Random.Range(10, 31);
        XP.Instance.AddXP(randomXP);

        ObjectPoolManager.SpawnObject(
            enemyDeathEffect,
            collision.transform.position,
            Quaternion.identity,
            ObjectPoolManager.PoolType.ParticleSystems
        );

        if (enemyDeathSprites != null && enemyDeathSprites.Length > 0) {
            int randomIndex = Random.Range(0, enemyDeathSprites.Length);
            ObjectPoolManager.SpawnObject(
                enemyDeathSprites[randomIndex],
                collision.transform.position,
                Quaternion.identity,
                ObjectPoolManager.PoolType.ParticleSystems
            );
        }

        ObjectPoolManager.ReturnObjectToPool(collision.gameObject);
        ReturnToPool();
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
