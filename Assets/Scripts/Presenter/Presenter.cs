using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;


public class Presenter : MonoBehaviour
{
    public TalkView talkView;
    public TalkModel talkModel;
    public LogView logView;
    public LogModel logModel;
    public ShopView shopView;
    public ShopModel shopModel;

    // Start is called before the first frame update
    void Start()
    {
        talkView.talkButton.OnClickAsObservable().Subscribe( _=> talkModel.ShowChatWindow(talkView.chatWindow)).AddTo(this);
        talkView.chatWindow.onEndEdit
            .AsObservable()
            .Where(q => q != "")
            .DistinctUntilChanged()
            .Subscribe(async (q) =>{
                talkModel.ShowAns(await talkModel.gptInstance.ChatGPT(q), talkView.chatPanel);
                talkModel.CleanChatWindow(talkView.chatWindow);
            }).AddTo(this);
        talkView.nextButton.OnClickAsObservable().Subscribe(_=> talkModel.CloseAns(talkView.chatPanel)).AddTo(this);
        talkView.chatWindow.onSelect.AddListener(_ => talkModel.OnSelectChatWindow(talkView.talkButton));
        talkView.chatWindow.onDeselect.AddListener(_=> talkModel.OnDeselectChatWindow(talkView.talkButton));

        GptCore.messageBox
            .ObserveAdd()
            .Where(message => message.Value.role != "system")
            .Subscribe(message => logModel.GenaleteLogContents(message.Value.role,message.Value.content,logView))
            .AddTo(this);
        logView.logButton.OnClickAsObservable().Subscribe(_=>logModel.ShowLog(logView.logPanel)).AddTo(this);

        shopView.shopButton.OnClickAsObservable().Subscribe(_=>shopModel.ShowShop(shopView.shopPanel)).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }   

}
