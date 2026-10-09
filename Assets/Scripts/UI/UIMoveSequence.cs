using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CardBattlerCourse.UI
{
    [Serializable]
    public class MoveStep
    {
        public RectTransform target;
        public Vector2 targetAnchoredPosition;
        public float duration = 0.5f;
        public Ease ease = Ease.OutQuad;
        public float delay = 0f;

        [Tooltip("If true, this step plays at the same time as the previous step instead of after it")]
        public bool joinPrevious = false;
    }

    public class UIMoveSequence : MonoBehaviour
    {
        [SerializeField] private List<MoveStep> steps = new();
        [SerializeField] private bool playOnStart = false;
        [SerializeField] private bool useUnscaledTime = false;

        private Vector2 originalPos;

        private Sequence sequence;

        protected virtual void Start()
        {
            //originalPos = GetComponent<RectTransform>().anchoredPosition;

            if (playOnStart)
                Play();
        }

        public void Play()
        {
            //GetComponent<RectTransform>().anchoredPosition = originalPos;  
            sequence?.Kill();
            sequence = DOTween.Sequence();
            sequence.SetUpdate(useUnscaledTime);

            foreach (var step in steps)
            {
                if (step.target == null) continue;

                Tween tween = step.target
                    .DOAnchorPos(step.targetAnchoredPosition, step.duration)
                    .SetEase(step.ease)
                    .SetDelay(step.delay);

                if (step.joinPrevious)
                    sequence.Join(tween);
                else
                    sequence.Append(tween);
            }
        }

        public void Stop()
        {
            sequence?.Kill();
        }

        private void OnDestroy()
        {
            sequence?.Kill(); // avoid the tween trying to animate a destroyed object
        }
    }
}