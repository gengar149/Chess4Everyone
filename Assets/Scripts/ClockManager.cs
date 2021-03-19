using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    // Start is called before the first frame update



    private Timer clockWhite;
    private Timer clockBlack;

    public TMP_Text displayWhite;
    public TMP_Text displayBlack;

    public GameObject highlightClockW;
    public GameObject highlightClockB;



    private bool isWhiteTurn = true;

    void Start()
    {
        
        clockWhite = new Timer();
        clockBlack = new Timer();
       
        clockWhite.Setup(15, displayWhite);
        clockBlack.Setup(25, displayBlack);

        clockWhite.Start();
        clockBlack.Start();
      
        highlightClockW.SetActive(true);
        highlightClockB.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWhiteTurn == true) {
            clockWhite.Update();
        } else
        {
            clockBlack.Update();
        }
    }

    public void changeTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        highlightClockW.SetActive(!highlightClockW.activeSelf);
        highlightClockB.SetActive(!highlightClockB.activeSelf);

    }

    public void setTurn(bool isWhiteTurn)
    {
        this.isWhiteTurn = isWhiteTurn;
        highlightClockW.SetActive(isWhiteTurn);
        highlightClockB.SetActive(!isWhiteTurn);
    }
}
