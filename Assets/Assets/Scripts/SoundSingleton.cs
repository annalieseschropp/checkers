using UnityEngine;

/// <summary>
/// Class
/// Singleton for playing sounds that can continue past scene loading. Also useful for menu SFX.
/// </summary>
public class SoundSingleton : MonoBehaviour
{
    private static GameObject objectInstance;
    private static SoundSingleton instance;
    private static ControlledAudioSource sfxAudioSource;
    private static ControlledAudioSource musicAudioSource;
    private static ControlledAudioSource buttonClickSource;
    private static ControlledAudioSource menuMusicSource;
    private static ControlledAudioSource checker1Source;
    private static ControlledAudioSource checker2Source;
    private static ControlledAudioSource checker3Source;
    private static ControlledAudioSource gameOverSource;

    /// <summary>
    /// Method
    /// Getter for the initialized instance.
    /// </summary>
    public static SoundSingleton GetInstance()
    {
        if(!instance)
        {
            objectInstance = new GameObject("soundSingletonObject");
            sfxAudioSource = objectInstance.AddComponent<ControlledAudioSource>();
            sfxAudioSource.isMusic = false;
            musicAudioSource = objectInstance.AddComponent<ControlledAudioSource>();
            musicAudioSource.isMusic = true;
            instance = objectInstance.AddComponent<SoundSingleton>();

            buttonClickSource = AddAudioSource(SoundBank.GetInstance().buttonClickSound, false, false, 1.0f);
            menuMusicSource = AddAudioSource(SoundBank.GetInstance().menuMusicSound, true, true, 0.5f);
            checker1Source = AddAudioSource(SoundBank.GetInstance().checkerSound1, false, false, 1.0f);
            checker2Source = AddAudioSource(SoundBank.GetInstance().checkerSound2, false, false, 1.0f);
            checker3Source = AddAudioSource(SoundBank.GetInstance().checkerSound3, false, false, 1.0f);
            gameOverSource = AddAudioSource(SoundBank.GetInstance().gameOverSound, false, false, 1.0f);
            DontDestroyOnLoad(objectInstance);
        }
        return instance;
    }

    /// <summary>
    /// Method
    /// Utility to load the correct asset reference and settings for a given sound.
    /// </summary>
    private static ControlledAudioSource AddAudioSource(AudioClip clip, bool isMusic, bool isLooping, float volumeMultiplier)
    {
        ControlledAudioSource source = objectInstance.AddComponent<ControlledAudioSource>();
        source.sound = clip;
        source.isMusic = isMusic;
        source.isLooping = isLooping;
        source.volumeMultiplier = volumeMultiplier;
        return source;
    }

    /// <summary>
    /// Method
    /// Plays a sound effect stored in the given audio source.
    /// </summary>
    public void PlaySFX(ControlledAudioSource audioSource)
    {
        Play(audioSource, sfxAudioSource);
    }

    /// <summary>
    /// Method
    /// Plays music stored in the given audio source.
    /// </summary>
    public void PlayMusic(ControlledAudioSource audioSource)
    {
        if(!musicAudioSource.GetAudioSource().isPlaying)
        {
            Play(audioSource, musicAudioSource);
        }
    }

    /// <summary>
    /// Method
    /// Stops playing music.
    /// </summary>
    public void StopMusic(ControlledAudioSource audioSource)
    {
        if(musicAudioSource.GetAudioSource().isPlaying)
        {
            musicAudioSource.Stop();
        }
    }

    /// <summary>
    /// Method
    /// Updates the volume of music while it might still be playing.
    /// </summary>
    public void UpdateMusicVolume()
    {
        musicAudioSource.GetAudioSource().volume = musicAudioSource.volumeMultiplier * GameOptionsStaticClass.musicVolume;
    }

    /// <summary>
    /// Method
    /// Updates the volume of SFX while it might still be playing.
    /// </summary>
    public void UpdateSFXVolume()
    {
        sfxAudioSource.GetAudioSource().volume = sfxAudioSource.volumeMultiplier * GameOptionsStaticClass.sfxVolume;
    }

    /// <summary>
    /// Method
    /// Play button click sound effect for menu UI.
    /// </summary>
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSource);
    }

    /// <summary>
    /// Method
    /// Play one of three checker movement sound effects.
    /// </summary>
    public void PlayCheckerSound1()
    {
        PlaySFX(checker1Source);
    }

    /// <summary>
    /// Method
    /// Play one of three checker movement sound effects.
    /// </summary>
    public void PlayCheckerSound2()
    {
        PlaySFX(checker2Source);
    }

    /// <summary>
    /// Method
    /// Play one of three checker movement sound effects.
    /// </summary>
    public void PlayCheckerSound3()
    {
        PlaySFX(checker3Source);
    }

    /// <summary>
    /// Method
    /// Play the sound effect for when the game is over.
    /// </summary>
    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSource);
    }

    /// <summary>
    /// Method
    /// Play music intended for the main menu.
    /// </summary>
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusicSource);
    }

    /// <summary>
    /// Method
    /// Utility for playing music or sound effects.
    /// </summary>
    private void Play(ControlledAudioSource audioSource, ControlledAudioSource player)
    {
        player.sound = audioSource.sound;
        player.volumeMultiplier = audioSource.volumeMultiplier;
        player.isLooping = audioSource.isLooping;
        player.isMusic = audioSource.isMusic;
        player.Play();
    }
}
