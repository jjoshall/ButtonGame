using UnityEngine;

public class BulletCollisonHandler : MonoBehaviour
{
    private float lifetime = 2f;
    private float hitStopDuration = 0.1f;

    [SerializeField] private ParticleSystem enemyDeathEffect;
    [SerializeField] private GameObject[] enemyDeathSprites;

    private void OnEnable() {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            HitStop.Instance.DoHitStop(hitStopDuration);

            // Spawning enemy death effect
            ObjectPoolManager.SpawnObject(enemyDeathEffect, collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

            // Random blood splatter sprite
            if (enemyDeathSprites.Length > 0) {
                int randomIndex = Random.Range(0, enemyDeathSprites.Length);

                ObjectPoolManager.SpawnObject(enemyDeathSprites[randomIndex], collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);
            }

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
