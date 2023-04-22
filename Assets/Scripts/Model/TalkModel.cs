using UnityEngine;
using UniRx;
using TMPro;


public class TalkModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gptInstance = new GptCore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [HideInInspector] public GptCore gptInstance;

    [HideInInspector] public IntReactiveProperty life;

    [HideInInspector] public bool isShowAns = false;

    public void ShowAns(string ans,GameObject ChatPanel)
    {
        if (isShowAns) return;
        ChatPanel.SetActive(true);
        isShowAns = true;
        TextMeshProUGUI ansewerText = ChatPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ansewerText.text = ans;
        ansewerText.pageToDisplay = 1;
    }

    public void NextAns(GameObject ChatPanel)
    {
        TextMeshProUGUI text = ChatPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.pageToDisplay ++;
        if (text.textInfo.pageCount < text.pageToDisplay) ChatPanel.SetActive(false);

    }

    public void CloseAns(GameObject ChatPanel)
    {
        if (!isShowAns) return;
        ChatPanel.SetActive(false);
        isShowAns = false;
    }

    public void ShowChatWindow(TMP_InputField chatWindow)
    {
        chatWindow.Select();
        //Debug.Log("show chatwoidow");

    }

    public void CleanChatWindow(TMP_InputField chatWindow)
    {
        chatWindow.text = "";
        //Debug.Log("clean chat window");
    }

    
}
