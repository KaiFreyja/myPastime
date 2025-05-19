using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
#if UNITY_EDITOR
    public static string IP = "http://192.168.0.170";
#else
    public static string IP = "http://35.185.132.13";
#endif
    public static string API_DOMAIN
    {
        get
        {
            return IP + "/api/";
        }
    }
}
