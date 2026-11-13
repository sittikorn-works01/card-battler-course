using TMPro;
using UnityEngine;

namespace CardBattlerCourse.UI
{
    public class AnimatedTextUI : UIMoveSequence
    {
        [Header("")]
        [SerializeField] private TMP_Text text;

        public void SetText(string newText)
        {
            text.text = newText;
        }

    }
}
