using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using System;

public class LogModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void GenaleteLogContents(string role,string content,LogView logView)
    {
        GameObject contentsInstance;
        contentsInstance = role == "assistant"
            ? Instantiate(logView.aiContentsPanel, logView.contentsBox)
            : Instantiate(logView.userContentsPanel, logView.contentsBox);

        TextMeshProUGUI contentsText = contentsInstance.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        contentsText.text = content;
        //float contentBoxHeight = contentsText.rectTransform.sizeDelta.y;
        //contentsText.ForceMeshUpdate();
        //int contentLine = contentsText.textInfo.lineCount;
        //contentsText.rectTransform.sizeDelta = new Vector2(contentsText.rectTransform.sizeDelta.x,contentLine * contentBoxHeight);

        //contentsText.rectTransform.sizeDelta = new Vector2(contentsText.rectTransform.sizeDelta.x, contentsText.preferredHeight);
    }

    public BoolReactiveProperty isShowLog = new BoolReactiveProperty(false); 
}
