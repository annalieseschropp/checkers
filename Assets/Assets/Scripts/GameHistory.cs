using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHistory : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowsParent;
    public GameObject self;


    // Start is called before the first frame update
    void Start()
    {
        switchActive();
        GameHistoryRecordKeeper.LoadData();
        for (int i = GameHistoryRecordKeeper.listOfGameHistory.Count - 1; i >= 0; i--)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = GameHistoryRecordKeeper.listOfGameHistory[i].blackPlayerName;
            texts[1].text = GameHistoryRecordKeeper.listOfGameHistory[i].whitePlayerName;
            texts[2].text = GameHistoryRecordKeeper.listOfGameHistory[i].blackFinalScore.ToString();
            texts[3].text = GameHistoryRecordKeeper.listOfGameHistory[i].whitePlayerName.ToString();
            texts[4].text = GameHistoryRecordKeeper.listOfGameHistory[i].gameWinner;
        }
        GameHistoryRecordKeeper.SaveData();
    }

    void switchActive()
    {
        if (self.activeSelf)
            self.SetActive(false);
        else
            self.SetActive(true);
    }
}
