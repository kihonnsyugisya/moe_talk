using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class GptCore 
{
    static ReactiveDictionary<EMOTIONS, int> emotionData = new ReactiveDictionary<EMOTIONS, int>();

    // const string API_URL = "https://api.openai.com/v1/completions";
    const string API_URL = "https://api.openai.com/v1/chat/completions";
    // 参考URL　https://1-notes.com/implement-chatgpt-model-api/

    private string API_KEY = env.CHAT_GPT_API_KEY;

    public class JsonRequestBody
    {
        public string model;
        public string content;
        public float temperature;
        public int max_tokens;
    }

    public async UniTask<string> ChatGPT(string content)
    {
        var request = new UnityWebRequest(API_URL, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);
        request.SetRequestHeader("Content-Type", "application/json");

        //Dictionary<string, object> requestParam = new Dictionary<string, object>();
        //requestParam.Add("model", "gpt-3.5-turbo");
        //requestParam.Add("prompt", "Hello, how are you?");
        //requestParam.Add("temperature", 0.5f);
        //requestParam.Add("max_tokens", 50);

        JsonRequestBody jsonRequestBody = new JsonRequestBody();
        jsonRequestBody.model = "gpt-3.5-turbo";
        jsonRequestBody.content = content;
        jsonRequestBody.temperature = 0.5f;
        jsonRequestBody.max_tokens = 30;

        string jsonData = JsonUtility.ToJson(jsonRequestBody);

        //string jsonData = JsonUtility.ToJson(requestParam);


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
            Debug.Log("成功");
        }

        return System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

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