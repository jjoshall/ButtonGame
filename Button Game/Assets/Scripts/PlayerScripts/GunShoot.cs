using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D component
    
    [SerializeField] private float slideForceAmt = 50f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float dragAmt = 5f;

    private void FixedUpdate() {
        if (playerRb != null) {
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

        if (context.performed) {
            FacePlayerToMouseClick();
            MovePlayerAwayFromClick();
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
}
