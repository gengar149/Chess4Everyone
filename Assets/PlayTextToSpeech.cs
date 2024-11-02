using LMNT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTextToSpeech : MonoBehaviour
{
    private LMNTSpeech zeke;
    public string text;
    int num = 1;


    public void playAudio() {
        zeke = GetComponent<LMNTSpeech>();
        //zeke.dialogue = num++.ToString();
        StartCoroutine(zeke.Talk());
    }

    public void changeAudio()
    {
        string erm = "new text";
        zeke.SetDialogue(erm);

        
    }

}
