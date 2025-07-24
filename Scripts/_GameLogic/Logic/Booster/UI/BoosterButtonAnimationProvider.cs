using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public static class BoosterButtonAnimationProvider
    {
        private static Dictionary<RectTransform, bool> _isAnimating = new Dictionary<RectTransform, bool>();
        private static RectTransform _highlightTransform;
        private static Vector2 _defaultSize;
        private static Sequence _highlightSequence;
        private const float ResetAnimationDuration = 0.1f;

        public static void AnimateHighlight(RectTransform highlightTransform, float highlightScale,
            float highlightScaleSpeed,Transform tutorialHand = null)
        {
            _highlightTransform = highlightTransform;

            if (_isAnimating.TryGetValue(highlightTransform, out var isAnimating) && isAnimating)
                return;

            _isAnimating[highlightTransform] = true;

            _highlightSequence?.Kill(); 
            _highlightSequence = DOTween.Sequence();
            _defaultSize = highlightTransform.sizeDelta;
            Vector2 targetSize = _defaultSize * highlightScale;

            _highlightSequence.Append(highlightTransform.DOSizeDelta(targetSize, highlightScaleSpeed)
                .SetEase(Ease.Linear));
            _highlightSequence
                .Append(highlightTransform.DOSizeDelta(_defaultSize, highlightScaleSpeed).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Yoyo);


            if (tutorialHand != null)
            {
                tutorialHand.gameObject.SetActive(true);
            }
        }
        
        
        public static void StopHighlightAnimation()
        {
            if (_highlightTransform == null)
                return;

            _highlightSequence?.SetLoops(0);
            
            _highlightSequence?.Kill(); 
            
            _highlightTransform.DOSizeDelta(_defaultSize, ResetAnimationDuration).OnComplete(() =>
            {
                _isAnimating[_highlightTransform] = false;

                _highlightTransform = null;
            });
        }
    }
}