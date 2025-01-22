using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    System.Diagnostics.Process process = null;
    public static int level = 0;
    string lastFEN;

    public static Dictionary<int, int> IA_Level = new Dictionary<int, int>()
    {
        {0, 0},
        {1, 5},
        {2, 20}
    };

    public static Dictionary<int, int> IA_Game_Level = new Dictionary<int, int>()
    {
        {0, 1},
        {5, 2},
        {20, 3}
    };

    public void Setup()
    {
        process = new System.Diagnostics.Process();
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "IA/stockfish_13/stockfish_13_win_x64.exe");
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogError("Stockfish executable not found at: " + filePath);
            return;
        }
        process.StartInfo.FileName = filePath;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        process.StandardInput.WriteLine("setoption Name Skill Level value " + level);
        process.StandardInput.WriteLine("position startpos");

        //lastFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        lastFEN = GetFEN();
    }

    public void Close()
    {
        process.Close();
    }

    public string GetBestMove()
    {
        string setupString = "position fen " + lastFEN;
        process.StandardInput.WriteLine(setupString);

        // Process for 5 seconds
        string processString = "go movetime 1";

        // Process deep
        //string processString = "go depth 1";

        process.StandardInput.WriteLine(processString);

        string bestMoveInAlgebraicNotation = "";
        do
        {
            bestMoveInAlgebraicNotation = process.StandardOutput.ReadLine();
        } while (!bestMoveInAlgebraicNotation.Contains("bestmove"));

        bestMoveInAlgebraicNotation = bestMoveInAlgebraicNotation.Substring(9, 4);

        return bestMoveInAlgebraicNotation;
    }

    public string GetFEN()
    {
        process.StandardInput.WriteLine("d");
        string output = "";
        do
        {
            output = process.StandardOutput.ReadLine();
        }
        while (!output.Contains("Fen"));

        output = output.Substring(5);
        return output;
    }

    public void setIAmove(string move)
    {
        string setupString = "position fen " + lastFEN + " moves " + move;
        process.StandardInput.WriteLine(setupString);
        lastFEN = GetFEN();
    }
}
