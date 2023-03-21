using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GptCore 
{
    public GptCore() { InitialGPT(); }
    static ReactiveDictionary<EMOTIONS, int> emotionData = new ReactiveDictionary<EMOTIONS, int>();

    const string API_URL = "https://api.openai.com/v1/chat/completions";
    private string API_KEY = env.CHAT_GPT_API_KEY;
    const float TEMPERATURE = 0.5f;
    const int MAX_TOKENS = 50;
    const string MODEL = "gpt-3.5-turbo";

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class JsonRequestBodyMessage
    {
        public string[] messages;
    }

    [SerializeField] Dictionary<string, object> requestParam = new Dictionary<string, object>();

    readonly Message initialMessage = new()
    {
        role = "system",
        content = @"以下の条件に従って、疑似的な感情をもつチャットボットとしてロールプレイをします。
                以後の会話では、あなたは下記の4つの感情パラメーターを持つかのように、振る舞うものとします。
                各感情パラメーターは会話を通じて変動するものとします。
                現在の感情パラメーターの値を反映するように、あなたの返答のトーンや発言は変化します。
                以後の会話ではまず現在の感情パラメータを出力し、その後に会話を出力してください。
                出力形式は以下のjsonフォーマットとします。このフォーマット以外で会話しないでください。
                {
                    emotion: {
                        joy: 0~5,
                        fun: 0~5,
                        anger: 0~5,
                        sad: 0~5,
                    }
                    message: ""会話の文章""
                } "
    };

    private readonly List<Message> messageBox = new List<Message>();

    public void InitialGPT()
    {
        messageBox.Add(initialMessage);
    }

    public async UniTask<string> ChatGPT(string content)
    {
        using var request = new UnityWebRequest(API_URL, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");

        Message userMessage = new ()
        {
            role = "user",
            content = content
        };
        messageBox.Add(userMessage);

        requestParam.Add("model", MODEL);
        requestParam.Add("temperature", TEMPERATURE);
        requestParam.Add("max_tokens", MAX_TOKENS);
        requestParam.Add("messages", messageBox);

        string jsonData = JsonConvert.SerializeObject(requestParam, Formatting.Indented);

        //Debug.Log(jsonData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        await request.SendWebRequest();


        // HTTPレスポンスの受信と解析
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"HTTP Request Error: {request.error}");
            Debug.Log("失敗");
            return request.error;
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log($"HTTP Response Body: {responseText}");
            Debug.Log(content);
        }


        string encodingResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
        //Debug.Log(result);

        Dictionary<string, object> responseParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(encodingResult);
        //Debug.Log(responseParam["choices"]);

        List<Dictionary<string, object>> choicesResponseParam = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseParam["choices"].ToString());
        //Debug.Log(choicesResponseParam[0]["message"]);

        Message messageResponseParam = JsonConvert.DeserializeObject<Message>(choicesResponseParam[0]["message"].ToString());
        //Debug.Log(messageResponseParam.content);

        string RemoveNewLines(string input)
        {
            return input.Replace("\n", "").Replace("\r", "");
        }

        string result = RemoveNewLines(messageResponseParam.content);

        Message assistantMessage = new()
        {
            role = "assistant",
            content = result
        };

        //Message[] d = (Message[])requestParam["messages"];
        //Debug.Log(d);

        return result;
    }

    string Translate(string lang)
    {
        
        return "translate";

    }

}

public enum EMOTIONS
{
    HAPPY,LOVE,SAD,ANGRY,FEAR 
}