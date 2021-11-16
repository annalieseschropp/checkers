using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CheckersState;
using CheckersMove;

public class PieceCaptureDisplay : MonoBehaviour
{
    public Text takenBlack;
    public Text takenWhite;
    public Board board;

    void Update()
    {
        takenBlack.text = board.GetBlackPiecesLost().ToString();
        takenWhite.text = board.GetWhitePiecesLost().ToString();
    }
}
