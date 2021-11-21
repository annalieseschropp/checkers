using UnityEngine;

public class SoundBank : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public AudioClip menuMusicSound;

    private static SoundBank instance;

    public static SoundBank GetInstance()
    {
        if (instance == null)
        {
            instance = (Instantiate(Resources.Load("SoundBank")) as GameObject).GetComponent<SoundBank>();
        }
        return instance;
    }
}