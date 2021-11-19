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
    }

    void switchActive()
    {
        if (self.activeSelf)
            self.SetActive(false);
        else
            self.SetActive(true);
    }
}
