using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleContentImageItemController : MonoBehaviour
{
    public RawImage imageView = null;

    public void setTexture(Texture texture)
    {
        imageView.texture = texture;
    }
}
