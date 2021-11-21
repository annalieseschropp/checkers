using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

/// <summary>
/// Class
/// Component for playing audio sources that respect global volume settings.
/// </summary>
public class ControlledAudioSource : MonoBehaviour
{
    public bool isMusic;
    public bool isLooping;
    public float volumeMultiplier = 1.0f;
    public AudioClip sound;
    private AudioSource audioSource;

    /// <summary>
    /// Method
    /// Initializer performed before any script attempts to access the PieceSet.
    /// </summary>
    void Awake()
    {
        this.UpdateAudioSource();
    }

    /// <summary>
    /// Method
    /// Plays the sound respecting the level in the options menu.
    /// </summary>
    public void Play()
    {
        this.UpdateAudioSource();
        if(this.audioSource.volume > 0.0f)
        {
            this.audioSource.Play();
        }
    }

    /// <summary>
    /// Method
    /// Stop the currently playing sound.
    /// </summary>
    public void Stop()
    {
        if(this.audioSource.isPlaying)
        {
            this.audioSource.Stop();
        }
    }

    /// <summary>
    /// Method
    /// Plays the sound respecting the level in the options menu, but on a global level.
    /// </summary>
    public void PlayGlobal()
    {
        this.UpdateAudioSource();
        if(isMusic)
        {
            SoundSingleton.GetInstance().PlayMusic(this);
        }
        else
        {
            SoundSingleton.GetInstance().PlaySFX(this);
        }
    }

    /// <summary>
    /// Method
    /// Updates the audio source's settings.
    /// </summary>
    private void UpdateAudioSource()
    {
        audioSource = audioSource == null ? this.gameObject.AddComponent<AudioSource>() : audioSource;
        this.audioSource.clip = sound;
        this.audioSource.loop = isLooping;
        float volume = isMusic ? GameOptionsStaticClass.musicVolume : GameOptionsStaticClass.sfxVolume;
        this.audioSource.volume = volume * volumeMultiplier;
    }

    /// <summary>
    /// Method
    /// Getter for the AuidoSource.
    /// </summary>
    public AudioSource GetAudioSource()
    {
        this.UpdateAudioSource();
        return this.audioSource;
    }
}
