﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Diagnostics;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public bool isWhite;

    public bool hasMoved = false;

    [HideInInspector]
    static int cellPadding = 10;

    protected Cell originalCell = null;

    [HideInInspector]
    public Cell currentCell = null;

    protected RectTransform rt = null;
    protected PieceManager pieceManager;

    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();
    protected List<Cell> attackedCells = new List<Cell>();

    /// <summary>
    /// Cellule visée par la souris
    /// </summary>
    public Cell targetCell = null;

    public bool inDrag = false;

    [SerializeField] TMP_Text moveHistory;


    public PieceManager GetPieceManager()
    {
        return pieceManager;
    }

    public static int CellPadding { get => cellPadding; }
    public Cell TargetCell { get => targetCell; set => targetCell = value; }

    /// <summary>
    /// Init piece
    /// </summary>
    /// <param name="newIsWhite"></param>
    /// <param name="newPM"></param>
    public virtual void Setup(bool newIsWhite, PieceManager newPM)
    {
        inDrag = false;
        pieceManager = newPM;
        isWhite = newIsWhite;
        hasMoved = false;

        rt = GetComponent<RectTransform>();

        if (pieceManager.theme == null)
        {
            if(isWhite)
                GetComponent<Image>().color = Color.white;
            else
                GetComponent<Image>().color = Color.grey;
        }
        else
        {
            if (isWhite)
                GetComponent<Image>().color = pieceManager.theme.whitePiece;
            else
                GetComponent<Image>().color = pieceManager.theme.blackPiece;
        }
    }



    /// <summary>
    /// Place piece on the board
    /// </summary>
    /// <param name="newCell"></param>
    public void PlaceInit(Cell newCell)
    {
        currentCell = newCell;
        originalCell = newCell;
        currentCell.currentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true); // ?
    }

    /// <summary>
    /// Check possible moves for a direction
    /// </summary>
    /// <param name="xDirection"></param>
    /// <param name="yDirection"></param>
    /// <param name="movement"></param>
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
            if (state != CellState.FRIEND && state != CellState.CHECK &&  state != CellState.CHECK_ENEMY && state != CellState.CHECK_FRIEND)
            {
                if (!pieceManager.checkVerificationInProcess)
                {
                    // Add to list
                    if (state == CellState.ENEMY)
                    {
                        targeted.outlineImage.GetComponent<Image>().color = new Color(1, 0, 0, (float)0.5);
                    }
                    else
                    {
                        targeted.outlineImage.GetComponent<Image>().color = new Color(0, 1, 0, (float)0.5);
                    }
                    //highlightedCells.Add(targeted);
                }

                addPossibleCell(targeted);
            }
            if (state == CellState.ENEMY || state == CellState.FRIEND || state == CellState.CHECK_ENEMY || state == CellState.CHECK_FRIEND)
                break;
        }
    }

    /// <summary>
    /// Add cell in a specific list
    /// </summary>
    /// <param name="possibleCell"></param>
    protected void addPossibleCell(Cell possibleCell)
    {
        if (pieceManager.checkVerificationInProcess)
            attackedCells.Add(possibleCell);
        else
            highlightedCells.Add(possibleCell);
    }

    /// <summary>
    /// Check possible moves of a piece
    /// </summary>
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

    /// <summary>
    /// Display possible moves
    /// </summary>
    protected void ShowCellsHighlight()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = true;
    }

    /// <summary>
    /// Clear display of all possible moves
    /// </summary>
    protected void ClearCellsHighlight()
    {
        foreach (Cell cell in highlightedCells)
            cell.outlineImage.enabled = false;

        highlightedCells.Clear();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        inDrag = true;

        // Test for cells
        CheckPathing();

        // Show valid cells
        ShowCellsHighlight();

        transform.position = Input.mousePosition;
        transform.SetAsLastSibling();
    }




    public void TTsSelect()
    {

        inDrag = true;

        // Test for cells
        CheckPathing();

        // Show valid cells
        ShowCellsHighlight();

        transform.SetAsLastSibling();
    }




    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // Follow pointer
        transform.position += (Vector3)eventData.delta;
    }


    public string TTSPlayerMove(string move)
    {
        if (!ValidMove(move))
            return "Not A Valid Move: " + move;
        else
        {
            UnityEngine.Debug.Log("move valid");
            return "";
        }
    }


    bool ValidMove(string move)
    {
        if (move.Length > 3)
            return false;
        //else if (move.Length == 3)
            //code
        //else if (move.Length == 2)
            //code
        else
            return false;
    }






    public void TTsDrop(Cell newCell)
    {

        inDrag = false;

        // Get target cell
        targetCell = null;
        //foreach (Cell cell in highlightedCells)
        //{

            //if newCell is in highlightedCells - do stuff
            if (highlightedCells.Contains(newCell))
            {
                targetCell = newCell;
//                break;
            }
        //}


        //leave as is
        // Return to his original position
        if (!targetCell || pieceManager.gameState != GameState.INGAME)
        {
            transform.position = currentCell.transform.position; // gameObject
        }
        else
        {
            if (PieceManager.IAmode)
            {
                string move = "";
                move += pieceManager.posA[currentCell.boardPosition.x];
                move += pieceManager.posB[currentCell.boardPosition.y];
                move += pieceManager.posA[targetCell.boardPosition.x];
                move += pieceManager.posB[targetCell.boardPosition.y];
                // If promotion
                if (this.GetType() == typeof(Pawn) && (TargetCell.boardPosition.y == 0 || TargetCell.boardPosition.y == 7))
                {
                    move += "q";
                }
                string symbol = "";

                if (currentCell.currentPiece.GetType() == typeof(Queen))
                    symbol = "Q";

                else if (currentCell.currentPiece.GetType() == typeof(King))
                    symbol = "K";

                else if (currentCell.currentPiece.GetType() == typeof(Bishop))
                    symbol = "B";

                else if (currentCell.currentPiece.GetType() == typeof(Rook))
                    symbol = "R";

                else if (currentCell.currentPiece.GetType() == typeof(Knight))
                    symbol = "K";
                else
                    symbol = "";


                pieceManager.PrintPlayerMoves(symbol + pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                //moveHistory.text += move + "\n";
                //moveHistory.text += move + "\n";

                UnityEngine.Debug.Log("player moved");







                pieceManager.stockfish.setIAmove(move);
                Move();
            }
            else
            {
                string move = "";
                move += pieceManager.posA[currentCell.boardPosition.x];
                move += pieceManager.posB[currentCell.boardPosition.y];
                move += pieceManager.posA[targetCell.boardPosition.x];
                move += pieceManager.posB[targetCell.boardPosition.y];

                if (this.GetType() == typeof(Pawn) && (TargetCell.boardPosition.y == 0 || TargetCell.boardPosition.y == 7))
                {
                    move += "q";
                }
                string symbol = "";

                if (currentCell.currentPiece.GetType() == typeof(Queen))
                    symbol = "Q";

                else if (currentCell.currentPiece.GetType() == typeof(King))
                    symbol = "K";

                else if (currentCell.currentPiece.GetType() == typeof(Bishop))
                    symbol = "B";

                else if (currentCell.currentPiece.GetType() == typeof(Rook))
                    symbol = "R";

                else if (currentCell.currentPiece.GetType() == typeof(Knight))
                    symbol = "K";
                else
                    symbol = "";

                if (pieceManager.whitePieces.Contains(this))
                {

                    pieceManager.PrintPlayerMoves(symbol + pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                    Move();
                }
                else
                {
                    pieceManager.EnemyHistoryLog(symbol, pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                    Move();
                }
            }
        }

        // Hide Highlited
        ClearCellsHighlight();


    }











    public override void OnEndDrag(PointerEventData eventData)
    {
        
        base.OnEndDrag(eventData);

        inDrag = false;

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
        if (!targetCell || pieceManager.gameState != GameState.INGAME)
        {
            transform.position = currentCell.transform.position; // gameObject
        }
        else
        {
            if (PieceManager.IAmode)
            {
                string move = "";
                move += pieceManager.posA[currentCell.boardPosition.x];
                move += pieceManager.posB[currentCell.boardPosition.y];
                move += pieceManager.posA[targetCell.boardPosition.x];
                move += pieceManager.posB[targetCell.boardPosition.y];
                // If promotion
                if (this.GetType() == typeof(Pawn) && (TargetCell.boardPosition.y == 0 || TargetCell.boardPosition.y == 7))
                {
                    move += "q";
                }
                string symbol = "";

                if (currentCell.currentPiece.GetType() == typeof(Queen))
                    symbol = "Q";

                else if (currentCell.currentPiece.GetType() == typeof(King))
                    symbol = "K";

                else if (currentCell.currentPiece.GetType() == typeof(Bishop))
                    symbol = "B";

                else if (currentCell.currentPiece.GetType() == typeof(Rook))
                    symbol = "R";

                else if (currentCell.currentPiece.GetType() == typeof(Knight))
                    symbol = "K";
                else
                    symbol = "";


                pieceManager.PrintPlayerMoves(symbol + pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                //moveHistory.text += move + "\n";
                //moveHistory.text += move + "\n";

                UnityEngine.Debug.Log("player moved");







                pieceManager.stockfish.setIAmove(move);
                Move();
            }
            else
            {
                string move = "";
                move += pieceManager.posA[currentCell.boardPosition.x];
                move += pieceManager.posB[currentCell.boardPosition.y];
                move += pieceManager.posA[targetCell.boardPosition.x];
                move += pieceManager.posB[targetCell.boardPosition.y];

                if (this.GetType() == typeof(Pawn) && (TargetCell.boardPosition.y == 0 || TargetCell.boardPosition.y == 7))
                {
                    move += "q";
                }
                string symbol = "";

                if (currentCell.currentPiece.GetType() == typeof(Queen))
                    symbol = "Q";

                else if (currentCell.currentPiece.GetType() == typeof(King))
                    symbol = "K";

                else if (currentCell.currentPiece.GetType() == typeof(Bishop))
                    symbol = "B";

                else if (currentCell.currentPiece.GetType() == typeof(Rook))
                    symbol = "R";

                else if (currentCell.currentPiece.GetType() == typeof(Knight))
                    symbol = "K";
                else
                    symbol = "";

                if (pieceManager.whitePieces.Contains(this))
                {

                    pieceManager.PrintPlayerMoves(symbol + pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                    Move();
                }
                else
                {
                    pieceManager.EnemyHistoryLog(symbol, pieceManager.posA[targetCell.boardPosition.x] + pieceManager.posB[targetCell.boardPosition.y]);
                    Move();
                }
            }                   
        }

        // Hide Highlited
        ClearCellsHighlight();

        
    }

    public void Reset()
    {
        Kill();
        PlaceInit(originalCell);
    }

    public virtual void Kill()
    {
        currentCell.currentPiece = null;
        gameObject.SetActive(false);
    }

    public virtual void Move()
    {
        // sounds
        AudioClip clip = null;
        if (targetCell.currentPiece == null)
        {
            clip = (AudioClip)Resources.Load("Sounds/basic/move");
        }
        else
        {
            clip = (AudioClip)Resources.Load("Sounds/basic/pick");
        }

        // Disable check
        if (pieceManager.getKing(isWhite).isCheck)
        {
            pieceManager.getKing(isWhite).setCheck(false);
        }

        // If there is a piece, remove it
        targetCell.RemovePiece();

        UnityEngine.Debug.Log(targetCell.boardPosition);

        bool castling = false;
        // Handle castle
        if (currentCell.currentPiece.GetType() == typeof(King) && currentCell.currentPiece.hasMoved == false)
        {
            if(targetCell.boardPosition.x == 2)
            {
                BasePiece rook = currentCell.board.allCells[0][currentCell.boardPosition.y].currentPiece;
                rook.targetCell = currentCell.board.allCells[3][currentCell.boardPosition.y];
                rook.Move();
                castling = true;
            }
            else if(targetCell.boardPosition.x == 6)
            {
                BasePiece rook = currentCell.board.allCells[7][currentCell.boardPosition.y].currentPiece;
                rook.targetCell = currentCell.board.allCells[5][currentCell.boardPosition.y];
                rook.Move();
                castling = true;
            }
        }

        currentCell.currentPiece = null;

        currentCell = targetCell;
        currentCell.currentPiece = this;

        transform.position = currentCell.transform.position;
        targetCell = null;
		
		hasMoved = true;

        if(pieceManager.enPassantCell != null)
        {
            pieceManager.enPassantCell.enPassant = null;
            pieceManager.enPassantCell = null;
        }

        // verify if there is a check in the opposite side
        pieceManager.checkVerificationInProcess = true;
        if (isCheckVerif(isWhite))
        {
            pieceManager.getKing(!isWhite).setCheck(true);
            clip = (AudioClip)Resources.Load("Sounds/basic/servant-bell-ring");
        }
        
        pieceManager.checkVerificationInProcess = false;

        

        CheckGameOver(!isWhite);

        // Sounds
        if (pieceManager.gameState != GameState.INGAME)
            clip = null;
        if (clip != null)
            pieceManager.audio.PlayOneShot(clip);

        if (!pieceManager.IATurn && pieceManager.gameState == GameState.INGAME && !castling)
            pieceManager.SetTurn(!isWhite);
    }

    public bool isCheckVerif(bool AttakingSideIsWhite)
    {
        foreach (List<Cell> row in currentCell.board.allCells)
        {
            foreach(Cell boardCell in row)
            {
                BasePiece pieceBoard = boardCell.currentPiece;
                if(pieceBoard != null && pieceBoard.isWhite == AttakingSideIsWhite)
                {
                    King targetKing = pieceManager.getKing(!AttakingSideIsWhite);

                    pieceBoard.CheckPathing();
                    foreach (Cell cell in pieceBoard.attackedCells)
                    {
                        if(cell.boardPosition.x == targetKing.currentCell.boardPosition.x &&
                            cell.boardPosition.y == targetKing.currentCell.boardPosition.y)
                        {
                            pieceBoard.ClearAttackedCell();
                            return true;
                        }
                    }
                    pieceBoard.ClearAttackedCell();
                }
            }
        }
        return false;
    }

    public void ClearAttackedCell()
    {
        attackedCells.Clear();
    }

    public bool PossibleMove(bool isWhite)
    {
        foreach (List<Cell> row in currentCell.board.allCells)
        {
            foreach (Cell boardCell in row)
            {
                BasePiece piece = boardCell.currentPiece;
                if (piece != null && piece.isWhite == isWhite)
                {
                    piece.CheckPathing();
                    if (piece.highlightedCells.Count > 0)
                    {
                        piece.highlightedCells.Clear();
                        return true;
                    }
                    piece.highlightedCells.Clear();
                }
            }
        }
        return false;
    }

    public void CheckGameOver(bool isWhite)
    {
        if (!PossibleMove(isWhite))
        {
            if (pieceManager.getKing(isWhite).isCheck)
            {
                if (isWhite)
                    pieceManager.gameState = GameState.BLACK_WIN;
                else
                    pieceManager.gameState = GameState.WHITE_WIN;
            }
            else
            {
                pieceManager.gameState = GameState.PAT;
            }
            UnityEngine.Debug.Log(pieceManager.gameState);
            pieceManager.ShowResult();
        }
    }
}
