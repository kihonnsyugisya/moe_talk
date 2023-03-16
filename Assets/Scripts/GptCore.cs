using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class GptCore 
{
    static ReactiveDictionary<EMOTIONS, int> emotionData = new ReactiveDictionary<EMOTIONS, int>();

    static string ChatGPT(string question)
    {
        return "d";
    }

    static string Translate(string lang)
    {
        
        return "d";

    }



}

public enum EMOTIONS
{
    HAPPY,LOVE,SAD,ANGRY,FEAR 
}