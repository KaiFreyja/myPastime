using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        init(() =>
        {
            Debug.Log("初始化結束");
            if (File.Exists(Application.persistentDataPath + "/" + DBConfig.DB_BASE + ".db"))
            {
                Debug.Log("DB搬動成功");
            }

            //ViewController.GetViewController(typeof(RoleListViewController), (ViewController view) => { view.show(); });
            ViewController.GetViewController<RoleListViewController>().show();
            ViewController.GetViewController<RoleContentViewController>().show();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void init(Action action)
    {
#if UNITY_EDITOR
        action.Invoke();
#else
        if (File.Exists(Application.persistentDataPath + "/info"))
        {
            action?.Invoke();
        }
        else
        {
            copyDB(() =>
            {
                File.Create(Application.persistentDataPath + "/info").Dispose();
                action?.Invoke();
            });
        }
#endif
    }

    private int dbcount = 0;
    private int count = 0;
    private Action dbAction;
    private void copyDB(Action action)
    {
        dbAction = action;
        string[] dbFiles = { DBConfig.DB_BASE + ".db", DBConfig.DB_HISTORY + ".db" };
        dbcount = dbFiles.Length;
        foreach (var db in dbFiles)
        {
            StartCoroutine(CopySingleDB(db));
        }
    }

    IEnumerator CopySingleDB(string dbName)
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, "DB/" + dbName);
        UnityWebRequest uwr = UnityWebRequest.Get(sourcePath);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            string destPath = Path.Combine(Application.persistentDataPath, dbName);
            File.WriteAllBytes(destPath, uwr.downloadHandler.data);
        }

        count++;

        if (dbcount == count)
        {
            dbAction?.Invoke();
        }
    }
}
