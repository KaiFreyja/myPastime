using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    public static string IP = "http://35.185.132.13";

    public static string API_DOMAIN
    {
        get
        {
            return IP + "/api/";
        }
    }
}
