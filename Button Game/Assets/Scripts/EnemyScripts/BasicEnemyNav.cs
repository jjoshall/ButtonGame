using UnityEngine;

public class BasicEnemyNav : MonoBehaviour
{
    public Transform target; // The target to follow
    public float speed = 2f; // Speed of the enemy

    [SerializeField] private Rigidbody2D enemy;

    private void FixedUpdate() {
        if (enemy == null) {
            Debug.Log("Rigidbody not assigned for enemy movement");
        }

        Vector2 direction = (target.position - transform.position).normalized;
        enemy.MovePosition(enemy.position + direction * speed * Time.fixedDeltaTime);
    }
}
