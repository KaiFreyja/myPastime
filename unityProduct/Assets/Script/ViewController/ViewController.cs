using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    static GameObject canvas = null;
    static Dictionary<Type, ViewController> tempUI = new Dictionary<Type, ViewController>();
    public static void GetViewController(Type type, Action<ViewController> callback)
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        if (!tempUI.ContainsKey(type))
        {
            if (Config.IS_UI_ASSEST_BUNDLE)
            {
                LoadAssestBundle.Instance.Load("ui", type.ToString(), (UnityEngine.Object obj) =>
                {
                    if (!tempUI.ContainsKey(type))
                    {
                        GameObject view = (GameObject)GameObject.Instantiate(obj);
                        view.transform.SetParent(canvas.transform);
                        var rectTransform = view.transform.GetComponent<RectTransform>();
                        rectTransform.sizeDelta = new Vector2(1920, 1080);
                        rectTransform.anchoredPosition = Vector2.zero;
                    }
                    callback?.Invoke(tempUI[type]);
                });
                return;
            }
            else
            {
                var view = (GameObject)GameObject.Instantiate(Resources.Load("ui/" + type.ToString()));
                view.transform.SetParent(canvas.transform);
                var rectTransform = view.transform.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(1920, 1080);
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
        callback?.Invoke(tempUI[type]);
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
