// Main Contributor: Moth Harper
// Secondary Contributor: Gabriel Heiser
// Based on my SoundManagers for previous projects
// Description: This script is called for playing sounds and then removing them

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance;

    // The prefab to use for playing audios
    [SerializeField] private AudioSource _soundPlayerPrefab;

    // The mixer group for sound effects
    [SerializeField] private AudioMixerGroup _fxMixer;
    // The mixer group for music
    [SerializeField] private AudioMixerGroup _musicMixer;
    
    // Music audio source on the player
    private AudioSource _musicSource;
    // Music Clips
    [SerializeField] private AudioClip _battleMusic;
    [SerializeField] private AudioClip _menuMusic;
    private AudioClip _pausedClip;
    private float _pausedClipTime;

    // Set the instance or destroy if it's a duplicate
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    /// <summary>
    /// Functions for managing music
    /// </summary>
    public void PlayMenuMusic()
    {
        _musicSource = PlayerController.instance.GetMusicSource();
        
        // Save the state of whatever music is currently playing
        _pausedClip = _musicSource.clip;
        _pausedClipTime = _musicSource.time;
        
        _musicSource.clip = _menuMusic;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void ReturnToLastSong()
    {
        _musicSource.clip = _pausedClip;
        _musicSource.time = _pausedClipTime;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    /// <summary>
    /// Plays a sound effect, optionally with a random pitch.
    /// </summary>
    /// <param name="clip">The audio to play.</param>
    /// <param name="parent">The transform to attach the audio to.</param>
    /// <param name="volume">The volume the audio will play at.</param>
    /// <param name="pitch">The value that the random pitch range will be centered to. Use 1 for default pitch.</param>
    /// <param name="pitchFluctuation">The maximum amount the pitch can differ (postively or negatively) from the base pitch</param>
    public void PlayFXAudio(AudioClip clip, Transform parent, float volume = 1f, float pitch = 1f, float pitchFluctuation = 0f)
    {
        // Create an object to play the audio
        AudioSource player = Instantiate(_soundPlayerPrefab, parent);

        // Set the attributes of the player
        player.clip = clip;
        player.volume = volume;
        player.outputAudioMixerGroup = _fxMixer;
        // set a random pitch
        float randomPitch = pitch + UnityEngine.Random.Range(-pitchFluctuation, pitchFluctuation);
        player.pitch = randomPitch;

        // Play the audio
        player.Play();

        // After the sound is done playing, destroy the player
        float clipLength = clip.length;
        Destroy(player.gameObject, clipLength);
    }
    
    /// <summary>
    /// Plays a sound effect, optionally with a random pitch.
    /// </summary>
    /// <param name="clip">The audio to play.</param>
    /// <param name="position">The position the audio will play at.</param>
    /// <param name="volume">The volume the audio will play at.</param>
    /// <param name="pitch">The value that the random pitch range will be centered to. Use 1 for default pitch.</param>
    /// <param name="pitchFluctuation">The maximum amount the pitch can differ (postively or negatively) from the base pitch</param>
    public void PlayFXAudio(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float pitchFluctuation = 0f)
    {
        // Create an object to play the audio
        AudioSource player = Instantiate(_soundPlayerPrefab, position, quaternion.identity);

        // Set the attributes of the player
        player.clip = clip;
        player.volume = volume;
        player.outputAudioMixerGroup = _fxMixer;
        // set a random pitch
        float randomPitch = pitch + UnityEngine.Random.Range(-pitchFluctuation, pitchFluctuation);
        player.pitch = randomPitch;

        // Play the audio
        player.Play();

        // After the sound is done playing, destroy the player
        float clipLength = clip.length;
        Destroy(player.gameObject, clipLength);
    }
}
