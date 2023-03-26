using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

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


    public void ShowAdInducationView()
    {
        CBNativeDialog.Instance.Show(
            title: "ライフが足りません",
            message: "このトークを続けるには1つのライフが必要です。",
            positiveButtonTitle: "ショップを開く",
            positiveButtonAction: () => Debug.Log("ショップを開くメソッド"),
            negativeButtonTitle: "キャンセル",
            negativeButtonAction: () => Debug.Log("dad")
        );
    }

    public void ShowShop(GameObject shopPanel)
    {
        isShowShop.Value = !isShowShop.Value;
        shopPanel.SetActive(isShowShop.Value);
    }


}
