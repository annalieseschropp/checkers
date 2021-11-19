using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchBoards : MonoBehaviour
{
    public Button switchButton;
    public GameObject Leaderboard;
    public GameObject GameHistory;
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        // Set the on click listener for the back button
        Button back = switchButton.GetComponent<Button>();
        back.onClick.AddListener(SwitchBoard);
    }

    // Listener for the back button
    public void SwitchBoard()
    {
        if (Leaderboard.activeSelf)
        {
            Leaderboard.SetActive(false);
            GameHistory.SetActive(true);
            buttonText.text = "Switch to\nLeader Board";
        }
        else
        {
            Leaderboard.SetActive(true);
            GameHistory.SetActive(false);
            buttonText.text = "Switch to\nGame History";
        }
    }
}
