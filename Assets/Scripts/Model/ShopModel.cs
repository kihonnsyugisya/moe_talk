using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
