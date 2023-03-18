using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GptCore 
{
    static ReactiveDictionary<EMOTIONS, int> emotionData = new ReactiveDictionary<EMOTIONS, int>();

    string apiKey;

    public GptCore(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public string ChatGPT(string question)
    {
        return "chat gpt";
    }

    string Translate(string lang)
    {
        
        return "translate";

    }



}

public enum EMOTIONS
{
    HAPPY,LOVE,SAD,ANGRY,FEAR 
}