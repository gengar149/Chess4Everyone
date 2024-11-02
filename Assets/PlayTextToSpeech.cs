using LMNT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTextToSpeech : MonoBehaviour
{
    private LMNTSpeech zeke;
    public string text;
    int num = 1;

    private void Start()
    {
        zeke = GetComponent<LMNTSpeech>();
    }

    public void PlayAudio() {
        StartCoroutine(zeke.Talk());
    }

    public void ChangeAudio(string newText)
    {
        zeke.SetDialogue(newText);
        PlayAudio(); 
    }

}
