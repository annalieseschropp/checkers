using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowsParent;
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        // Set the on click listener for the back button
        Button back = backButton.GetComponent<Button>();
        back.onClick.AddListener(BackButtonOnClick);
        // Load the data from the records
        RecordKeeper.LoadData();
        for (int i = 0; i < RecordKeeper.listOfRecords.Count; i++)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = i.ToString();
            texts[1].text = RecordKeeper.listOfRecords[i].name;
            texts[2].text = RecordKeeper.listOfRecords[i].gamesWon.ToString();
            texts[3].text = RecordKeeper.listOfRecords[i].gamesLost.ToString();
        }
        // Save the data back to the menu
        RecordKeeper.SaveData();
    }

    // Listener for the back button
    public void BackButtonOnClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
