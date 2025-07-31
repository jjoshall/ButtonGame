using UnityEngine;

public class ParticlePooling : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    private void OnEnable() {
        if (ps == null) {
            Debug.LogError("Particle System is not assigned in the ParticlePooling script.");
        }

        ps.Play();
        Invoke(nameof(ReturnToPool), ps.main.duration + ps.main.startLifetime.constantMax);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.ParticleSystems);
    }
}
