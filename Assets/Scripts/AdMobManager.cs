using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UniRx;


public class AdMobManager : MonoBehaviour
{
    [HideInInspector] public BoolReactiveProperty isOnAdLoadedRewardedAd = new BoolReactiveProperty(false);
    // Start is called before the first frame update
    void Start()
    {
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

    //動画広告同意パネルを表示する。(インスペクタでOnAdLoadedコールバックに設定)
    public void OnRewardedAdLoaded()
    {
        isOnAdLoadedRewardedAd.Value = true;
    }

    //広告読み込みが失敗した時の処理。SDKからstringのパラメータを取得できる。(インスペクタでOnAdFailedToLoadコールバックに設定)
    public void OnRewardAdFailedToLoad(string reason)
    {
        Debug.Log("リワード広告のロードに失敗 ><;" + reason);
    }

    // Rewarded Adオブジェクトの報酬獲得コールバックにセットする(インスペクタでOnUserEarnedRewardコールバックに設定)
    // SDKからRewardクラスのパラメータを取得できる。
    public void OnUserEarnedRewardAd(Reward reward)
    {
        Debug.Log("ユーザはリワードを取得しました。: リワードのタイプ=" +
            reward.Type + ", リワードの量=" + reward.Amount);
        isOnAdLoadedRewardedAd.Value = false;
    }

}
