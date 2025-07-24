using UnityEngine;

// This script will make the enemy follow the target.
public class BasicEnemyNav : MonoBehaviour
{
    private GameObject target;
    [SerializeField] private float moveSpeed = 2f;

    private void Start() {
        target = PlayerMovement.Instance;
    }

    private void Update() {
        Vector2 direction = (target.transform.position - this.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
