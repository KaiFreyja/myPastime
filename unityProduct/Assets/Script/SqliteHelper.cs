/*
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;
using System.Data.SQLite;

public class SqliteHelper
{
    private string[] dbPaths;
    private SQLiteConnection cnn;

    public SqliteHelper(string[] dbFilenames)
    {
        dbPaths = dbFilenames;
    }

    private void OpenDB()
    {
        string mainDbPath = Path.Combine(Config.DB_PATH, dbPaths[0] + ".db");
        cnn = new SQLiteConnection("Data Source=" + mainDbPath);
        cnn.Open();

        // Attach 其他 DB（如果有的話）
        for (int i = 1; i < dbPaths.Length; i++)
        {
            string attachPath = Path.Combine(Config.DB_PATH, dbPaths[i] + ".db");
            string attachSql = $"ATTACH DATABASE '{attachPath}' AS db{i};";
            using (var attachCmd = cnn.CreateCommand())
            {
                attachCmd.CommandText = attachSql;
                attachCmd.ExecuteNonQuery();
            }
        }
    }

    private void CloseDB()
    {
        cnn?.Close();
        cnn = null;
    }

    public JArray ExecuteSQL(string sql)
    {
        UnityEngine.Debug.Log("ExecuteSQL : " + sql);

        if (cnn == null) OpenDB(); // 自動開啟

        var result = new JArray();
        using (var cmd = cnn.CreateCommand())
        {
            cmd.CommandText = sql;

            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    JObject json = new JObject();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        json.Add(reader.GetName(i), reader.GetValue(i)?.ToString());
                    }
                    result.Add(json);
                }
            }
        }

        CloseDB();

        return result;
    }

    public static void CreateDB(string dbName)
    {
        string path = Path.Combine(Config.DB_PATH, dbName + ".db");

        if (File.Exists(path))
        {
            UnityEngine.Debug.Log($"資料庫已存在：{path}");
            return;
        }

        // 建立空資料庫檔案
        SQLiteConnection.CreateFile(path + ".db");
        UnityEngine.Debug.Log($"成功建立資料庫：{path}");
    }
}
*/


using System;
using System.IO;
using Newtonsoft.Json.Linq;
using SQLitePCL;
using UnityEngine;

public class SqliteHelper
{
    private sqlite3 db;
    private string[] dbPaths;

    public SqliteHelper(string[] dbFilenames)
    {
        dbPaths = dbFilenames;
    }

    /// <summary>
    /// 開啟主 DB 並 Attach 其他 DB
    /// </summary>
    private void OpenDB()
    {
        Batteries_V2.Init(); // 初始化 SQLite native library

        string mainPath = Path.Combine(Config.DB_PATH, dbPaths[0] + ".db");

        // 打開主資料庫
        var rc = raw.sqlite3_open(mainPath, out db);
        if (rc != raw.SQLITE_OK)
        {
            throw new Exception("無法開啟 DB: " + mainPath);
        }

        // Attach 其他 DB
        for (int i = 1; i < dbPaths.Length; i++)
        {
            string attachPath = Path.Combine(Config.DB_PATH, dbPaths[i] + ".db");
            string sql = $"ATTACH DATABASE '{attachPath}' AS db{i};";
            raw.sqlite3_exec(db, sql, null, null, out _);
        }
    }

    /// <summary>
    /// 執行 SQL 並回傳 JArray
    /// </summary>
    public JArray ExecuteSQL(string sql)
    {
        if (db == null) OpenDB();

        JArray result = new JArray();

        sqlite3_stmt stmt;
        var rc = raw.sqlite3_prepare_v2(db, sql, out stmt);
        if (rc != raw.SQLITE_OK)
        {
            Debug.LogError("SQL Prepare 失敗: " + sql);
            return result;
        }

        try
        {
            while (raw.sqlite3_step(stmt) == raw.SQLITE_ROW)
            {
                JObject row = new JObject();
                int colCount = raw.sqlite3_column_count(stmt);
                for (int i = 0; i < colCount; i++)
                {
                    string key = raw.sqlite3_column_name(stmt, i).utf8_to_string();
                    string value = raw.sqlite3_column_text(stmt, i).utf8_to_string() ?? "";
                    row.Add(key, value);
                }
                result.Add(row);
            }
        }
        finally
        {
            raw.sqlite3_finalize(stmt);
        }

        return result;
    }

    /// <summary>
    /// 關閉 DB
    /// </summary>
    public void CloseDB()
    {
        if (db != null)
        {
            raw.sqlite3_close(db);
            db = null;
        }
    }

    /// <summary>
    /// 建立空資料庫
    /// </summary>
    public static void CreateDB(string dbName)
    {
        string path = Path.Combine(Config.DB_PATH, dbName + ".db");

        if (File.Exists(path))
        {
            Debug.Log($"資料庫已存在：{path}");
            return;
        }

        // 用 SQLitePCLRaw 建立空資料庫
        sqlite3 newDb;
        raw.sqlite3_open(path, out newDb);
        raw.sqlite3_close(newDb);

        Debug.Log($"成功建立資料庫：{path}");
    }
}