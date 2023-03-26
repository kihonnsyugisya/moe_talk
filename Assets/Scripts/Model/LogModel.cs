using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

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

    public void ShowLog(GameObject logPanel)
    {
        isShowLog.Value = !isShowLog.Value;
        logPanel.SetActive(isShowLog.Value);
    }


    public void GenaleteLogContents(string role,string content,LogView logView)
    {
        GameObject contentsInstance;
        contentsInstance = role == "assistant"
            ? Instantiate(logView.aiContentsPanel, logView.contentsBox)
            : Instantiate(logView.userContentsPanel, logView.contentsBox);

        TextMeshProUGUI contentsText = contentsInstance.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        contentsText.text = content;
        contentsText.rectTransform.sizeDelta = new Vector2(contentsText.rectTransform.sizeDelta.x, contentsText.preferredHeight);
    }

    public BoolReactiveProperty isShowLog = new BoolReactiveProperty(false); 
}
