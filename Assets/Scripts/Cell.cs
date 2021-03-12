using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    NONE,
    FRIEND,
    ENEMY,
    FREE
}

public class Cell : MonoBehaviour
{
    public Image outlineImage;

    [HideInInspector]
    public Vector2Int boardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board board = null;
    [HideInInspector]
    public RectTransform rectTransform = null;
    [HideInInspector]
    public BasePiece currentPiece;


    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        boardPosition = newBoardPosition;
        board = newBoard;

        rectTransform = GetComponent<RectTransform>();
        outlineImage.enabled = false;
    }

    public void RemovePiece()
    {
        if (currentPiece != null)
        {
            currentPiece.Kill();
        }
    }

    public CellState GetState(BasePiece checkingPiece)
    {
        if(currentPiece != null)
        {
            // if friend
            if (checkingPiece.isWhite == currentPiece.isWhite)
            {
                return CellState.FRIEND;
            }
            // if enemy
            else
            {
                return CellState.ENEMY;
            }

        }
        return CellState.FREE;
    }
}
