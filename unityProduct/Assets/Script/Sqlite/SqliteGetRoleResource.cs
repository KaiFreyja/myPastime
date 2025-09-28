using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using UnityEngine;

public class SqliteGetRoleResource
{
    public void Send(JObject input, Action<APIResult> callback)
    {
        string rid = input["rid"].ToString();

        SqliteHelper sqliteHelper = new SqliteHelper(new string[] { DBConfig.DB_BASE });
        string query = "SELECT * FROM " + DBConfig.TABLE_ROLE_RESOURCE + " rr " + " WHERE " + "rr.rid=" + rid;
        JArray data = sqliteHelper.ExecuteSQL(query);
        for (int i = 0; i < data.Count; i++)
        {
            data[i]["url"] = Config.IMAGE_PATH + data[i]["rid"] + "/" + data[i]["rrid"] + ".png";
        }

        JObject role_resource = new JObject();
        role_resource.Add("data", data);
        JObject result = new JObject();
        result.Add("role_resource", role_resource);

        DataResult dataResult = new DataResult();
        dataResult.json = result;

        callback.Invoke(dataResult);
    }
}
