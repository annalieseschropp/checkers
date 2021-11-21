using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Button backButton;
    public Slider moveSpeedSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        Button backBtn = backButton.GetComponent<Button>();
        backBtn.onClick.AddListener(GoBack);
        backBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Slider speedSlider = moveSpeedSlider.GetComponent<Slider>();
        speedSlider.onValueChanged.AddListener(OnMoveSpeedSliderChanged);
        speedSlider.value = GameOptionsStaticClass.moveSpeed;

        Slider musicSlider = musicVolumeSlider.GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(MusicVolumeSliderChanged);
        musicSlider.value = GameOptionsStaticClass.musicVolume;

        Slider sfxSlider = sfxVolumeSlider.GetComponent<Slider>();
        sfxSlider.onValueChanged.AddListener(SFXVolumeSliderChanged);
        sfxSlider.value = GameOptionsStaticClass.sfxVolume;
    }

    public void OnMoveSpeedSliderChanged(float value)
    {
        GameOptionsStaticClass.moveSpeed = value;
    }

    public void MusicVolumeSliderChanged(float value)
    {
        GameOptionsStaticClass.musicVolume = value;
        SoundSingleton.GetInstance().UpdateMusicVolume();
    }

    public void SFXVolumeSliderChanged(float value)
    {
        GameOptionsStaticClass.sfxVolume = value;
        SoundSingleton.GetInstance().UpdateSFXVolume();
        SoundSingleton.GetInstance().PlayButtonClickSound();
    }

    public void GoBack()
    {
        Debug.Log("Back To Main Menu");
        SceneManager.LoadScene("Menu");
    }
}
