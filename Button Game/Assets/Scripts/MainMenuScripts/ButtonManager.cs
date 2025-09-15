using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip hoverSound;

    public void PlayButton() {
        SoundEffectManager.Instance.PlaySoundFXClip(buttonSound, transform, 1f);

        SceneManager.LoadScene("SampleScene");
    }

    public void QuitButton() {
        SoundEffectManager.Instance.PlaySoundFXClip(buttonSound, transform, 1f);

        Application.Quit();
        Debug.Log("Quit pressed"); // useful for testing in the editor
    }

    public void PlayHoverSound() {
        SoundEffectManager.Instance.PlaySoundFXClip(hoverSound, transform, .5f);
    }
}
