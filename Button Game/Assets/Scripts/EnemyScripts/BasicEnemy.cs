using System;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Movement")]
    private GameObject target;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float damage = 10f;

    private void Start() {
        target = PlayerMovement.Instance;
    }

    // Enemy movement
    private void Update() {
        Vector2 direction = (target.transform.position - this.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damage); // Adjust damage as needed

                ObjectPoolManager.ReturnObjectToPool(gameObject); // Return enemy to pool after hitting player
            }
        }
    }
}
