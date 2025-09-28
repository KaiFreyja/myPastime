using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class SqliteGetRole
{
    public void Send(JObject input, Action<APIResult> callback)
    {
        SqliteHelper sqliteHelper = new SqliteHelper(new string[] { DBConfig.DB_BASE });
        string query = "SELECT * FROM " + DBConfig.TABLE_ROLE;
        JArray data = sqliteHelper.ExecuteSQL(query);
        for (int i = 0; i < data.Count; i++)
        {
            data[i]["url"] = Config.IMAGE_PATH + data[i]["rid"] + ".png";
        }
        JObject role = new JObject();
        role.Add("data", data);
        JObject result = new JObject();
        result.Add("role", role);

        DataResult dataResult = new DataResult();
        dataResult.json = result;

        callback.Invoke(dataResult);
    }
}
