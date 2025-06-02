using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exampleViewController : ViewController
{
    public Button btn = null;

    protected override void init()
    {
        base.init();

        btn.onClick.AddListener(onClick);
    }

    protected override void open(object obj)
    {
        base.open(obj);
    }

    public override void close()
    {
        base.close();
    }

    protected override void onTimer()
    {
        base.onTimer();
    }

    private void onClick()
    {
        Debug.Log("onClick");

        ViewController.GetViewController<example2ViewController>().show("‘—‹‹Ž‘—¿");
    }
}
