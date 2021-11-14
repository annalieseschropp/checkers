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

        Slider speedSlider = moveSpeedSlider.GetComponent<Slider>();
        speedSlider.onValueChanged.AddListener(OnMoveSpeedSliderChanged);
    }

    public void OnMoveSpeedSliderChanged(float value)
    {
        Debug.Log("Slider Value" + value);
    }

    public void GoBack()
    {
        Debug.Log("Back To Main Menu");
        SceneManager.LoadScene("Menu");
    }
}
