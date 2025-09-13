using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float timer;

    [Header("Enemy Spawner Settings")]
    [SerializeField] private float spawnTimer = 2f;
    [SerializeField] private float spawnRadius = 250f;
    [SerializeField] private float safeRadius = 5f;

    [Header("Batch Settings")]
    [SerializeField] private int baseEnemiesPerSpawn = 1;
    [SerializeField] private int levelsPerIncrease = 3;
    [SerializeField] private int maxEnemiesPerSpawn = 5;

    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private GameObject[] enemiesToSpawn;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= spawnTimer) {
            SpawnBasicEnemy();
            //SpawnEnemyBatch();
            timer = 0f;
        }
    }

    private void SpawnEnemyBatch() {
        if (PlayerMovement.Instance == null) {
            Debug.Log("PlayerMovement instance is null, cannot spawn enemy.");
            return;
        }

        // Get current player level from XP system
        int playerLevel = XP.Instance != null ? XP.Instance.GetLevel() : 1;

        // Calculate how many enemies to spawn this batch
        int enemiesPerSpawn = baseEnemiesPerSpawn + (playerLevel / levelsPerIncrease);
        enemiesPerSpawn = Mathf.Min(enemiesPerSpawn, maxEnemiesPerSpawn);

        for (int i = 0; i < enemiesPerSpawn; i++) {
            Vector3 spawnPos;
            int attempts = 0;
            do {
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                spawnPos = PlayerMovement.Instance.transform.position + (Vector3)randomOffset;
                attempts++;
            }
            while (Vector3.Distance(spawnPos, PlayerMovement.Instance.transform.position) < safeRadius && attempts < 20);

            ObjectPoolManager.SpawnObject(enemyToSpawn, spawnPos, Quaternion.identity);
        }

        Debug.Log($"Spawned {enemiesPerSpawn} enemies at level {playerLevel}");
    }

    private void SpawnBasicEnemy() {
        if (PlayerMovement.Instance == null) {
            Debug.Log("PlayerMovement instance is null, cannot spawn enemy.");
            return;
        }

        Debug.Log("Spawning enemy...");
        Vector3 spawnPos;
        int attempts = 0;
        do {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPos = PlayerMovement.Instance.transform.position + (Vector3)randomOffset;
            attempts++;
        }
        while (Vector3.Distance(spawnPos, PlayerMovement.Instance.transform.position) < safeRadius && attempts < 20);

        ObjectPoolManager.SpawnObject(enemyToSpawn, spawnPos, Quaternion.identity);
        Debug.Log("Enemy spawned");
    }
}
