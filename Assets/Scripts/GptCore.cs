using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GptCore 
{
    static ReactiveDictionary<EMOTIONS, int> emotionData = new ReactiveDictionary<EMOTIONS, int>();

    const string API_URL = "https://api.openai.com/v1/chat/completions";
    private string API_KEY = env.CHAT_GPT_API_KEY;
    const float TEMPERATURE = 0.5f;
    const int MAX_TOKENS = 50;
    const string MODEL = "gpt-3.5-turbo";

    [System.Serializable]
    public class UserMessage
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

    public async UniTask<string> ChatGPT(string content)
    {
        using var request = new UnityWebRequest(API_URL, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");

        UserMessage userMessage = new UserMessage();
        userMessage.role = "user";
        userMessage.content = content;

        requestParam.Add("model", MODEL);
        requestParam.Add("messages", new UserMessage[] { userMessage });
        requestParam.Add("temperature", TEMPERATURE);
        requestParam.Add("max_tokens", MAX_TOKENS);

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

        UserMessage messageResponseParam = JsonConvert.DeserializeObject<UserMessage>(choicesResponseParam[0]["message"].ToString());
        //Debug.Log(messageResponseParam.content);

        string RemoveNewLines(string input)
        {
            return input.Replace("\n", "").Replace("\r", "");
        }

        return RemoveNewLines(messageResponseParam.content);
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