using System.Collections;
using TMPro;
using UnityEngine;

public class UpgradeTextFade : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float showDuration = 1.0f; // how long text stays fully visible
    [SerializeField] private float fadeDuration = 0.5f; // how long it takes to fade out
    [SerializeField] private float popScale = 1.2f;     // how big it scales up before settling
    [SerializeField] private float moveUpAmount = 30f;  // how far upward it drifts (UI units)

    private Vector3 startScale;
    private Vector3 startPos;

    private void OnEnable() {
        if (text == null) text = GetComponent<TMP_Text>();
        startScale = transform.localScale;
        startPos = transform.localPosition;

        StartCoroutine(PopAndFade());
    }

    private IEnumerator PopAndFade() {
        // Reset values
        transform.localScale = startScale * 0.8f;
        text.alpha = 0f;

        // ---- Pop In ----
        float t = 0f;
        while (t < 0.15f) {
            t += Time.deltaTime;
            float p = t / 0.15f;
            transform.localScale = Vector3.Lerp(startScale * 0.8f, startScale * popScale, p);
            text.alpha = p; // fade in
            yield return null;
        }
        transform.localScale = startScale;

        // ---- Hold ----
        yield return new WaitForSeconds(showDuration);

        // ---- Fade + Move Up ----
        t = 0f;
        Vector3 endPos = startPos + Vector3.up * moveUpAmount;
        while (t < fadeDuration) {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            text.alpha = Mathf.Lerp(1f, 0f, p);
            transform.localPosition = Vector3.Lerp(startPos, endPos, p);
            yield return null;
        }

        // Reset & hide for pooling
        transform.localScale = startScale;
        transform.localPosition = startPos;
        gameObject.SetActive(false);
    }

}
