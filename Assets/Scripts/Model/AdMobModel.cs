using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

public class AdMobModel : MonoBehaviour
{
    public BoolReactiveProperty isOnAdLoadedRewardedAd = new();
    private RewardedAdGameObject rewardedAd;
    // Start is called before the first frame update
    void Start()
    {
        rewardedAd = MobileAds.Instance.GetAd<RewardedAdGameObject>("Rewarded Ad");
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            // SDK initialization is complete
            Debug.Log("admob init");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRewardedAdLoaded()
    {
        isOnAdLoadedRewardedAd.Value = true;
    }

    public async UniTask OnRewardAdFailedToLoad(string reason)
    {
        Debug.Log("reason" + reason);
        Debug.Log("waiting...");
        await UniTask.Delay(TimeSpan.FromSeconds(15000));
        LoadRewardedAd();
    }

    public void OnUserEarnedRewardAd(Reward reward)
    {
        Debug.Log("type:" +reward.Type + ", =" + reward.Amount);
        isOnAdLoadedRewardedAd.Value = false;
    }

    public void LoadRewardedAd()
    {
        rewardedAd.LoadAd();
        Debug.Log("load rewarded ad");
    }

    public void ShowRewardeAd()
    {
        if (isOnAdLoadedRewardedAd.Value)
        {
            rewardedAd.ShowIfLoaded();
            Debug.Log("show rewarded ad");
        }
    }


}
