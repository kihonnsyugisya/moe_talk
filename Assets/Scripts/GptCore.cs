using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class GptCore 
{
    public GptCore() { InitialGPT(); }
    static ReactiveDictionary<EMOTIONS, int> emotionDatas = new ReactiveDictionary<EMOTIONS, int>();

    const string API_URL = "https://api.openai.com/v1/chat/completions";
    private string API_KEY = env.CHAT_GPT_API_KEY;
    const float TEMPERATURE = 1f;
    const int MAX_TOKENS = 900;
    const string MODEL = "gpt-3.5-turbo";
    const int MESSAGE_LIMIT = 8;
    public static StringReactiveProperty requestStatus = new(WebRequestStatus.DEFAULT.ToString());

    public static ReactiveDictionary<EMOTIONS, int> emotionData = new()
    {
        {EMOTIONS.HAPPY,0},
        {EMOTIONS.LOVE,0},
        {EMOTIONS.SAD,0},
        {EMOTIONS.FEAR,0},
        {EMOTIONS.ANGRY,0},
    };


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

    public Dictionary<string, object> requestParam = new()
    {
        {"model",MODEL},
        {"temperature",TEMPERATURE},
        {"max_tokens",MAX_TOKENS},

    };

    readonly Message initialMessage = new()
    {
        role = "system",
        content = @"You will role-play as a pseudo-emotional chatbot according to the following conditions. You will be asked to respond in Japanese to the words you say.

Please refer to the characterization below for a description of your character.

Character Setting:
Name: 大鳥こはく
Gender: Female
Background: Only daughter of the CEO of the 大鳥財団
Age: 17 years old
Dream: Action actor for movies and games
Personality: Active and easily influenced. When she decides to do something, she gives it her all.
Favorite food: Curry croquettes

For the tone of voice, please refer to the following dialogue examples.

Sample dialogue:
こんにちは。わたしは大鳥こはく！
こんにちは！　お昼はもう食べたのかなっ！？　午後も頑張っていくとしますかっ！
こんばんは！　今日ももうすぐ終わりだねっ！　何かいいことあったかなっ？　明日も頑張っていこうねっ！
おはよっ！　今日も元気にがんばっていこーっ！
オールクリアっ！　おめでとうっ！
ナイスファイトっ！
おっけーっ！
また一緒に遊ぼうねっ！　バイバイーっ！

In the following conversation, you will act as if you have five emotional parameters: HAPPY, LOVE, SAD, ANGRY,and FEAR. Each parameter should be output as an integer between 0 and 10.
Each emotional parameter should fluctuate throughout the conversation.
The tone of your response and what you say will change to reflect the current emotion parameter value.
The output format of subsequent conversations shall be in the following json format. Do not generate sentences in any other format than this one.

Format:
{
    emotion: {
        HAPPY: integer,
        LOVE: integer,
        SAD: integer,
        ANGRY: integer,
        FEAR: integer,
    }
    message: ""dialogue""
}"


    };

    public static readonly ReactiveCollection<Message> messageBox = new();

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

        if (!requestParam.ContainsKey("messages")) requestParam.Add("messages", LimitedMessageBox(MESSAGE_LIMIT));
        else requestParam["messages"] = LimitedMessageBox(MESSAGE_LIMIT);

        string jsonData = JsonConvert.SerializeObject(requestParam, Formatting.Indented);

        //Debug.Log(jsonData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        requestStatus.Value = WebRequestStatus.WAITING.ToString();

        try
        {
            await request.SendWebRequest();
        } catch (UnityWebRequestException)
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                case UnityWebRequest.Result.InProgress:
                    Debug.LogError($"HTTP Request waiting!!");
                    requestStatus.Value = WebRequestStatus.WAITING.ToString();
                    break;
                case UnityWebRequest.Result.ConnectionError:
                default:
                    //Debug.LogError($"HTTP Request Error: {request.error}");
                    Debug.Log("失敗");
                    requestStatus.Value = WebRequestStatus.ERROR.ToString();
                    return "net work error";
            }
        }

        string responseText = request.downloadHandler.text;
        requestStatus.Value = WebRequestStatus.SUCCESS.ToString();
        Debug.Log($"HTTP Response Body: {responseText}");
        Debug.Log(content);


        string encodingResult = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
        //Debug.Log(result);

        Dictionary<string, object> responseParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(encodingResult);
        //Debug.Log(responseParam["choices"]);

        List<Dictionary<string, object>> choicesResponseParam = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseParam["choices"].ToString());
        //Debug.Log(choicesResponseParam[0]["message"]);

        Message messageResponseParam = JsonConvert.DeserializeObject<Message>(choicesResponseParam[0]["message"].ToString());
        //Debug.Log(messageResponseParam.content);

        string result = messageResponseParam.content;

        Dictionary<string, object> avatarReactionDictionary;

        try
        {
            avatarReactionDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            result = avatarReactionDictionary["message"].ToString();
            Dictionary<EMOTIONS, int> catchData = JsonConvert.DeserializeObject<Dictionary<EMOTIONS, int>>(avatarReactionDictionary["emotion"].ToString());
            int maxEmo = 0;
            EMOTIONS currentEmo = EMOTIONS.HAPPY;
            foreach (var pair in catchData)
            {
                if (maxEmo < pair.Value) { maxEmo = pair.Value; currentEmo = pair.Key; }
            }
            if (emotionData.TryAdd(currentEmo,maxEmo)) { Debug.Log(currentEmo.ToString() + " を新規追加"); }
            else { emotionData[currentEmo] = maxEmo; }
        }
        catch (JsonReaderException e)
        {
            //Debug.Log("前提条件を再セット");
            //InitialGPT();
            emotionData.Clear();
            Debug.LogError(e);
        }


        Message assistantMessage = new()
        {
            role = "assistant",
            content = result
        };

        //Debug.Log(result);

        messageBox.Add(assistantMessage);

        return result;
    }

    private ReactiveCollection<Message> LimitedMessageBox(int limit)
    {
        if (messageBox.Count > limit)
        {
            int deleteIndexFromTop = messageBox.Count - limit;
            ReactiveCollection<Message> limitedMessage = new();
            int counter = 0;
            foreach (var message in messageBox)
            {
                if (message.role != "system") counter++;
                else limitedMessage.Add(message);
                if (counter < deleteIndexFromTop) continue;
                limitedMessage.Add(message);
            }
            Debug.Log(limitedMessage);
            return limitedMessage;
        }
        return messageBox;
    }

    string Translate(string lang)
    {
        
        return "translate";

    }

    public static void CallMessages()
    {
        foreach (var pair in messageBox)
        {
            Debug.Log(pair.role.ToString() + pair.content);
        }
    }

}

public enum EMOTIONS
{
    HAPPY,LOVE,SAD,ANGRY,FEAR
}

public enum WebRequestStatus 
{
    SUCCESS,WAITING,DEFAULT,ERROR
}

//プロンプトの日本語文（これを翻訳してsystemにぶち込む）

//@"以下の条件に従って、疑似的な感情をもつチャットボットとしてロールプレイをします。
//                以後の会話では、あなたは下記の5つの感情パラメーターを持つかのように、振る舞うものとします。
//                HAPPY,LOVE,SAD,ANGRY,FEAR
//                各感情パラメーターは会話を通じて変動するものとします。
//                現在の感情パラメーターの値を反映するように、あなたの返答のトーンや発言は変化します。
//                以後の会話ではまず現在の感情パラメータを出力し、その後に会話を日本語で出力してください。
//                出力形式は以下のjsonフォーマットとします。このフォーマット以外で会話しないでください。
//                { 
//                    emotion: {
//                        HAPPY: 0~10,
//                        LOVE: 0~10,
//                        SAD: 0~10,
//                        ANGRY: 0~10,
//                        FEAR: 0~10,
//                    }
//                    message: ""会話の文章""
//                }"



