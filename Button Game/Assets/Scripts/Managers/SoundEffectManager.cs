using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource musicSource; // dedicated music player

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this; // no DontDestroyOnLoad
    }

    private void OnDestroy() {
        if (Instance == this) Instance = null;
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        // spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audioClip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy clip after its done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) {
        // assign random index
        int randomIndex = Random.Range(0, audioClip.Length);

        // spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // assign the audioClip
        audioSource.clip = audioClip[randomIndex];

        // assign volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy clip after its done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayMusic(AudioClip musicClip, float volume = 0.5f) {
        if (musicSource == null) {
            Debug.LogError("Music Source is not assigned in SoundEffectManager.");
            return;
        }

        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    // Optional: Stop music
    public void StopMusic() {
        if (musicSource != null && musicSource.isPlaying) {
            musicSource.Stop();
        }
    }
}
