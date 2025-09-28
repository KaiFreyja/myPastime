using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
#if UNITY_EDITOR
    public static string IP = "http://192.168.0.170";
#else
    public static string IP = "http://35.185.174.5";
#endif
    public static string API_DOMAIN
    {
        get
        {
            return IP + "/api/";
        }
    }

    public static bool IS_UI_ASSEST_BUNDLE = true;

    /// <summary>
    /// 是否使用sqlite
    /// </summary>
    public const bool IS_SQLITE = true;
    
    public static string DB_PATH
    {
        get
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath + "/DB/";
#else
            return Application.persistentDataPath + "/";
#endif
        }
    }

    /// <summary>
    /// 圖片路徑
    /// </summary>
    public static string IMAGE_PATH
    {
        get
        {
#if UNITY_EDITOR
            return "file:///" + Application.streamingAssetsPath + "/Image/" + "role/";
#else
            return Application.streamingAssetsPath + "/Image/" + "role/";
#endif
        }
    }

    public const string GAMINI_API_KEY = "AIzaSyAbKCvLOF-1eCOl1toM9Y_4ZnDwl81WrAg";
}
