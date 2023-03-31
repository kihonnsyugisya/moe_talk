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

    public IntReactiveProperty life = new();

    public void PlusLife(int value)
    {
        life.Value += value;
    }

    public void MinusLife(int value)
    {
        life.Value -= value;
    }

    public void SetInitialLife()
    {
        //1日一回の処理を入れる
        life.Value = 3;
    }

    public void SetLife(TextMeshProUGUI lifePoint)
    {
        lifePoint.text = "×" + life;
    }


}
