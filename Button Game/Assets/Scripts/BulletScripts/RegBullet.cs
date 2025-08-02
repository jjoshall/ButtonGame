using UnityEngine;

public class RegBullet : MonoBehaviour
{
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float bulletLifetime = 2f;
    [SerializeField] private float hitStopDuration = 0.05f;    

    public void FireBullet(GameObject bulletPrefab, Vector3 gunPosition, Vector3 shootDirection) {
        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, gunPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        if (bulletRb != null) {
            bulletRb.linearVelocity = Vector2.zero;
            bulletRb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);
        }
        else {
            Debug.LogError("Instantiated bullet has no Rigidbody2D component.");
        }

        BulletCollisonHandler handler = bullet.GetComponent<BulletCollisonHandler>();
        if (handler != null) {
            handler.SetLifetime(bulletLifetime);
            handler.SetHitStopDuration(hitStopDuration);
        }
        else {
            Debug.LogError("Instantiated bullet has no BulletCollisonHandler component.");
        }
    }
}
