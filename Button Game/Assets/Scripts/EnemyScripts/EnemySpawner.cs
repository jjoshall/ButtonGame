using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float timer;
    public float spawnTimer = 2f;
    [SerializeField] private float spawnRadius = 250f;

    [SerializeField] private GameObject enemyToSpawn;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= spawnTimer) {
            SpawnBasicEnemy();
            timer = 0f;
        }
    }

    private void SpawnBasicEnemy() {
        if (PlayerMovement.Instance == null) {
            Debug.Log("PlayerMovement instance is null, cannot spawn enemy.");
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = PlayerMovement.Instance.transform.position + (Vector3)randomOffset;

        GameObject enemy = ObjectPoolManager.SpawnObject(enemyToSpawn, spawnPos, Quaternion.identity);
    }
}
