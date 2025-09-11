using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D component
    [SerializeField] private Transform gun;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private RegBullet regBullet; // Reference to the RegBullet script for firing bullets
    [SerializeField] private ParticleSystem dashEffect; // Reference to the dash effect particle system
    [SerializeField] private ShotFeedback shotFeedback; // Reference to the shot feedback script
    [SerializeField] private AudioClip[] shootSounds; // Array of shooting sound effects
    [SerializeField] private Image[] uiBullets;

    [Header("Dash Settings")]
    [SerializeField] private float slideForceAmt = 50f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float dragAmt = 5f;

    [Header("Gun Settings")]
    [SerializeField] private int bulletAmountMax = 3;
    [SerializeField] private float reloadTime = 1f;
    private int currentBulletAmount;
    private bool isReloading = false;

    public static GameObject Instance;

    private void Awake() {
        Instance = gameObject;

        currentBulletAmount = bulletAmountMax;

        foreach (var img in uiBullets) {
            img.fillAmount = 1f;
        }
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

    private void Update() {
        FaceGunToMouse();
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
            if (currentBulletAmount <= 0) {
                Debug.Log("Out of bullets!");

                // play empty clip sound

                return;
            }

            UseBullet();
            currentBulletAmount--;
            Debug.Log("Bullets left: " + currentBulletAmount.ToString());
            ReloadBullets();

            SoundEffectManager.Instance.PlayRandomSoundFXClip(shootSounds, player, 1f);

            MovePlayerAwayFromClick();

            Vector3 direction = GetDirectionOfMouseClick();
            regBullet.FireBullet(bulletPrefab, gun.position, direction);

            PlayDashEffect(direction);
            shotFeedback.PlayRecoil();
        }
    }

    private Vector3 GetDirectionOfMouseClick() {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        return (mouseWorldPosition - player.position).normalized;
    }

    private void FaceGunToMouse() {
        Vector3 direction = GetDirectionOfMouseClick();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);
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

    private void ReloadBullets() {
        if (!isReloading) {
            StartCoroutine(ReloadCoroutine());
        }

        //if (currentBulletAmount < bulletAmountMax) {
        //    Debug.Log("Starting reload coroutine");
        //    StartCoroutine(ReloadCoroutine());
        //}
    }

    private IEnumerator ReloadCoroutine() {
        isReloading = true;

        while (currentBulletAmount < bulletAmountMax) {
            int index = currentBulletAmount;
            float elapsed = 0f;

            while (elapsed < reloadTime) {
                elapsed += Time.deltaTime;
                uiBullets[index].fillAmount = Mathf.Clamp01(elapsed / reloadTime);
                yield return null;
            }

            uiBullets[index].fillAmount = 1f;
            currentBulletAmount++;

            // Play reload sound effect

            Debug.Log("Reloaded 1 bullet. Current bullets: " + currentBulletAmount);
        }

        isReloading = false;
    }

    private void UseBullet() {
        if (currentBulletAmount > 0) {
            int index = currentBulletAmount - 1;
            uiBullets[index].fillAmount = 0f;
        }
    }
}