using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float timer;
    private bool spawningPaused = false;
    private bool waveTriggered = false;

    [SerializeField] private TextMeshProUGUI swarmTxt;

    [Header("Enemy Spawner Settings")]
    [SerializeField] private float spawnTimer = 2f;
    [SerializeField] private float spawnRadius = 250f;
    [SerializeField] private float safeRadius = 5f;
    [SerializeField] private int maxActiveEnemies = 25;

    [Header("Batch Settings")]
    [SerializeField] private int baseEnemiesPerSpawn = 1;
    [SerializeField] private int levelsPerIncrease = 3;
    [SerializeField] private int maxEnemiesPerSpawn = 8;

    [Header("Enemy Types")]
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject fastEnemy;
    [SerializeField] private GameObject heavyEnemy;
    //[SerializeField] private GameObject enemyToSpawn;

    [SerializeField] private AudioClip backgroundMusic; // Background music clip

    public static readonly HashSet<GameObject> ActiveEnemies = new HashSet<GameObject>();

    private void Start() {
        SoundEffectManager.Instance.PlayLoopingMusic(backgroundMusic, transform, 0.5f);
    }

    private void Update() {
        if (spawningPaused) return;

        timer += Time.deltaTime;

        if (timer >= spawnTimer) {
            // SpawnBasicEnemy();
            SpawnEnemyBatch();
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

        if (ActiveEnemies.Count >= maxActiveEnemies) return;

        // Calculate how many enemies to spawn this batch
        int enemiesPerSpawn = baseEnemiesPerSpawn + (playerLevel / levelsPerIncrease);
        enemiesPerSpawn = Mathf.Min(enemiesPerSpawn, maxEnemiesPerSpawn);

        bool heavyWave = playerLevel % 5 == 0 && playerLevel > 0;

        if (heavyWave && !waveTriggered) {
            waveTriggered = true;
            StartCoroutine(HandleHeavyWave(playerLevel));
            return;
        }
        else if (!heavyWave) {
            waveTriggered = false;
        }

        for (int i = 0; i < enemiesPerSpawn; i++) {
            if (ActiveEnemies.Count >= maxActiveEnemies) break;

            GameObject enemyToSpawn = ChooseEnemyType(playerLevel, heavyWave);

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
    }

    private IEnumerator HandleHeavyWave(int playerLevel) {
        spawningPaused = true;

        // Clear all active enemies
        foreach (var enemy in new List<GameObject>(ActiveEnemies)) {
            ObjectPoolManager.ReturnObjectToPool(enemy);
        }
        ActiveEnemies.Clear();

        swarmTxt.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);

        spawningPaused = false;
    }

    private GameObject ChooseEnemyType(int level, bool heavyWave) {
        // Default probabilities
        float normalChance = 0.8f;
        float fastChance = 0.2f;
        float heavyChance = 0f;

        // Fast enemies unlock at level 3
        if (level < 3) {
            fastChance = 0f;
            normalChance = 1f;
        }

        // Heavy waves every 5 levels
        if (heavyWave) {
            // Fewer fasts, introduce heavies
            normalChance = 0.6f;
            fastChance = 0.1f;
            heavyChance = 0.3f;

            // Scale heavies & fasts as player gets higher
            int waveCount = level / 5; // 1st wave = level 5, 2nd wave = level 10, etc.
            heavyChance += 0.05f * waveCount; // add more heavies
            fastChance += 0.05f * waveCount;  // add more fasts
            float total = normalChance + fastChance + heavyChance;

            // Normalize back to 1
            normalChance /= total;
            fastChance /= total;
            heavyChance /= total;
        }

        float roll = Random.value;

        if (roll < normalChance) return normalEnemy;
        if (roll < normalChance + fastChance) return fastEnemy;
        return heavyEnemy;
    }

    //private void SpawnBasicEnemy() {
    //    if (PlayerMovement.Instance == null) {
    //        Debug.Log("PlayerMovement instance is null, cannot spawn enemy.");
    //        return;
    //    }

    //    Debug.Log("Spawning enemy...");
    //    Vector3 spawnPos;
    //    int attempts = 0;
    //    do {
    //        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
    //        spawnPos = PlayerMovement.Instance.transform.position + (Vector3)randomOffset;
    //        attempts++;
    //    }
    //    while (Vector3.Distance(spawnPos, PlayerMovement.Instance.transform.position) < safeRadius && attempts < 20);

    //    ObjectPoolManager.SpawnObject(enemyToSpawn, spawnPos, Quaternion.identity);
    //    Debug.Log("Enemy spawned");
    //}
}
