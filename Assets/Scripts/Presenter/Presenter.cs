using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class Presenter : MonoBehaviour
{
    public TalkView talkView;
    public TalkModel talkModel;
    // Start is called before the first frame update
    void Start()
    {
        talkView.talkButton.OnClickAsObservable().Subscribe( _=> talkModel.ShowChatWindow(talkView.chatWindow)).AddTo(this);
        talkView.chatWindow.onEndEdit.AsObservable().Subscribe(q=> talkModel.ShowAns(talkModel.gptInstance.ChatGPT(q),talkView.chatPanel)).AddTo(this);
        talkView.nextButton.OnClickAsObservable().Subscribe(_=> talkModel.CloseAns(talkView.chatPanel)).AddTo(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }   

}
