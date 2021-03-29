using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Board board;

    public PieceManager pieceManager;

    void Start()
    {
        board.Create();

        pieceManager.Setup(board);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0); // Menu
    }

    public void Reload()
    {
        // SceneManager.LoadScene(1); // Menu
        pieceManager.ResetGame();
    }
}

