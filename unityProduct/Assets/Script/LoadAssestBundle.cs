using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;

public class LoadAssestBundle : MonoBehaviour
{
    Dictionary<string, AssetBundle> temp = new Dictionary<string, AssetBundle>();

    List<Item> items = new List<Item>();
    bool isLoading = false;
    class Item
    {
        public string path;
        public string name;
        public Action<UnityEngine.Object> action = null;
    }

    public static LoadAssestBundle Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("LoadAssestBundle");
                instance = go.AddComponent<LoadAssestBundle>();
            }
            return instance;
        }
    }
    private static LoadAssestBundle instance = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (items.Count > 0 && !isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadBundle());
        }*/
    }

    public void Load(string path, string name, Action<UnityEngine.Object> action)
    {
        Item item = new Item();
        item.path = path;
        item.name = name;
        item.action = action;

        foreach (var a in items)
        {
            if (a.path == item.path && a.name == item.name)
            {
                a.action += item.action;
                return;
            }
        }
        items.Add(item);
        if (!isLoading)
        {
            StartCoroutine(LoadBundle());
        }
    }

    IEnumerator LoadBundle()
    {
        isLoading = true;

        while (items.Count > 0)
        {

            Item item = items[0];
            string bundlePath = item.path;
            if (!temp.ContainsKey(bundlePath))
            {
                string parent = string.Empty;
#if UNITY_ANDROID
                parent = Application.streamingAssetsPath + "/" + "android/";
#elif UNITY_IOS
        parent = Application.streamingAssetsPath + "/" + "ios/";
#else
        parent = Application.streamingAssetsPath + "/" + "win/";
#endif

                AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(parent + bundlePath);
                yield return bundleRequest;
                AssetBundle assetBundle = bundleRequest.assetBundle;
                if (assetBundle == null)
                {
                    items.RemoveAt(0);
                    Debug.LogError("Failed to load AssetBundle!");
                    yield break;
                }
                temp.Add(item.path, assetBundle);
            }

            AssetBundle bundle = temp[bundlePath];

            // ç⁄ì¸éëåπÅió·î@ prefabÅj
            AssetBundleRequest request = bundle.LoadAssetAsync(item.name);
            yield return request;

            items.RemoveAt(0);
            try
            {
                item?.action(request.asset);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

        }
        isLoading = false;
    }
}
