using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Button singlePlayerButton;
    public Button twoPlayerButton;
    public Button leaderboardButton;
    public Button optionsButton;
    public Button quitButton;
    public Button creditsButton;

    /// <summary>
    /// Method
    /// Initializer performed before any script attempts to access the PieceSet.
    /// </summary>
    void Awake()
    {
        SoundSingleton.GetInstance().PlayMenuMusic();
    }

    void Start() {
        Button singleButton = singlePlayerButton.GetComponent<Button>();
        Button twoButton = twoPlayerButton.GetComponent<Button>();
        Button leaderboard = leaderboardButton.GetComponent<Button>();
        Button options = optionsButton.GetComponent<Button>();
        Button quit = quitButton.GetComponent<Button>();
        Button credits = creditsButton.GetComponent<Button>();

        singleButton.onClick.AddListener(PlayAIGame);
        twoButton.onClick.AddListener(PlayGame);
        leaderboard.onClick.AddListener(ShowLeaderboards);
        options.onClick.AddListener(LoadOptions);
        quit.onClick.AddListener(QuitGame);
        creditsButton.onClick.AddListener(LoadCredits);

        singleButton.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        twoButton.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        leaderboard.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        options.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        quit.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        creditsButton.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
    }

    public void PlayAIGame()
    {
        SceneManager.LoadScene("AINameEntry");
    }
    
    public void PlayGame()
    {
        Debug.Log("Loading Game!");
        SceneManager.LoadScene("NameEntry");
    }

    public void ShowLeaderboards()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void LoadOptions()
    {
        Debug.Log("Loading Options!");
        SceneManager.LoadScene("OptionsMenu");
    }

    public void LoadCredits()
    {
        Debug.Log("Loading credits screen!");
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
