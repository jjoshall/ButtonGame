using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D component
    [SerializeField] private Transform gun;
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private RegBullet regBullet; // Reference to the RegBullet script for firing bullets
    [SerializeField] private ParticleSystem dashEffect; // Reference to the dash effect particle system

    [Header("Settings")]
    [SerializeField] private float slideForceAmt = 50f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float dragAmt = 5f;

    public static GameObject Instance;

    private void Awake() {
        Instance = gameObject;
    }

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

            PlayDashEffect(direction);
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

    private void PlayDashEffect(Vector3 direction) {
        if (dashEffect == null) {
            Debug.Log("Dash effect particle system is not assigned.");
            return;
        }

        Vector3 spawnPos = player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        ObjectPoolManager.SpawnObject(dashEffect, spawnPos, rot, ObjectPoolManager.PoolType.ParticleSystems);
    }
}
