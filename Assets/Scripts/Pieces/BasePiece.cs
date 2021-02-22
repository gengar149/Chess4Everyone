using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();

    /// <summary>
    /// Cellule visée par la souris
    /// </summary>
    protected Cell targetCell = null;

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

    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        // Target position
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // Check each cell
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            if (currentX < 0 || currentY < 0 ||
                currentX > currentCell.board.Column - 1 || currentY > currentCell.board.Row - 1)
                continue;

            Cell targeted = currentCell.board.allCells[currentX][currentY];

            CellState state = targeted.GetState(this);
            if (state != CellState.FRIEND)
            {
                // Add to list
                if (state == CellState.ENEMY)
                {
                    targeted.outlineImage.GetComponent<Image>().color = new Color(255,0,0,180);
                }
                highlightedCells.Add(targeted);
            }            
        }
    }

    protected virtual void CheckPathing()
    {
        // Horizontal
        CreateCellPath(1, 0, movement.x);
        CreateCellPath(-1, 0, movement.x);

        // Vertical 
        CreateCellPath(0, 1, movement.y);
        CreateCellPath(0, -1, movement.y);

        // Upper diagonal
        CreateCellPath(1, 1, movement.z);
        CreateCellPath(-1, 1, movement.z);

        // Lower diagonal
        CreateCellPath(-1, -1, movement.z);
        CreateCellPath(1, -1, movement.z);
    }

    protected void ShowCellsHighlight()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = true;
    }

    protected void ClearCellsHighlight()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = false;

        highlightedCells.Clear();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        // Test for cells
        CheckPathing();

        // Show valid cells
        ShowCellsHighlight();

        transform.position = Input.mousePosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // Follow pointer
        transform.position += (Vector3)eventData.delta;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);        

        // Get target cell
        targetCell = null;
        foreach (Cell cell in highlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectTransform, Input.mousePosition))
            {
                targetCell = cell;
                break;
            }
        }

        // Return to his original position
        if (!targetCell)
        {
            transform.position = currentCell.transform.position; // gameObject
        }
        else
        {
            Move();
            pieceManager.SetTurn(!isWhite);
        }

        // Hide Highlited
        ClearCellsHighlight();
    }

    public void Reset()
    {
        Kill();
        Place(originalCell);
    }

    public virtual void Kill()
    {
        currentCell.currentPiece = null;
        gameObject.SetActive(false);
    }

    public virtual void Move()
    {
        // If there is a piece, remove it
        targetCell.RemovePiece();

        currentCell.currentPiece = null;

        currentCell = targetCell;
        currentCell.currentPiece = this;

        transform.position = currentCell.transform.position;
        targetCell = null;
    }
}
