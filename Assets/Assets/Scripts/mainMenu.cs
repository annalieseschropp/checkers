using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Button twoPlayerButton;
    public Button optionsButton;
    public Button quitButton;

    void Start() {
        Button twoButton = twoPlayerButton.GetComponent<Button>();
        Button options = optionsButton.GetComponent<Button>();
        Button quit = quitButton.GetComponent<Button>();

        twoButton.onClick.AddListener(PlayGame);
        options.onClick.AddListener(LoadOptions);
        quit.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        Debug.Log("Loading Game!");
        SceneManager.LoadScene("SampleScene");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadOptions()
    {
        Debug.Log("Loading Options!");
        SceneManager.LoadScene("OptionsMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
