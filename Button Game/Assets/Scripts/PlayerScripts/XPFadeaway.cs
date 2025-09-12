using TMPro;
using UnityEngine;

public class XPFadeaway : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float flashSpeed = 5f;

    private void OnEnable() {
        if (_text == null) {
            Debug.LogError("Text is not assigned in the XPFadeaway script.");
            return;
        }

        Invoke(nameof(ReturnToPool), 2f);
    }

    private void Update() {
        if (_text == null) {
            return;
        }

        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f; // Normalize to 0-1
        Color c = _text.color;
        c.a = alpha;
        _text.color = c;
    }

    public void SetValue(int value) {
        if (_text == null) {
            Debug.LogError("Text is not assigned in the XPFadeaway script.");
            return;
        }
        _text.text = $"+{value}";
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void ReturnToPool() {
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.ParticleSystems);
    }
}
