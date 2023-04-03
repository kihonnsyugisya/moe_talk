using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class BottomNaviModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [HideInInspector] public BoolReactiveProperty isNone = new(true);
    [HideInInspector] public BoolReactiveProperty isShop = new(false);
    [HideInInspector] public BoolReactiveProperty isTalk = new(false);
    [HideInInspector] public BoolReactiveProperty isLog = new(false);

    public enum MODE
    {
        NONE,TALK,LOG,SHOP
    }

    public void ChangeMode(MODE mode)
    {
        switch (mode)
        {
            case MODE.NONE:
                isNone.Value = !isNone.Value;
                isShop.Value = isTalk.Value = isLog.Value = false;
                break;
            case MODE.TALK:
                isTalk.Value = !isTalk.Value;
                isShop.Value = isLog.Value = isNone.Value = false;
                break;
            case MODE.LOG:
                isLog.Value = !isLog.Value;
                isShop.Value = isTalk.Value = isNone.Value = false;
                break;
            case MODE.SHOP:
                isShop.Value = !isShop.Value;
                isTalk.Value = isLog.Value = isNone.Value = false;
                break;
        }
    }

    public void ShowPanel(GameObject panel, bool isInteractabl)
    {
        panel.SetActive(isInteractabl);
    }

    public void SelectColorControll(Button button,TextMeshProUGUI text,bool isActive)
    {
        button.image.color = text.color = isActive ? SelectColors.themeColor : SelectColors.defaultButtonColor ;
    }



}
