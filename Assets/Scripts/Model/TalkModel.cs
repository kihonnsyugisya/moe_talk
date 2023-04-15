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

    public GptCore gptInstance;

    public IntReactiveProperty life;

    public void ShowAns(string ans,GameObject ChatPanel)
    {
        ChatPanel.SetActive(true);
        TextMeshProUGUI ansewerText = ChatPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ansewerText.text = ans;
        //Debug.Log("show ans");
    }

    public void CloseAns(GameObject ChatPanel)
    {
        ChatPanel.SetActive(false);
        //Debug.Log("close ans");
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
