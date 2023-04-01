using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class Presenter : MonoBehaviour
{
    public TalkView talkView;
    public TalkModel talkModel;
    public LogView logView;
    public LogModel logModel;
    public ShopView shopView;
    public ShopModel shopModel;
    public AdMobModel adMobModel;
    public LifeModel lifeModel;

    // Start is called before the first frame update
    void Start()
    {
        talkView.talkButton.OnClickAsObservable().Subscribe( _=> talkModel.ShowChatWindow(talkView.chatWindow)).AddTo(this);
        talkView.chatWindow.onEndEdit
            .AsObservable()
            .Where(q => q != "")
            .DistinctUntilChanged()
            .Subscribe(async (q) =>{
                if (lifeModel.totalLife.Value <= 0)
                {
                    shopModel.ShowAdInducationView();
                    return;
                }
                talkModel.ShowAns(await talkModel.gptInstance.ChatGPT(q), talkView.chatPanel);
                talkModel.CleanChatWindow(talkView.chatWindow);
                lifeModel.MinusLife(shopView.freeLifePoint,shopView.paidLifePoint,1); 
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
        shopView.rewardButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(1000)).Subscribe(_ => adMobModel.ShowRewardeAd()).AddTo(this);

        adMobModel.isOnAdLoadedRewardedAd.Subscribe(_=>shopModel.DisableRewardButton(shopView.rewardPanel,adMobModel.isOnAdLoadedRewardedAd.Value)).AddTo(this);
        adMobModel.amountValue.DistinctUntilChanged().Subscribe(amount=>lifeModel.PlusFreeLife(shopView.freeLifePoint,amount));

        lifeModel.SetInitialLife();
        lifeModel.totalLife.Subscribe(_=>{
            lifeModel.SetLife(shopView.yourLifePoint);
            lifeModel.SetLife(talkView.life);
        }).AddTo(this);
        lifeModel.freeLife.Subscribe(_=>lifeModel.SetLife(shopView.freeLifePoint)).AddTo(this);
        lifeModel.totalLife.Subscribe().AddTo(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }   

}
