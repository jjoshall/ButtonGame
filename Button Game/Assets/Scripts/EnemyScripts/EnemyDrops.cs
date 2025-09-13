using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] private GameObject healthPackPrefab;
    [SerializeField] private bool canDropHealthPack = false;

    private void Awake() {
        canDropHealthPack = false;
    }

    public void EnableHealthPackDrop() {
        canDropHealthPack = true;
        Debug.Log("Health Pack Drop Enabled");
    }

    public void DropHealthPackAt(Vector3 worldPos) {
        if (canDropHealthPack && healthPackPrefab != null) {
            int dropChance = Random.Range(1, 101); // 1 to 100

            if (dropChance <= 20) { // 20% chance
                ObjectPoolManager.SpawnObject(
                healthPackPrefab,
                worldPos,
                Quaternion.identity,
                ObjectPoolManager.PoolType.GameObjects
                );
            }
        }
    }
}
