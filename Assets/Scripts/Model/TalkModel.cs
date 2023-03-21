using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;
using System;

public class TalkModel : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        gptInstance = new GptCore();
        
        var enter = await gptInstance.ChatGPT("応答してください");
        Debug.Log(enter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GptCore gptInstance;

    public IntReactiveProperty life;

    public void ShowAns(string ans,GameObject ChatPanel)
    {
        ChatPanel.SetActive(true);
        Text ansewerText = ChatPanel.transform.GetChild(0).GetComponent<Text>();
        ansewerText.text = ans;
    }

    public void CloseAns(GameObject ChatPanel)
    {
        ChatPanel.SetActive(false);
    }

    public void ShowChatWindow(TMP_InputField chatWindow)
    {
        chatWindow.Select();
        Debug.Log(chatWindow.text);

    }

    
}
