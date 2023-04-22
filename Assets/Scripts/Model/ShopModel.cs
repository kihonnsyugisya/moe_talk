using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BoolReactiveProperty isShowShop = new BoolReactiveProperty(false);


    public void ShowAdInducationView(Action showSop)
    {
        CBNativeDialog.Instance.Show(
            title: "ライフが足りません",
            message: "このトークを続けるには1つのライフが必要です。",
            negativeButtonTitle: "キャンセル",
            negativeButtonAction: () => Debug.Log("dad"),
            positiveButtonTitle: "ショップを開く",
            positiveButtonAction: showSop
        );;
    }

    public void DisableRewardButton(GameObject rewardPanel,bool isInteractabl)
    {
        rewardPanel.SetActive(isInteractabl);
    }
}
    