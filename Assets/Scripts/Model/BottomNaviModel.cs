using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    private BoolReactiveProperty isNone = new(true);
    private BoolReactiveProperty isShop = new(false);
    private BoolReactiveProperty isTalk = new(false);
    private BoolReactiveProperty isLog = new(false);

    public enum MODE
    {
        NONE,TALK,LOG,SHOP
    }

    public void ChangeMode(MODE mode)
    {
        switch (mode)
        {
            case MODE.NONE:
                isNone.Value = true;
                isShop.Value = isTalk.Value = isLog.Value = false;
                break;
            case MODE.TALK:
                isTalk.Value = true;
                isShop.Value = isLog.Value = isNone.Value = false;
                break;
            case MODE.LOG:
                isLog.Value = true;
                isShop.Value = isTalk.Value = isNone.Value = false;
                break;
            case MODE.SHOP:
                isShop.Value = true;
                isTalk.Value = isLog.Value = isNone.Value = false;
                break;
        }
    }

    public void ShowControll()
    {

    }



}
