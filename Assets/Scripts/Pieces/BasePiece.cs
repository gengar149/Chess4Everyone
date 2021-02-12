using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public bool isWhite;
    [HideInInspector]
    static int cellPadding = 10;

    protected Cell originalCell = null;
    protected Cell currentCell = null;

    protected RectTransform rt = null;
    protected PieceManager pieceManager;

    public static int CellPadding { get => cellPadding; }

    public virtual void Setup(bool newIsWhite, PieceManager newPM)
    {
        pieceManager = newPM;
        isWhite = newIsWhite;

        rt = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        currentCell = newCell;
        originalCell = newCell;
        currentCell.currentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true); // ?
    }

}
