using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class ClientAiTalk
{
    public void Send(JObject input, Action<APIResult> callBack)
    {
        string uid = input["uid"] == null ? "" : input["uid"].ToString();
        string tgid = input["tgid"] == null ? "" : input["tgid"].ToString();
        string ask = input["ask"] == null ? "" : input["ask"].ToString();

        if (string.IsNullOrEmpty(uid))
        {
            Debug.Log("uid不存在");
        }

        if (!string.IsNullOrEmpty(tgid))
        {
            //檢查tgid(talk group 是否存在)
            SqliteHelper sqliteHelper = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
            string query = "SELECT * FROM " + DBConfig.TABLE_TALK_GROUP + " tg " + " WHERE " + "tg.tgid=" + tgid;
            JArray data = sqliteHelper.ExecuteSQL(query);
            if (data.Count == 0)
            {
                Debug.Log("tgid " + tgid + " 不存在");
            }
        }

        if (string.IsNullOrEmpty(tgid))
        {
            //新增tgid (Talk group)
            SqliteHelper sqliteHelper = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
            string query = "INSERT INTO "+ DBConfig.TABLE_TALK_GROUP +" (uid,name,description) VALUES (" + uid + ", '" + ask.Substring(0, ask.Length > 30 ? 30 : ask.Length) + "' , '" + ask.Substring(0, ask.Length > 250 ? 250 : ask.Length) + "' )";
            sqliteHelper.ExecuteSQL(query);

            sqliteHelper = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
            query = "SELECT * FROM " + DBConfig.TABLE_TALK_GROUP + " ORDER BY tgid DESC LIMIT 1";
            var result = sqliteHelper.ExecuteSQL(query);
            if (result.Count > 0)
            {
                tgid = result[0]["tgid"].ToString();
            }
        }

        //取得talk content筆數
        SqliteHelper sqliteHelper1 = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
        string query1 = "SELECT * " + " FROM " + DBConfig.TABLE_TALK_CONTENT + " tc " + " WHERE " + "tc.tgid=" + tgid;
        var countent = sqliteHelper1.ExecuteSQL(query1);
        int total = countent.Count;

        //新增talk content user ask
        sqliteHelper1 = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
        query1 = "INSERT INTO " + DBConfig.TABLE_TALK_CONTENT + "(tgid,content,talker,seq)" + "VALUES (" + tgid + ", '" + ask + "'" + "," + "'user'" + "," + (total + 1) + ")";
        sqliteHelper1.ExecuteSQL(query1);

        //取得對話歷史紀錄
        sqliteHelper1 = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
        query1 = "SELECT * " + " FROM " + DBConfig.TABLE_TALK_CONTENT + " WHERE " + "tgid=" + tgid + " ORDER BY seq";
        var res = sqliteHelper1.ExecuteSQL(query1);

        //string ans = "測試測試測試";
        talkAsk(res,(string ans) =>
        {
            //新增回答紀錄
            sqliteHelper1 = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
            query1 = "INSERT INTO " + DBConfig.TABLE_TALK_CONTENT + "(tgid,content,talker,seq)" + "VALUES (" + tgid + ", '" + ans + "'" + "," + "'model'" + "," + (total + 2) + ")";
            sqliteHelper1.ExecuteSQL(query1);

            //取得剛新增的紀錄
            sqliteHelper1 = new SqliteHelper(new string[] { DBConfig.DB_HISTORY });
            query1 = "SELECT * FROM " + DBConfig.TABLE_TALK_CONTENT + " ORDER BY tcid DESC LIMIT 1";
            res = sqliteHelper1.ExecuteSQL(query1);

            JObject talk_group_ask = new JObject();
            talk_group_ask["talk_group_ask"] = res[0];
            DataResult dataResult = new DataResult();
            dataResult.json = talk_group_ask;
            callBack.Invoke(dataResult);

        });
    }

    private void talkAsk(JArray input,Action<string> callback)
    {
        string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=" + Config.GAMINI_API_KEY;

        JArray contents = new JArray();
        foreach (var item in input)
        {
            string role = item["talker"].ToString() == "user" ? "user" : "model";
            string text = item["content"].ToString();
            JObject content = new JObject();
            content.Add("role", role);
            JObject textJobj = new JObject();
            textJobj.Add("text", text);
            JArray textJarray = new JArray();
            textJarray.Add(textJobj);
            content.Add("parts", textJarray);

            contents.Add(content);
        }

        JObject body = new JObject();
        body.Add("contents", contents);

        GameObject g = new GameObject(url);
        UnityEngine.Object.DontDestroyOnLoad(g);
        APIClient client = g.AddComponent<APIClient>();
        client.SendPOST(url, body, (APIResult result)=>
        {
            Debug.Log(result.getData());
            var data = result.getData();
            string res = data["candidates"][0]["content"]["parts"][0]["text"].ToString();

            if (callback != null)
            {
                callback.Invoke(res);
            }

        });
    }
}
