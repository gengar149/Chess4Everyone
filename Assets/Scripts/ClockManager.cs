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

    private bool isWhiteTurn = true;


    void Start()
    {
        clockWhite = new Timer();
        clockBlack = new Timer();
       
        clockWhite.Setup(15, displayWhite);
        clockBlack.Setup(25, displayBlack);

        clockWhite.Start();
        clockBlack.Start();
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
    }
}
