using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class APIClient : MonoBehaviour
{
    private String url = "";
    private JObject input = new JObject();
    private Action<APIResult> callBack;

    public void SendAPI(string url,JObject input,Action<APIResult> action)
    {
        this.url = url;
        this.input = input;
        this.callBack = action;
        StartCoroutine(GetData());
    }

    public void SendPOST(string url, JObject input, Action<APIResult> action)
    {
        this.url = url;
        this.input = input;
        this.callBack = action;
        StartCoroutine(PostData());
    }

    IEnumerator GetData()
    {
        string p = string.Empty;
        foreach (var a in input)
        {
            p += string.IsNullOrEmpty(p) ? "?" : "&";
            p += a.Key + "=" + a.Value;
        }
        string url = this.url + p;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string text = request.downloadHandler.text;
            JObject jobj = JObject.Parse(text);
            if (callBack != null)
            {
                DataResult result = new DataResult();
                result.json = jobj;
                callBack.Invoke(result);
            }
        }
        else
        {
            if (callBack != null)
            {
                DataResult result = new DataResult();
                result.json = new JObject();
                callBack.Invoke(result);
            }
            Debug.LogError("Error: " + request.error);
        }

        Destroy(this.gameObject);
    }

    IEnumerator PostData()
    {
        UnityWebRequest request = new UnityWebRequest(this.url, "POST");

        // 設定 JSON 傳送內容
        string jsonStr = input.ToString();
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string text = request.downloadHandler.text;
            JObject jobj = JObject.Parse(text);
            callBack?.Invoke(new DataResult { json = jobj });
        }
        else
        {
            callBack?.Invoke(new DataResult { json = new JObject() });
            Debug.LogError("POST Error: " + request.error);
        }

        Destroy(this.gameObject);
    }
}

public class DataResult : APIResult
{
    public JObject json = new JObject();
    public JObject getData()
    {
        return json;
    }
}