using UnityEngine;

public class SoundBank : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public AudioClip menuMusicSound;
    public AudioClip checkerSound1;
    public AudioClip checkerSound2;
    public AudioClip checkerSound3;
    public AudioClip gameOverSound;

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