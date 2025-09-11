using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f; // Maximum health of the enemy
    private float currentHealth; // Current health of the enemy
    
    private void Start() {
        currentHealth = maxHealth;
    }
}
