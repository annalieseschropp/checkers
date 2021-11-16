using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Button twoPlayerButton;
    public Button leaderboardButton;
    public Button optionsButton;
    public Button quitButton;
    public Button creditsButton;

    void Start() {
        Button twoButton = twoPlayerButton.GetComponent<Button>();
        Button leaderboard = leaderboardButton.GetComponent<Button>();
        Button options = optionsButton.GetComponent<Button>();
        Button quit = quitButton.GetComponent<Button>();
        Button credits = creditsButton.GetComponent<Button>();

        twoButton.onClick.AddListener(PlayGame);
        leaderboard.onClick.AddListener(ShowLeaderboards);
        options.onClick.AddListener(LoadOptions);
        quit.onClick.AddListener(QuitGame);
        creditsButton.onClick.AddListener(LoadCredits);
    }

    public void PlayGame()
    {
        Debug.Log("Loading Game!");
        SceneManager.LoadScene("NameEntry");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
