using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using DG.Tweening;
using Handler.Extensions;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class GridObjectScaleAndPlayParticleOnMatch : MonoBehaviour, IMatchableAction
    {
        #region Private Variables

        private Action _OnScaleTweenEnded { get; set; }
        private Action _OnEndGameTweenEnded { get; set; }
        private Sequence _scaleSequence;
        private Sequence _endGameSequence;

        private const float _scalingFactor = 1.2f;
        private const float _duration = 0.25f;
        
        #endregion

        #region Inspector Variables

        [SerializeField] private ParticleCallbackProvider _particleCallbackProvider;
        [SerializeField] private ObjectSpawnProvider _objectSpawnProvider;
        [SerializeField] private Transform _mathedObjectParent;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            _scaleSequence = DOTween.Sequence();
        }

        #endregion

        #region Public Methods

        public void MatchAction(GridTile _gridTile, GridObjectType gridObjectType)
        {
            AddOnEndCallback(gridObjectType);
            ExecuteMatchingScaleSequence(true);
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            AddParticalCallback(gridObjectType);

            var objectToAnimation = transform;

            if (gridObjectType == GridObjectType.Matched)
                objectToAnimation = _mathedObjectParent;
            
            ExecuteMatchFoundSequence(true, objectToAnimation);
        }

        #endregion
        
        #region Private Methods
        
        private void ExecuteMatchingScaleSequence(bool _callEndCallback)
        {
            _scaleSequence = DOTween.Sequence();
            
            _scaleSequence.Append(ScaleInWorldSpaceSequence(transform, _scalingFactor * Vector3.one, _duration))
                .Append(ScaleInWorldSpaceSequence(transform, Vector3.zero, _duration)).AppendCallback(() =>
                {
                    if (_callEndCallback)
                    {
                        _OnScaleTweenEnded?.Invoke();
                        EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true);
                    }
                }).SetLink(gameObject);
        }

        private void ExecuteMatchFoundSequence(bool _callEndCallback, Transform objectToAnimate)
        {
            if (_endGameSequence == null || !_endGameSequence.IsActive() || _endGameSequence.IsComplete())
            {
                _endGameSequence = DOTween.Sequence();
            }
            
            _endGameSequence.Append(objectToAnimate.DOMoveY(0.7f, 0.3f)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _endGameSequence.Append(ScaleInWorldSpaceSequence(objectToAnimate, _scalingFactor * Vector3.one, 0.3f)
                .SetEase(Ease.OutBounce)).SetLink(gameObject);
            
            _endGameSequence.Append(ScaleInWorldSpaceSequence(objectToAnimate, Vector3.zero, 0.3f)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _endGameSequence.AppendCallback(() =>
            {
                if (_callEndCallback)
                {
                    _OnEndGameTweenEnded?.Invoke();
                    EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true);
                }
            });
        }
        
        private void AddOnEndCallback(GridObjectType type)
        {
            _OnScaleTweenEnded += () =>
            {
                _particleCallbackProvider.PlayParticles(type);
                //_objectSpawnProvider.ObjectSpawnAnimation();
            };
        }
        
        private void AddParticalCallback(GridObjectType type)
        {
            _OnEndGameTweenEnded += () =>
            {
                _particleCallbackProvider.PlayParticles(type);
            };
        }

        private Tween ScaleInWorldSpaceSequence(Transform target, Vector3 targetWorldScale, float duration)
        {
            Vector3 currentWorldScale = target.lossyScale;
            Vector3 normalizedScaleChange = CalculateNormalizedScaleChange(currentWorldScale, targetWorldScale);

            Vector3 localScaleChange = target.InverseTransformVector(normalizedScaleChange);
            Vector3 targetLocalScale = target.localScale.MultipliedBy(localScaleChange);
            Vector3 absoluteTargetLocalScale = CalculateAbsoluteTargetLocalScale(targetLocalScale);

            return DOTween.To(() => target.localScale, x => target.localScale = x, absoluteTargetLocalScale, duration)
                .SetEase(Ease.InOutCirc)
                .SetLink(target.gameObject);
        }

        private Vector3 CalculateNormalizedScaleChange(Vector3 currentScale, Vector3 targetScale)
        {
            return new Vector3(
                NormalizeScale(currentScale.x, targetScale.x),
                NormalizeScale(currentScale.y, targetScale.y),
                NormalizeScale(currentScale.z, targetScale.z)
            );
        }

        private float NormalizeScale(float current, float target)
        {
            return current != 0 ? target / current : 1.0f;
        }

        private Vector3 CalculateAbsoluteTargetLocalScale(Vector3 targetLocalScale)
        {
            return new Vector3(
                Mathf.Abs(targetLocalScale.x),
                Mathf.Abs(targetLocalScale.y),
                Mathf.Abs(targetLocalScale.z)
            );
        }
        
        #endregion
    }
}