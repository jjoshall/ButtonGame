using System.Collections;
using TMPro;
using UnityEngine;

public class BulletCollisonHandler : MonoBehaviour
{
    private float lifetime = 2f;
    private float hitStopDuration = 3f;
    private int randomXP;

    // Penetrate upgrade
    [SerializeField] private int remainingPenetration;

    // Size Upgrade
    [SerializeField] private Vector3 currentSize;

    // Camera Shake
    [SerializeField] private float camShakeDuration = 0.3f;
    [SerializeField] private float camShakeMagnitude = 0.2f;

    // Audio
    [SerializeField] private AudioClip[] enemyKillSounds;

    // Particle Effects
    [SerializeField] private ParticleSystem enemyDeathEffect;
    [SerializeField] private GameObject[] enemyDeathSprites;
    [SerializeField] private GameObject xpNumber;
    [SerializeField] private TextMeshPro xpNumText;

    private Collider2D _collider;
    private Rigidbody2D _rb;
    private bool _hasProcessedHit;

    // References
    [SerializeField] private EnemyDrops enemyDrops;

    [SerializeField] private bool CritXP = false;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();

        CritXP = false;
    }

    private void OnEnable() {
        _hasProcessedHit = false;
        if (_collider) _collider.enabled = true;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);

        remainingPenetration = UpgradeManager.BulletPenetration;
        currentSize = UpgradeManager.BulletSize;
        transform.localScale = currentSize;
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasProcessedHit || !collision.CompareTag("Enemy")) return;
        _hasProcessedHit = true;

        BasicEnemy basicEnemy = collision.GetComponent<BasicEnemy>();
        if (basicEnemy == null) return;

        HitStop.Instance.DoHitStop(hitStopDuration);
        CameraShake.Instance.Shake(camShakeDuration, camShakeMagnitude);
        SoundEffectManager.Instance.PlayRandomSoundFXClip(enemyKillSounds, transform, 1f);

        Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
        basicEnemy.TakeHit(hitDirection);

        if (basicEnemy.ReturnHitsToKill() > 0) {         
            ObjectPoolManager.SpawnObject(
            enemyDeathEffect,
            collision.transform.position,
            Quaternion.identity,
            ObjectPoolManager.PoolType.ParticleSystems
            );

            if (remainingPenetration > 0) {
                remainingPenetration--;
                StartCoroutine(ResetProcessedFlagNextFrame());
            }
            else {
                if (_collider) _collider.enabled = false;
                if (_rb) _rb.linearVelocity = Vector2.zero;
                ReturnToPool();
            }
        }
        else {
            if (CritXP) {
                // Have a 10% chance to get a large amount of XP on kill
                int critChance = Random.Range(1, 11); // 1 to 10

                if (critChance == 1) {
                    randomXP = Random.Range(70, 101); // Between 50 and 100 XP
                    XP.Instance.AddXP(randomXP);
                }
                else {
                    randomXP = Random.Range(10, 31);
                    XP.Instance.AddXP(randomXP);
                }
            }
            else {
                randomXP = Random.Range(10, 31);
                XP.Instance.AddXP(randomXP);
            }

            // For XP number popup
            var xpObj = ObjectPoolManager.SpawnObject(
                xpNumber,
                collision.transform.position + Vector3.up * 0.5f,
                Quaternion.identity,
                ObjectPoolManager.PoolType.ParticleSystems
            );

            var xpText = xpObj.GetComponent<XPFadeaway>();
            if (xpText != null) {
                xpText.SetValue(randomXP);
            }

            // For the enemy death particles
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

            enemyDrops.DropHealthPackAt(collision.transform.position);

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject);

            if (remainingPenetration > 0) {
                remainingPenetration--;
                StartCoroutine(ResetProcessedFlagNextFrame());
            }
            else {
                if (_collider) _collider.enabled = false;
                if (_rb) _rb.linearVelocity = Vector2.zero;
                ReturnToPool();
            }
        }
    }

    private IEnumerator ResetProcessedFlagNextFrame() {
        yield return null; // Wait for the next frame
        _hasProcessedHit = false;
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

    public void EnableCritXPUpgrade() {
        CritXP = true;
        Debug.Log("CritXP on");
    }
}
