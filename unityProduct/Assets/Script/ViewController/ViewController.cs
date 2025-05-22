using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    static Dictionary<Type, ViewController> tempUI = new Dictionary<Type, ViewController>();
    public static ViewController GetViewController(Type type)
    {
        return tempUI[type];
    }

    bool isTryOpen = false;
    private object openData = null;
    private void Awake()
    {
        tempUI.Add(GetType(),this);       
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
