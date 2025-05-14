using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ImageDownloader : MonoBehaviour
{

    public void StartDownload(string imageUrl,Action<Texture> callBack)
    {
        StartCoroutine(DownloadImage(imageUrl, callBack));
    }

    IEnumerator DownloadImage(string url,Action<Texture> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (callback != null)
            {
                callback.Invoke(texture);
            }
        }
        else
        {
            Debug.LogError("â∫ç⁄ö§ï–é∏îs: " + request.error);
        }
    }
}
