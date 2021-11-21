using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    private static GameObject objectInstance;
    private static SoundSingleton instance;
    private static ControlledAudioSource sfxAudioSource;
    private static ControlledAudioSource musicAudioSource;
    private static ControlledAudioSource buttonClickSource;
    private static ControlledAudioSource menuMusicSource;

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