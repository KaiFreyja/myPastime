using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoleListItemController : MonoBehaviour
{
    public Button btn = null;
    public RawImage image = null;
    //public TMPro.TextMeshProUGUI label = null;
    public Text label = null;

    public Action onClick;

    private void Start()
    {
        if (btn != null)
        {
            btn.onClick.AddListener(new UnityEngine.Events.UnityAction(onClick));
        }
    }

    public void SetName(string name)
    {
        if (label != null)
        {
            label.text = name;
        }
    }

    public void SetImage(Texture t)
    {
        if (image != null)
        {
            image.texture = t;
        }
    }
}
