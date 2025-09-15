using UnityEngine;
using TMPro;
using System.Collections;

public class TitleMovement : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI sTxt;
    [SerializeField] private TextMeshProUGUI shapeTxt;
    [SerializeField] private TextMeshProUGUI shooterTxt;

    [SerializeField] private float bounceHeight = 20f;   // How high the bounce goes
    [SerializeField] private float bounceDuration = 0.25f;
    [SerializeField] private float delayBetweenLetters = 0.05f;
    [SerializeField] private float delayBetweenLoops = 1.0f; // Pause between full cycles

    private void Start() {
        StartCoroutine(LoopBounce(sTxt, 0f));
        StartCoroutine(LoopBounce(shapeTxt, 0.5f));
        StartCoroutine(LoopBounce(shooterTxt, 1.0f));
    }

    private IEnumerator LoopBounce(TextMeshProUGUI tmp, float startDelay) {
        yield return new WaitForSeconds(startDelay);

        while (true) {
            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++) {
                if (!textInfo.characterInfo[i].isVisible) continue;

                StartCoroutine(BounceLetter(tmp, i));
                yield return new WaitForSeconds(delayBetweenLetters);
            }

            yield return new WaitForSeconds(delayBetweenLoops);
        }
    }

    private IEnumerator BounceLetter(TextMeshProUGUI tmp, int charIndex) {
        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;
        var charInfo = textInfo.characterInfo[charIndex];
        int vertexIndex = charInfo.vertexIndex;
        int materialIndex = charInfo.materialReferenceIndex;

        Vector3[] verts = textInfo.meshInfo[materialIndex].vertices;
        Vector3 offset = Vector3.up * bounceHeight;

        float elapsed = 0f;
        while (elapsed < bounceDuration) {
            float t = elapsed / bounceDuration;
            float ease = Mathf.Sin(t * Mathf.PI); // smooth bounce curve

            // Reset vertices to baseline each frame
            tmp.ForceMeshUpdate();
            verts = tmp.textInfo.meshInfo[materialIndex].vertices;

            Vector3 displacement = offset * ease;
            verts[vertexIndex + 0] += displacement;
            verts[vertexIndex + 1] += displacement;
            verts[vertexIndex + 2] += displacement;
            verts[vertexIndex + 3] += displacement;

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
