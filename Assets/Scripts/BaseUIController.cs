using UnityEngine;

public class BaseUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup UICanvas;
    public void OpenCanvas()
    {
        UICanvas.alpha = 1;
    }
    public void HideCanvas()
    {
        UICanvas.alpha = 0;
    }
}
