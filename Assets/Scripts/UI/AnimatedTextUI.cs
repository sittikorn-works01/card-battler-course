using TMPro;
using UnityEngine;

namespace CardBattlerCourse.UI
{
    public class AnimatedTextUI : UIMoveSequence
    {
        [Header("")]
        [SerializeField] private TextMeshProUGUI text;

        public void SetText(string newText)
        {
            text.text = newText;
        }

    }
}
