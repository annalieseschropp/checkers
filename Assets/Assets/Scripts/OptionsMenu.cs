using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Button backButton;
    public Slider moveSpeedSlider;

    // Start is called before the first frame update
    void Start()
    {
        Button backBtn = backButton.GetComponent<Button>();
        backBtn.onClick.AddListener(GoBack);
        backBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Slider speedSlider = moveSpeedSlider.GetComponent<Slider>();
        speedSlider.onValueChanged.AddListener(OnMoveSpeedSliderChanged);
        speedSlider.value = GameOptionsStaticClass.moveSpeed;
    }

    public void OnMoveSpeedSliderChanged(float value)
    {
        GameOptionsStaticClass.moveSpeed = value;
    }

    public void GoBack()
    {
        Debug.Log("Back To Main Menu");
        SceneManager.LoadScene("Menu");
    }
}
