using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleListItemController : MonoBehaviour
{
    public RawImage image = null;
    //public TMPro.TextMeshProUGUI label = null;
    public Text label = null;
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
