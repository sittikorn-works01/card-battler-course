using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CardBattlerCourse.Systems
{
    public class UIBubbleManager : Singleton<UIBubbleManager>
    {
        [SerializeField] private TextMeshPro textPopup;
        [SerializeField] private float moveUpDistance;
        [SerializeField] private float moveUpDuration;
        private DG.Tweening.Sequence textPopupSequence;
        [SerializeField] private float holdTime;
        [SerializeField] private float fadeDuration;

        public void DisplayePopupText(TextType textType, string text, Vector2 position)
        {
            TextMeshPro spawnedText = Instantiate(textPopup, position, Quaternion.identity);            

            switch (textType)
            {
                case TextType.Heal:
                    spawnedText.color = Color.green;
                    spawnedText.text = "+" + text;
                    break;

                case TextType.Damage:
                    spawnedText.color = Color.red;
                    spawnedText.text = "-" + text;
                    break;

                case TextType.Block:
                    spawnedText.color = Color.cyan;
                    spawnedText.text = text;
                    break;
                default:
                    spawnedText.text = text;
                    break;
            }
            
            textPopupSequence = DOTween.Sequence()
                .SetUpdate(false)
                .SetLink(gameObject) // auto-kill if this object gets destroyed mid-animation
                .Append(spawnedText.transform.DOMoveY(position.y + moveUpDistance, moveUpDuration).SetEase(Ease.Unset))
                .Append(spawnedText.transform.DOMoveY(position.y, moveUpDuration).SetEase(Ease.Unset))
                .AppendInterval(holdTime)
                .Append(spawnedText.DOFade(0f, fadeDuration))
                .OnComplete(() =>
                {
                    Destroy(spawnedText.gameObject);
                });
            
        }
    }
}
