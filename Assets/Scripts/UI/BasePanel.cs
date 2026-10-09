using UnityEngine;

namespace CardBattlerCourse.UI
{
    public class BasePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup UICanvas;
        public virtual void Open()
        {
            UICanvas.alpha = 1;
            UICanvas.interactable = true;
            UICanvas.blocksRaycasts = true;
        }
        public virtual void Close()
        {
            UICanvas.alpha = 0;
            UICanvas.interactable = false;
            UICanvas.blocksRaycasts = false;
        }
    }
}
