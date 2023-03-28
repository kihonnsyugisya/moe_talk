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

    //����L�����Ӄp�l����\������B(�C���X�y�N�^��OnAdLoaded�R�[���o�b�N�ɐݒ�)
    public void OnRewardedAdLoaded()
    {
        isOnAdLoadedRewardedAd.Value = true;
    }

    //�L���ǂݍ��݂����s�������̏����BSDK����string�̃p�����[�^���擾�ł���B(�C���X�y�N�^��OnAdFailedToLoad�R�[���o�b�N�ɐݒ�)
    public void OnRewardAdFailedToLoad(string reason)
    {
        Debug.Log("�����[�h�L���̃��[�h�Ɏ��s ><;" + reason);
    }

    // Rewarded Ad�I�u�W�F�N�g�̕�V�l���R�[���o�b�N�ɃZ�b�g����(�C���X�y�N�^��OnUserEarnedReward�R�[���o�b�N�ɐݒ�)
    // SDK����Reward�N���X�̃p�����[�^���擾�ł���B
    public void OnUserEarnedRewardAd(Reward reward)
    {
        Debug.Log("���[�U�̓����[�h���擾���܂����B: �����[�h�̃^�C�v=" +
            reward.Type + ", �����[�h�̗�=" + reward.Amount);
        isOnAdLoadedRewardedAd.Value = false;
    }

}
