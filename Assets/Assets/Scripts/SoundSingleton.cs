using UnityEngine;

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

    private static ControlledAudioSource AddAudioSource(AudioClip clip, bool isMusic, bool isLooping, float volumeMultiplier)
    {
        ControlledAudioSource source = objectInstance.AddComponent<ControlledAudioSource>();
        source.sound = clip;
        source.isMusic = isMusic;
        source.isLooping = isLooping;
        source.volumeMultiplier = volumeMultiplier;
        return source;
    }

    public void PlaySFX(ControlledAudioSource audioSource)
    {
        Play(audioSource, sfxAudioSource);
    }

    public void PlayMusic(ControlledAudioSource audioSource)
    {
        if(!musicAudioSource.GetAudioSource().isPlaying)
        {
            Play(audioSource, musicAudioSource);
        }
    }

    public void StopMusic(ControlledAudioSource audioSource)
    {
        if(musicAudioSource.GetAudioSource().isPlaying)
        {
            musicAudioSource.Stop();
        }
    }

    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSource);
    }

    public void PlayCheckerSound1()
    {
        PlaySFX(checker1Source);
    }

    public void PlayCheckerSound2()
    {
        PlaySFX(checker2Source);
    }

    public void PlayCheckerSound3()
    {
        PlaySFX(checker3Source);
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSource);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusicSource);
    }

    private void Play(ControlledAudioSource audioSource, ControlledAudioSource player)
    {
        player.sound = audioSource.sound;
        player.volumeMultiplier = audioSource.volumeMultiplier;
        player.isLooping = audioSource.isLooping;
        player.isMusic = audioSource.isMusic;
        player.Play();
    }
}