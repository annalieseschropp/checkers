using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    private static GameObject objectInstance;
    private static SoundSingleton instance;
    private static ControlledAudioSource sfxAudioSource;
    private static ControlledAudioSource musicAudioSource;

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
            DontDestroyOnLoad(objectInstance);
        }
        return instance;
    }

    public void PlaySFX(ControlledAudioSource audioSource)
    {
        Play(audioSource, sfxAudioSource);
    }

    public void PlayMusic(ControlledAudioSource audioSource)
    {
        Play(audioSource, musicAudioSource);
    }

    public void StopMusic(ControlledAudioSource audioSource)
    {
        if(musicAudioSource.GetAudioSource().isPlaying)
        {
            musicAudioSource.Stop();
        }
    }

    private void Play(ControlledAudioSource audioSource, ControlledAudioSource player)
    {
        player.sound = audioSource.sound;
        player.volumeMultiplier = audioSource.volumeMultiplier;
        player.Play();
    }
}