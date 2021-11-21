using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

/// <summary>
/// Class
/// Visual representation of a set of related 2D checker pieces.
/// </summary>
public class ControlledAudioSource : MonoBehaviour
{
    public bool isMusic;
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
    /// Plays the sound respecting the level in the options menu, but on a global level.
    /// </summary>
    public void PlayGlobal()
    {
        this.UpdateAudioSource();
        Debug.Log("Transform.position: " + Camera.main.transform.position);
        //AudioSource.PlayClipAtPoint(this.audioSource.clip, this.transform.position, this.audioSource.volume);
        AudioSource.PlayClipAtPoint(this.audioSource.clip, Camera.main.transform.position, this.audioSource.volume);
    }

    /// <summary>
    /// Method
    /// Updates the audio source's settings.
    /// </summary>
    private void UpdateAudioSource()
    {
        audioSource = audioSource == null ? this.gameObject.AddComponent<AudioSource>() : audioSource;
        this.audioSource.clip = sound;
        float volume = isMusic ? GameOptionsStaticClass.musicVolume : GameOptionsStaticClass.sfxVolume;
        this.audioSource.volume = volume * volumeMultiplier;
    }
}
