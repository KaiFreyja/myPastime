using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class example2ViewController : ViewController
{
    public Text lab = null;

    protected override void init()
    {
        base.init();
    }

    protected override void open(object obj)
    {
        base.open(obj);

        lab.text = obj.ToString();
    }
}
