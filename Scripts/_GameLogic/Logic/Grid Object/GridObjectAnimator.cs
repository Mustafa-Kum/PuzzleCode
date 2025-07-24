using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class GridObjectAnimator
    {
        #region Private Variables

        private const int _heightValue = 2;

        private readonly List<GridObjectType> GridObjectRotationOrder;

        private readonly GridObject currentGridObject;

        private readonly GridTile _gridTile;

        private readonly GridObjectColorData gridObjectColorData;

        private readonly int _currentRotationIndex;

        private static float _totalTweenTime;

        private Sequence _vertexSequence;
        
        private readonly Transform _vertexTransform;
        
        private readonly Transform _rotatableObjectTransform;

        #endregion

        #region Constructors

        public GridObjectAnimator(GridObjectColorData _gridObjectColorData,
            List<GridObjectType> _gridObjectRotationOrder, GridObject _gridObject, int _currentRotationIndex,
            GridTile gridTile, float totalTweenTime, Transform vertexTransform, Transform rotatableObjectTransform)
        {
            gridObjectColorData = _gridObjectColorData;
            GridObjectRotationOrder = _gridObjectRotationOrder;
            currentGridObject = _gridObject;
            this._currentRotationIndex = _currentRotationIndex;
            _gridTile = gridTile;
            _totalTweenTime = totalTweenTime;
            _vertexTransform = vertexTransform;
            _rotatableObjectTransform = rotatableObjectTransform;
        }

        #endregion

        #region Public Methods
        
        public void AscendGridObject()
        {
            ChangeGridTileType();
            
            DisableCollider();

            CreateTweenSequence(_rotatableObjectTransform.DOMoveY(_heightValue, _totalTweenTime / 3),
                    () =>
                    {
                        EventManager.GridEvents.OnGridObjectAscend?.Invoke();
                        RotateToNextOrientation();
                    })
                .SetLink(currentGridObject.gameObject)
                .SetEase(Ease.OutQuad);
        }

        private void RotateToNextOrientation()
        {
            CreateTweenSequence(
                    _rotatableObjectTransform.DORotate(gridObjectColorData.GetNextRotation(_currentRotationIndex),
                        _totalTweenTime / 3),
                    DescendGridObject)
                .SetLink(currentGridObject.gameObject);
        }

        private void DescendGridObject()
        {
            CreateTweenSequence(_rotatableObjectTransform.DOMoveY(0, _totalTweenTime / 3),
                    () =>
                    {
                        EventManager.GridEvents.OnGridObjectDescend?.Invoke();
                        PlayPlacingSound();
                        ScaleObject(OnObjectScaleEnd);
                    })
                .SetLink(currentGridObject.gameObject)
                .SetEase(Ease.OutQuad);
        }
        
        private void ScaleObject(Action completionCallback = null)
        {
            SetGridObjectScaleToOne();
            if(IsGridObjectTypeMatched())
            {
                return;
            }
            CreatePunchScaleSequence();
            DoVertexSequenceAndUpdateCollider(completionCallback);
        }

        private void SetGridObjectScaleToOne() => currentGridObject.transform.localScale = Vector3.one;

        private bool IsGridObjectTypeMatched() => _gridTile.GetGridObjectType() == GridObjectType.Matched;

        private void CreatePunchScaleSequence()
        {
            var _punchScaleSequence = DOTween.Sequence();
            var startScale = currentGridObject.transform.localScale;
            _punchScaleSequence.Append(currentGridObject.transform.DOPunchScale(startScale * 0.2f, 0.3f, 1, 0.5f)).SetEase(Ease.InOutSine);
        }

        private void DoVertexSequenceAndUpdateCollider(Action completionCallback)
        {
            _vertexSequence = CreateVertexSequence(completionCallback);
            UpdateCollider();
        }

        private Sequence CreateVertexSequence(Action completionCallback)
        {
            _vertexSequence = DOTween.Sequence();
            return _vertexSequence.Append(_vertexTransform.DOMoveY(0.02f, 0.3f).OnComplete(completionCallback.Invoke)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        private void UpdateCollider()
        {
            DOVirtual.DelayedCall(0.3f, EnableCollider);
            DOVirtual.DelayedCall(0.3f, KillVertexSequence);
        }

        private void KillVertexSequence() => _vertexSequence.Kill();

        private void OnObjectScaleEnd() => _vertexSequence.Kill();

        private void DisableCollider()
        {
            var collider = currentGridObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
        }

        private void EnableCollider()
        {
            var collider = currentGridObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;
        }

        private Sequence CreateTweenSequence(Tweener tween, TweenCallback onComplete)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(tween).OnComplete(onComplete);
            return sequence;
        }

        private void ChangeGridTileType() => _gridTile.SetGridObjectType(GridObjectRotationOrder[_currentRotationIndex]);

        private void PlayPlacingSound()
        {
            if(_gridTile.GetGridObjectType() == GridObjectType.Matched)
                return;
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Placing, false);
        }

        #endregion
    }
}