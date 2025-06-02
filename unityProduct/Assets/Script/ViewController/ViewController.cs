using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ViewController : MonoBehaviour
{
    static GameObject canvas = null;
    static Dictionary<Type, ViewController> tempUI = new Dictionary<Type, ViewController>();

    public static ViewController GetViewController(Type type)
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        if (!tempUI.ContainsKey(type))
        {
            if (Config.IS_UI_ASSEST_BUNDLE)
            {
                loadAssestBundle(type);
            }
            else
            {
                loadResource(type);
            }
        }
        return tempUI[type];
    }

    public static T GetViewController<T>() where T : ViewController
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        Type type = typeof(T);

        if (!tempUI.ContainsKey(type))
        {
            if (Config.IS_UI_ASSEST_BUNDLE)
            {
                loadAssestBundle(type);
            }
            else
            {
                loadResource(type);
            }
        }
        return tempUI[type] as T;
    }


    private static void loadResource(Type type)
    {
        if (tempUI.ContainsKey(type))
            return;

        GameObject viewGO = GameObject.Instantiate(Resources.Load("ui/" + type.ToString())) as GameObject;
        viewGO.transform.SetParent(canvas.transform);
        var rectTransform = viewGO.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1920, 1080);
        rectTransform.anchoredPosition = Vector2.zero;
        tempUI[type] = viewGO.GetComponent<ViewController>();
    }

    private static void loadAssestBundle(Type type)
    {
        if (tempUI.ContainsKey(type))
            return;

        var obj = LoadAssestBundle.Instance.loadAssetBundleSync("ui", type.ToString());
        GameObject viewGO = GameObject.Instantiate(obj) as GameObject;
        viewGO.transform.SetParent(canvas.transform);
        var rectTransform = viewGO.transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1920, 1080);
        rectTransform.anchoredPosition = Vector2.zero;
        tempUI[type] = viewGO.GetComponent<ViewController>();
    }

    bool isTryOpen = false;
    private object openData = null;
    private void Awake()
    {
        if (!tempUI.ContainsKey(GetType()))
        {
            tempUI.Add(GetType(), this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        init();   
    }

    // Update is called once per frame
    void Update()
    {
        if (isTryOpen)
        {
            isTryOpen = false;
            var input = openData;
            openData = null;
            open(input);
        }
        onTimer();
    }

    protected virtual void init()
    {

    }

    protected virtual void open(object obj)
    {

    }

    public void show()
    {
        this.gameObject.SetActive(true);
        isTryOpen = true;
    }

    public void show(object obj)
    {
        openData = obj;
        show();
    }

    public virtual void close()
    {
        this.gameObject.SetActive(false);
    }

    protected virtual void onTimer()
    {
    }
}
