package com.kai.kaiproductanndroid;
import com.Tools.API.BaseAPICallBack;
import com.Tools.API.FetchJsonTask;
import com.Tools.API.PostJsonTask;
import com.Tools.mLog;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Iterator;

public class APIController
{
    public void GetKaiTest(JSONObject input, BaseAPICallBack callBack) {
        sendAPI("kai_test", input, callBack);
    }

    public void GetFgoGetRole(JSONObject input, BaseAPICallBack callBack) {
        sendAPI("role", input, callBack);
    }

    public void GetFgoGetGender(JSONObject input, BaseAPICallBack callBack) {
        sendAPI("gender", input, callBack);
    }

    public void GetFgoGetProfession(JSONObject input, BaseAPICallBack callBack) {
        sendAPI("profession", input, callBack);
    }

    public void GetFgoRoleLevelAttr(JSONObject input, BaseAPICallBack callBack)
    {
        sendAPI("level_attr",input,callBack);
    }

    public void GetFgoProfessionAtkRate(JSONObject input,BaseAPICallBack callBack)
    {
        sendAPI("profession_atk_rate",input,callBack);
    }

    public void  GetFgoMapPos(JSONObject input,BaseAPICallBack callBack)
    {
        sendAPI("map_pos",input,callBack);
    }

    public void GetAiTalkGroup(JSONObject input,BaseAPICallBack callBack)
    {
        sendAPI("talk_group",input,callBack);
    }

    public void GetAiTalkContent(JSONObject input,BaseAPICallBack callBack)
    {
        sendAPI("talk_content",input,callBack);
    }

    public void AskAiTalk(JSONObject input,BaseAPICallBack callBack)
    {
        postAPI("talk_group_ask",input,callBack);
    }

    private void sendAPI(String url,JSONObject input,BaseAPICallBack callBack) {
        String p = "";
        if(input.length() > 0) {
            for (Iterator<String> it = input.keys(); it.hasNext(); ) {
                String key = it.next();
                String value = null;
                try {
                    value = input.getString(key);
                } catch (JSONException e) {
                    throw new RuntimeException(e);
                }
                if(!p.isEmpty())
                {
                    p += "&";
                }
                else
                {
                    p += "?";
                }
                p += key + "=" +value;
            }
        }
        new FetchJsonTask(config.API_DOMAIN + url + p, callBack).execute();
    }

    private void postAPI(String url,JSONObject input,BaseAPICallBack callBack)
    {
        new PostJsonTask(config.API_DOMAIN + url,input, callBack).execute();
    }
}
