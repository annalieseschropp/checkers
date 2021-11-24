using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Button backButton;
    public Button sfxTestButton;
    public Slider moveSpeedSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button clearRecordsButton;
    public Text clearedRecordsStatusText;

    /// <summary>
    /// Method
    /// Adds event listeners to all buttons and sliders.
    /// </summary>
    void Start()
    {
        Button backBtn = backButton.GetComponent<Button>();
        backBtn.onClick.AddListener(GoBack);
        backBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Button sfxTestBtn = sfxTestButton.GetComponent<Button>();
        sfxTestBtn.onClick.AddListener(TestSFX);

        Slider speedSlider = moveSpeedSlider.GetComponent<Slider>();
        speedSlider.onValueChanged.AddListener(OnMoveSpeedSliderChanged);
        speedSlider.value = GameOptionsStaticClass.moveSpeed;

        Slider musicSlider = musicVolumeSlider.GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(MusicVolumeSliderChanged);
        musicSlider.value = GameOptionsStaticClass.musicVolume;

        Slider sfxSlider = sfxVolumeSlider.GetComponent<Slider>();
        sfxSlider.onValueChanged.AddListener(SFXVolumeSliderChanged);
        sfxSlider.value = GameOptionsStaticClass.sfxVolume;

        Button clearBtn = clearRecordsButton.GetComponent<Button>();
        clearBtn.onClick.AddListener(ClearRecords);

        Text statusText = clearedRecordsStatusText.GetComponent<Text>();
        clearedRecordsStatusText.text = "";
    }

    /// <summary>
    /// Method
    /// Event listener for speed slider.
    /// </summary>
    public void OnMoveSpeedSliderChanged(float value)
    {
        GameOptionsStaticClass.moveSpeed = value;
    }

    /// <summary>
    /// Method
    /// Event listener for music slider.
    /// </summary>
    public void MusicVolumeSliderChanged(float value)
    {
        GameOptionsStaticClass.musicVolume = value;
        SoundSingleton.GetInstance().UpdateMusicVolume();
    }

    /// <summary>
    /// Method
    /// Event listener for SFX slider.
    /// </summary>
    public void SFXVolumeSliderChanged(float value)
    {
        GameOptionsStaticClass.sfxVolume = value;
        SoundSingleton.GetInstance().UpdateSFXVolume();
    }

    /// <summary>
    /// Method
    /// Event listener for SFX test button.
    /// </summary>
    public void TestSFX()
    {
        SoundSingleton.GetInstance().PlayCheckerSound2();
    }

    /// <summary>
    /// Method
    /// Event listener for the back button.
    /// </summary>
    public void GoBack()
    {
        Debug.Log("Back To Main Menu");
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Method
    /// Event listener for the clear records button.
    /// </summary>
    public void ClearRecords()
    {
        RecordKeeper.ClearRecords();
        GameHistoryRecordKeeper.DestroyAllData();
        StartCoroutine(UpdateStatus());
    }

    public IEnumerator UpdateStatus()
    { 
        clearedRecordsStatusText.text = "Successfully Cleared Records";
        yield return new WaitForSeconds(2);
        clearedRecordsStatusText.text = "";
    }
}
