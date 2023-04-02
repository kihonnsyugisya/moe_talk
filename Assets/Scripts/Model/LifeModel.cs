using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class LifeModel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IntReactiveProperty freeLife = new();
    public IntReactiveProperty paidLife = new();
    public IntReactiveProperty totalLife = new();

    public void PlusFreeLife(TextMeshProUGUI freeLifePoint, int value)
    {
        freeLife.Value += value;
    }

    public void PlusPaidLife(TextMeshProUGUI paidLifePoint, int value)
    {
        paidLife.Value += value;
    }

    public void MinusLife(TextMeshProUGUI freeLifePoint, TextMeshProUGUI paidLifePoint, int value)
    {
        if (freeLife.Value > 0)
        {
            freeLife.Value -= value;
            freeLifePoint.text = freeLife.Value.ToString();
        }
        else if (paidLife.Value > 0)
        {
            paidLife.Value -= value;
            paidLifePoint.text = paidLife.Value.ToString();
        }
    }

    public void SetInitialLife()
    {
        //1日一回の処理を入れる
        freeLife.Value = 3;
    }

    public void SetViewLife(TextMeshProUGUI lifeView,int value)
    {
        totalLife.Value = freeLife.Value + paidLife.Value;
        lifeView.text = "×" + value;
    }





}
