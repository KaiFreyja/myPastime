using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LoadAssestBundle : MonoBehaviour
{
    private string parent
    {
        get
        {
            String parent = string.Empty;

#if UNITY_ANDROID
            parent = Application.streamingAssetsPath + "/" + "android/";
#elif UNITY_IOS
        parent = Application.streamingAssetsPath + "/" + "ios/";
#else
        parent = Application.streamingAssetsPath + "/" + "win/";
#endif
            return parent;
        }
    }

    Dictionary<string, AssetBundle> bundleTemp = new Dictionary<string, AssetBundle>();

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
            if (!bundleTemp.ContainsKey(bundlePath))
            {
                AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(parent + bundlePath);
                yield return bundleRequest;
                AssetBundle assetBundle = bundleRequest.assetBundle;
                if (assetBundle == null)
                {
                    items.RemoveAt(0);
                    Debug.LogError("Failed to load AssetBundle!");
                    yield break;
                }
                bundleTemp.Add(item.path, assetBundle);
            }

            AssetBundle bundle = bundleTemp[bundlePath];

            // 載入資源（例如 prefab）
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

    public UnityEngine.Object loadAssetBundleSync(string path ,string name)
    {
        AssetBundle bundle = null;
        if (!bundleTemp.ContainsKey(path))
        {
            bundle = AssetBundle.LoadFromFile(parent + path);
            if (bundle == null)
            {
                Debug.LogError("Failed to load AssetBundle from path: " + path);
                return null;
            }
            bundleTemp.Add(path, bundle);
        }
        else
        {
            bundle = bundleTemp[path];
        }
        var asset = bundle.LoadAsset(name);
        //bundle.Unload(false); // 注意：如果要持續使用資源，這裡不要卸載或要保持bundle引用
        return asset;
    }
}
