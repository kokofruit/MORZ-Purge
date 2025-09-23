// Main Contributor: Moth Harper
// Based on my SoundManagers for previous projects
// Description: This script is called for playing sounds and then removing them

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance;

    [SerializeField] private AudioSource _soundPlayerPrefab;
    [SerializeField] private AudioMixer _mixer;

    // List of clips currently playing
    private List<AudioClip> _playingClips = new();

    // Set the instance or destroy if it's a duplicate
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Plays an audioclip, optionally with a random pitch.
    /// </summary>
    /// <param name="clip">The audio to play.</param>
    /// <param name="parent">The transform to attach the audio to.</param>
    /// <param name="volume">The volume the audio will play at.</param>
    /// <param name="pitch">The value that the random pitch range will be centered to. Use 1 for default pitch.</param>
    /// <param name="pitchFluctuation">The maximum amount the pitch can differ (postively or negatively) from the base pitch</param>
    public void PlayAudio(AudioClip clip, Transform parent, float volume = 1f, float pitch = 1f, float pitchFluctuation = 0f)
    {
        // Create an object to play the audio
        AudioSource player = Instantiate(_soundPlayerPrefab, parent);

        // Set the attributes of the player
        player.clip = clip;
        player.volume = volume;
        float randomPitch = pitch + Random.Range(-pitchFluctuation, pitchFluctuation);
        player.pitch = randomPitch;

        // Play the audio
        player.Play();

        // After the sound is done playing, destroy the player
        float clipLength = clip.length;
        Destroy(player.gameObject, clipLength);
    }

    public void PlayRepeating(AudioClip clip, Transform parent, float volume = 1f, float pitch = 1f, float pitchFluctuation = 0.2f)
    {
        if (_playingClips.Contains(clip))
        {
            return;
        }

        _playingClips.Add(clip);
        
    }
}
