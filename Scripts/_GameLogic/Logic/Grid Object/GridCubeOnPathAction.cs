using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class GridCubeOnPathAction : MonoBehaviour, IPathAction
    {
        #region Private Variables

        private Tween _scaleTween2;
        
        private Tween _scaleTween;

        private Tween _rotationTween;

        #endregion
        
        public void PathAction()
        {
            TweenTheRotatableObjectOnPath();
        }

        private void TweenTheRotatableObjectOnPath()
        {
            KillActiveTween();
            Sequence sequence = PrepareTweenSequence();
            AppendTweenToSequence(sequence);
        }

        private void KillActiveTween()
        {
            if (_scaleTween2 != null && _scaleTween2.IsActive() && _scaleTween != null && _scaleTween.IsActive())
            {
                _scaleTween2.Kill();
                _scaleTween.Kill();
            }
        }

        private Sequence PrepareTweenSequence()
        {
            _scaleTween = transform.DOMoveY(2f, 0.3f).SetLink(gameObject)
                .SetEase(Ease.InBack).SetDelay(3f);
            _scaleTween2 = transform.DOMoveY(0, 0.3f).SetLink(gameObject)
                .SetEase(Ease.InBack);
            _rotationTween = transform.DORotate(new Vector3(360, 0, 0), 0.5f, RotateMode.FastBeyond360);
            
            return DOTween.Sequence();
        }

        private void AppendTweenToSequence(Sequence sequence)
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true);
            
            sequence.Append(_scaleTween);
            sequence.Join(_rotationTween);
            sequence.Append(_scaleTween2);
        }
    }
}