using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D component
    [SerializeField] private Transform gun;
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private RegBullet regBullet; // Reference to the RegBullet script for firing bullets

    [Header("Settings")]
    [SerializeField] private float slideForceAmt = 50f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float dragAmt = 5f;
    [SerializeField] private float bulletForce = 10f;

    private void FixedUpdate() {
        if (playerRb != null) {
            // Apply drag to the player's Rigidbody2D so it doesnt slide forever
            if (playerRb.linearVelocity.magnitude > maxSpeed) {
                playerRb.linearVelocity = playerRb.linearVelocity.normalized * maxSpeed;
            }

            playerRb.linearVelocity *= 1 - dragAmt * Time.fixedDeltaTime;
        }
    }

    public void Shoot(InputAction.CallbackContext context) {
        if (player == null) {
            Debug.LogError("Player transform is not assigned.");
            return;
        }
        if (playerRb == null) {
            Debug.LogError("Player Rigidbody2D is not assigned.");
            return;
        }
        if (gun == null) {
            Debug.LogError("Gun transform is not assigned.");
            return;
        }
        if (bulletPrefab == null) {
            Debug.LogError("Bullet prefab is not assigned.");
            return;
        }

        if (context.performed) {
            FacePlayerToMouseClick();
            MovePlayerAwayFromClick();

            Vector3 direction = GetDirectionOfMouseClick();
            regBullet.FireBullet(bulletPrefab, gun.position, direction);
            //FireBullet();
        }
    }

    private Vector3 GetDirectionOfMouseClick() {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        return (mouseWorldPosition - player.position).normalized;
    }

    private void FacePlayerToMouseClick() {
        Vector3 direction = GetDirectionOfMouseClick();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        player.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MovePlayerAwayFromClick() {
        Vector3 direction = GetDirectionOfMouseClick();
        Vector3 moveDirection = -direction.normalized;
        playerRb.AddForce(moveDirection * slideForceAmt, ForceMode2D.Impulse);
    }

    //private void FireBullet() {
    //    Vector3 direction = GetDirectionOfMouseClick();

    //    // GameObject bullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
    //    GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, gun.position, Quaternion.identity);
    //    Rigidbody2D bulletRbInstance = bullet.GetComponent<Rigidbody2D>();

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    bullet.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

    //    if (bulletRbInstance != null) {
    //        bulletRbInstance.linearVelocity = Vector2.zero;
    //        bulletRbInstance.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    //    }
    //    else {
    //        Debug.LogError("Instantiated bullet has no Rigidbody2D component.");
    //    }

    //    // Destroy(bullet, 2f); // Destroy the bullet after 2 seconds to prevent clutter
    //    StartCoroutine(ReturnToPoolAfterDelay(bullet, 2f));
    //}

    //private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject bullet, float delay) {
    //    yield return new WaitForSeconds(delay);
    //    ObjectPoolManager.ReturnObjectToPool(bullet);
    //}
}
