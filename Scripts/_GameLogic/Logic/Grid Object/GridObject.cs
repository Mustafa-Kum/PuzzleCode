using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Game;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class GridObject : MonoBehaviour, IClickableAction, IMatchableAction, IPathAction
    {
        #region Public Variables

        [SerializeField] private Transform _vertexTransform;
        
        [SerializeField] private GridObjectType _gridObjectType;
        [SerializeField] private GameObject _partical;
        
        public Transform _rotatableObjectTransform;
        public GridObjectType GridObjectType => _gridObjectType;
        
        public GridObjectColorData GridObjectColorData;
        
        public GridTile _gridTile;
        
        public PlayerMoveData playerMoveData;

        public UnityAction<GridObject> OnGridObjectUpdated;
        
        public LevelList_SO levelListSo;
        
        #endregion

        #region Private Variables

        private int _currentRotationIndex;

        private const float totalTweenTime = 0.3f;

        public bool IsMatchBoostActive;

        private bool IsTutorialLevelLockedInput;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            InitializeRotationIndex();
            TutorialLevelInputRestriction();
        }

        private void OnEnable()
        {
            EventManager.BoosterEvents.BoosterUIActivationRequested += OnActivateBoosterPanel;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested += OnDeactivateBoosterPanel;
            EventManager.BoosterEvents.BoosterAnimationEnd += OnAnimationEndCubeAction;
        }

        private void OnDisable()
        {
            EventManager.BoosterEvents.BoosterUIActivationRequested -= OnActivateBoosterPanel;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested -= OnDeactivateBoosterPanel;
            EventManager.BoosterEvents.BoosterAnimationEnd -= OnAnimationEndCubeAction;
        }

        #endregion

        #region Private Methods

        private void TutorialLevelInputRestriction()
        {
            IsTutorialLevelLockedInput = levelListSo.GetCurrentLevelSO().isTutorialLevel;
        }
        
        private void OnActivateBoosterPanel(BoosterType type)
        {
            IsMatchBoostActive = type == BoosterType.GridTileMatcher;
        }

        private void OnDeactivateBoosterPanel(BoosterType arg0)
        {
            IsMatchBoostActive = false;
        }

        private void OnAnimationEndCubeAction()
        {
            IsMatchBoostActive = false;
        }

        private void InitializeRotationIndex()
        {
            for (var i = 0; i < GridObjectColorData.GridObjectRotationOrder.Count; i++)
                if (GridObjectColorData.GridObjectRotationOrder[i] == _gridObjectType)
                {
                    _currentRotationIndex = i;
                    return;
                }
        }
        
        private int CalculateAvailableRotations(int reductionCount)
        {
            return Mathf.Max(0, GridObjectColorData.GridObjectRotationOrder.Count - reductionCount);
        }

        private int UpdateRotationIndex(int availableRotations)
        {
            _currentRotationIndex = (_currentRotationIndex + 1) % availableRotations;
            return _currentRotationIndex;
        }

        private GridObjectAnimator CreateGridObjectAnimator(int currentRotationIndex)
        {
            var gridObjectBehaviour = new GridObjectAnimator(
                GridObjectColorData,
                GridObjectColorData.GridObjectRotationOrder,
                this,
                currentRotationIndex,
                _gridTile,
                totalTweenTime,
                _vertexTransform,
                _rotatableObjectTransform
            );

            return gridObjectBehaviour;
        }

        private void UpdateGridObjectType(int currentRotationIndex)
        {
            _gridObjectType =
                GridObjectColorData.GridObjectRotationOrder[
                    currentRotationIndex % GridObjectColorData.GridObjectRotationOrder.Count];
        }

        private void ScheduleGridObjectUpdatedCallback()
        {
            DOVirtual.DelayedCall(totalTweenTime, GridObjectUpdatedCallback);
        }

        private void GridObjectUpdatedCallback()
        {
            OnGridObjectUpdated?.Invoke(this);
        }
        
        private void ExecuteBoostActive()
        {
            EventManager.BoosterEvents.BoosterCommandExecute?.Invoke(BoosterType.GridTileMatcher, _gridTile.GridPosition);

            if (_partical != null)
                _partical.SetActive(true);
        }

        private void ProcessPlayerMove()
        {
            if (playerMoveData.CanMove())
            {
                playerMoveData.DecreaseCurrentMoves();
                EventManager.UIEvents.OnMoveCounterChanged?.Invoke(playerMoveData.GetCurrentMoves());
                EventManager.GridEvents.OnGridObjectClicked?.Invoke();
                AnimateThisGridObject();
            }
        }

        #endregion

        #region Public Methods

        public void AnimateThisGridObject()
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Click, false);

            var levelIndex = GridObjectColorData.levelIndex;

            var reductionCount = GridObjectColorData.RotationReductionByLevelIndex.GetValueOrDefault(levelIndex, 0);

            var availableRotations = CalculateAvailableRotations(reductionCount);

            if (availableRotations > 0)
            {
                var currentRotationIndex = UpdateRotationIndex(availableRotations);

                var gridObjectBehaviour = CreateGridObjectAnimator(currentRotationIndex);

                gridObjectBehaviour.AscendGridObject();

                UpdateGridObjectType(currentRotationIndex);
            }

            ScheduleGridObjectUpdatedCallback();
        }
        
        public void ClickableActionDown()
        {
            if (IsMatchBoostActive)
            {
                ExecuteBoostActive();
                return;
            }

            ProcessPlayerMove();
        }

        public void ClickableActionHold()
        {
            if (IsMatchBoostActive)
            {
                return;
            }
            
            if (IsTutorialLevelLockedInput)
            {
                return;
            }

            ProcessPlayerMove();
        }
        
        public void ClickableActionUp()
        {
            Debug.Log("Up");
        }

        public void SetGridTile(GridTile grid)
        {
            _gridTile = grid;
        }

        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            gridObjectType = GridObjectType.Matched;
            OnGridObjectUpdated?.Invoke(this);
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            gridObjectType = GridObjectType.Matched;
            OnGridObjectUpdated?.Invoke(this);
        }

        #endregion

        public void PathAction()
        {
        }
    }
}