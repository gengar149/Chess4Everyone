using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private bool isFirstMove = true;


    public override void Setup(bool newIsWhite, PieceManager newPM)
    {
        base.Setup(newIsWhite, newPM);
        movement = isWhite ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/pawn");

        isFirstMove = true;
    }

    protected override void Move()
    {
        base.Move();

        isFirstMove = false;
    }

    private bool MatchesState(Cell target, CellState targetState)
    {
        CellState cellstate = target.GetState(this);

        if(cellstate == targetState)
        {
            // Add to list
            if (cellstate == CellState.ENEMY)
            {
                target.outlineImage.GetComponent<Image>().color = new Color(1, 0, 0, (float)0.5);
            }
            else
            {
                target.outlineImage.GetComponent<Image>().color = new Color(0, 1, 0, (float)0.5);
            }
            highlightedCells.Add(target);
            return true;
        }
        return false;
    }

    protected override void CheckPathing()
    {
        
        // target pos
        int currentX = currentCell.boardPosition.x;
        int currentY = currentCell.boardPosition.y;

        // top left
        try
        {
            MatchesState(currentCell.board.allCells[currentX - movement.z][currentY + movement.z], CellState.ENEMY);
        }
        catch (Exception e) { e.ToString(); }

        try
        {
            // forward
            if (MatchesState(currentCell.board.allCells[currentX][currentY + movement.y], CellState.FREE))
            {
                if (isFirstMove)
                {
                    MatchesState(currentCell.board.allCells[currentX][currentY + movement.y * 2], CellState.FREE);
                }
            }
        }
        catch (Exception e) { e.ToString(); }

        // top right
        try
        {
            MatchesState(currentCell.board.allCells[currentX + movement.z][currentY + movement.z], CellState.ENEMY);
        }
        catch (Exception e) { e.ToString(); }

    }

}
