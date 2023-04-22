using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using UniRx.Triggers;

public class Presenter : MonoBehaviour
{
    public TalkView talkView;
    public TalkModel talkModel;
    public LogView logView;
    public AvatarView avatarView;
    public LogModel logModel;
    public ShopView shopView;
    public ShopModel shopModel;
    public AdMobModel adMobModel;
    public LifeModel lifeModel;
    public IapModel iapModel;
    public BottomNaviModel bottomNaviModel;
    public AvatarModel avatarModel;
    public UserDataModel userDataModel;

    // Start is called before the first frame update
    void Start()
    {
        talkView.talkButton.OnClickAsObservable().Subscribe(_ => { talkModel.ShowChatWindow(talkView.chatWindow); bottomNaviModel.ChangeMode(BottomNaviModel.MODE.TALK); }).AddTo(this);
        talkView.chatWindow.onEndEdit
            .AsObservable()
            .Where(q => q != "")
            .DistinctUntilChanged()
            .Subscribe(async (q) => {
                if (lifeModel.totalLife.Value <= 0)
                {
                    shopModel.ShowAdInducationView(()=>bottomNaviModel.ChangeMode(BottomNaviModel.MODE.SHOP));
                    return;
                }
                talkModel.ShowAns(await talkModel.gptInstance.ChatGPT(q), talkView.chatPanel);
                talkModel.CleanChatWindow(talkView.chatWindow);
                if(GptCore.requestStatus.Value != WebRequestStatus.ERROR.ToString()) lifeModel.MinusLife(shopView.freeLifePoint, shopView.paidLifePoint, 1);
            }).AddTo(this);
        talkView.nextButton.OnClickAsObservable().Subscribe(_ => talkModel.NextAns(talkView.chatPanel)).AddTo(this);
        talkView.chatWindow.onDeselect.AddListener(_ => bottomNaviModel.ChangeMode(BottomNaviModel.MODE.NONE));

        GptCore.messageBox
            .ObserveAdd()
            .Where(message => message.Value.role != "system")
            .Subscribe(message => logModel.GenaleteLogContents(message.Value.role, message.Value.content, logView))
            .AddTo(this);
        logView.logButton.OnClickAsObservable().Subscribe(_ => bottomNaviModel.ChangeMode(BottomNaviModel.MODE.LOG)).AddTo(this);

        GptCore.emotionData
            .ObserveReplace()
            .Subscribe(pair => {
                avatarView.animator.SetLayerWeight(1, pair.NewValue / 10f);
                avatarView.animator.Play(avatarView.TranslateEmoToFaceState(pair.Key));
                avatarView.animator.SetTrigger(pair.Key.ToString());
            })
            .AddTo(this);

        GptCore.emotionData
            .ObserveReset()
            .Subscribe(_=>avatarView.ResetFace())
            .AddTo(this);

        GptCore.requestStatus.DistinctUntilChanged().Subscribe(status => 
        {
            if (status == WebRequestStatus.WAITING.ToString())
            {
                talkView.chatWindow.readOnly = true;
                avatarView.animator.SetBool("THINKING", true);
            }
            else if (status == WebRequestStatus.ERROR.ToString())
            {
                talkView.chatWindow.readOnly = false;
                avatarView.animator.SetTrigger("SAD");
                avatarView.animator.SetBool("THINKING", false);
            }
            else {
                talkView.chatWindow.readOnly = false;
                avatarView.animator.SetBool("THINKING", false);
            }
        }).AddTo(this);

        shopView.shopButton.OnClickAsObservable().Subscribe(_ => bottomNaviModel.ChangeMode(BottomNaviModel.MODE.SHOP)).AddTo(this);
        shopView.rewardButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(1000)).Subscribe(_ => adMobModel.ShowRewardeAd()).AddTo(this);
        adMobModel.isOnAdLoadedRewardedAd.Subscribe(value => {
            bottomNaviModel.ShowPanel(shopView.rewardPanel,value);
            shopModel.DisableRewardButton(shopView.rewardPanel, adMobModel.isOnAdLoadedRewardedAd.Value);
        }).AddTo(this);
        adMobModel.amountValue.Subscribe(amount => lifeModel.PlusPaidLife(shopView.freeLifePoint, amount));

        lifeModel.totalLife.Subscribe(x =>
        {
            lifeModel.SetViewLife(shopView.yourLifePoint, x);
            lifeModel.SetViewLife(talkView.life, x);
        }).AddTo(this);
        lifeModel.freeLife.SkipLatestValueOnSubscribe().Subscribe(x =>
        {
            lifeModel.SetViewLife(shopView.freeLifePoint, x);
            userDataModel.SaveLife(PREFES_KEY.FREE_LIFE,x);
        }).AddTo(this);
        lifeModel.paidLife.SkipLatestValueOnSubscribe().Subscribe(x =>
        {
            lifeModel.SetViewLife(shopView.paidLifePoint, x);
            userDataModel.SaveLife(PREFES_KEY.PAID_LIFE,x);
        }).AddTo(this);
        lifeModel.SetInitialLife(userDataModel.judge_type);

        if (PlayerPrefs.HasKey(PREFES_KEY.FREE_LIFE.ToString())) lifeModel.freeLife.Value = userDataModel.LoadLife(PREFES_KEY.FREE_LIFE);
        if (PlayerPrefs.HasKey(PREFES_KEY.PAID_LIFE.ToString())) lifeModel.paidLife.Value = userDataModel.LoadLife(PREFES_KEY.PAID_LIFE);

        iapModel.amount.Subscribe(amount => lifeModel.PlusPaidLife(shopView.paidLifePoint, amount)).AddTo(this);

        bottomNaviModel.isNone.Subscribe(_ => bottomNaviModel.SelectColorControll(shopView.shopButton, shopView.bottomText, false)).AddTo(this);
        bottomNaviModel.isShop.Subscribe(value => { bottomNaviModel.ShowPanel(shopView.shopPanel, value); bottomNaviModel.SelectColorControll(shopView.shopButton, shopView.bottomText, value); talkModel.CloseAns(talkView.chatPanel); Debug.Log("1"); }).AddTo(this);
        bottomNaviModel.isTalk.Subscribe(value => { bottomNaviModel.SelectColorControll(talkView.talkButton, talkView.bottomText, value); talkModel.CloseAns(talkView.chatPanel); Debug.Log("2"); }).AddTo(this);
        bottomNaviModel.isLog.Subscribe(value => { bottomNaviModel.ShowPanel(logView.logPanel, value); bottomNaviModel.SelectColorControll(logView.logButton, logView.bottomText, value); talkModel.CloseAns(talkView.chatPanel); Debug.Log("3"); }).AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => Input.GetKey(KeyCode.Escape))
            .Subscribe(_ =>
            {
                userDataModel.ResetPrefes();
            });
    }

    // Update is called once per frame
    void Update()   
    {
    }   

}
