using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Theme : MonoBehaviour
{
    public Color white;
    public Color black;
    public Sprite textureSprite;

    public Board board;

    
    public void ChangeTheme()
    {

        for (int x = 0; x < board.Column; x++)
            for (int y = 0; y < board.Row; y++)
            {
                board.allCells[x][y].GetComponent<Image>().color = black;
                board.allCells[x][y].GetComponent<Image>().sprite = textureSprite;
            }


        // Color
        for (int x = 0; x < board.Column; x += 2)
        {
            for (int y = 0; y < board.Row; y++)
            {
                // Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                // Color
                //Color col = new Color32(230, 220, 187, 255);
                Image im = board.allCells[finalX][y].GetComponent<Image>();
                im.color = white;
            }
        }
    
    }
    
}
