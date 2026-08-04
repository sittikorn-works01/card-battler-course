using System;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public virtual void Open()
    {
        canvasGroup.alpha = 1.0f;
    }
    public virtual void Close()
    {
        canvasGroup.alpha = 0f;
    }
}
