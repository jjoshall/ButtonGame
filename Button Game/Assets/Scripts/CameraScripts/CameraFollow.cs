using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private float smoothSpeed = 0.125f; // Speed of camera smoothing
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Offset from the player position

    private void LateUpdate() {
        if (player == null) {
            Debug.LogError("Player transform is not assigned.");
            return;
        }
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;
        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
