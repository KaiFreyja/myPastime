using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class APIController
{
    public void GetFgoGetRole(JObject input, Action<APIResult> callBack)
    {
        sendAPI("role", input, callBack);
    }

    public void GetFgoRoleResource(JObject input,Action<APIResult> callBack)
    {
        sendAPI("role_resource", input, callBack);
    }

    public void AskAiTalk(JObject input, Action<APIResult> callBack)
    {
        postAPI("talk_group_ask", input, callBack);
    }


    private void sendAPI(string url,JObject jobj,Action<APIResult> callBack)
    {
        GameObject g = new GameObject(url);
        UnityEngine.Object.DontDestroyOnLoad(g);
        APIClient client = g.AddComponent<APIClient>();
        client.SendAPI(Config.API_DOMAIN + url, jobj, callBack);
    }

    private void postAPI(string url, JObject jobj, Action<APIResult> callBack)
    {
        GameObject g = new GameObject(url);
        UnityEngine.Object.DontDestroyOnLoad(g);
        APIClient client = g.AddComponent<APIClient>();
        client.SendPOST(Config.API_DOMAIN + url, jobj, callBack);
    }
}

public interface APIResult
{    
    JObject getData();
}
