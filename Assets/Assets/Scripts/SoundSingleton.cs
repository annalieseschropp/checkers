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

            buttonClickSource = AddAudioSource(SoundBank.GetInstance().buttonClickSound, false, false);
            menuMusicSource = AddAudioSource(SoundBank.GetInstance().menuMusicSound, true, true);
            checker1Source = AddAudioSource(SoundBank.GetInstance().checkerSound1, false, false);
            checker2Source = AddAudioSource(SoundBank.GetInstance().checkerSound2, false, false);
            checker3Source = AddAudioSource(SoundBank.GetInstance().checkerSound3, false, false);
            gameOverSource = AddAudioSource(SoundBank.GetInstance().gameOverSound, false, false);
            DontDestroyOnLoad(objectInstance);
        }
        return instance;
    }

    private static ControlledAudioSource AddAudioSource(AudioClip clip, bool isMusic, bool isLooping)
    {
        ControlledAudioSource source = objectInstance.AddComponent<ControlledAudioSource>();
        source.sound = clip;
        source.isMusic = isMusic;
        source.isLooping = isLooping;
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