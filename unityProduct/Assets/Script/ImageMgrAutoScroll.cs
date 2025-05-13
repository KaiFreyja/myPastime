using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMgrAutoScroll : MonoBehaviour
{
    public ScrollRect scroll = null;

    private bool isWaiting = true;
    private int targetIndex = 0;
    private float waitTimer = 0f;
    private float moveTimer = 0f;

    private float fromValue = 0f;
    private float toValue = 0f;

    private const float waitDuration = 3f;
    private const float moveDuration = 0.5f;

    private int visibleCount = 1;

    void Start()
    {
        UpdateVisibleCount();
    }

    void Update()
    {
        if (scroll == null || scroll.content == null) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitDuration)
            {
                waitTimer = 0f;
                isWaiting = false;

                UpdateVisibleCount();
                targetIndex = (targetIndex + 1) % visibleCount;

                fromValue = scroll.horizontalNormalizedPosition;
                toValue = (visibleCount <= 1) ? 0f : (float)targetIndex / (visibleCount - 1);

                moveTimer = 0f;
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / moveDuration);
            scroll.horizontalNormalizedPosition = Mathf.Lerp(fromValue, toValue, t);

            if (t >= 1f)
            {
                isWaiting = true;
            }
        }
    }

    void UpdateVisibleCount()
    {
        visibleCount = 0;
        foreach (Transform child in scroll.content)
        {
            if (child.gameObject.activeSelf)
                visibleCount++;
        }

        if (visibleCount == 0) visibleCount = 1; // ñhé~èúà»0
    }
}
