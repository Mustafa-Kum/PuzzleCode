using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Data.Game;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Manager;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridMatcher : MonoBehaviour
    {
        #region Public Variables

        public BoosterDataSO boosterDataSo;
        public PlayerMoveData playerMoveData;
        public GridMatrixController gridMatrixController;
        public GridManager gridManager;
        
        public List<GridTile> matchingTiles;
        public List<GridTile> excludePathTiles;
        
        public float levelCompletionCheckDelay = 0.5f;

        #endregion

        #region Private Variables


        private List<GridTile> _horizontalMatches = new();
        private List<GridTile> _verticalMatches = new();
        private List<GridTile> _newAllVerticalMatches;
        private List<GridTile> _newAllHorizontalMatches;
        private List<GridObject> _gridObjects;

        private const float AnimationDelayPerTile = 0.1f;
        private const float SuccessDelay = 3.5f;
        private const float FailDelay = 1f;
        
        private bool _isProcessingPath;
        private bool _isLastMove;

        private MatrixSearcherQueries _matrixSearcherQueries;
        private PotentialMatchSearcher _potentialMatchSearcher;
        private LevelFailureHandler _levelFailureHandler;
        private LevelSuccessHandler _levelSuccessHandler;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.GridEvents.OnGridObjectClicked += CheckAnyPotentialMatches;
            EventManager.BoosterEvents.BoosterAnimationEnd += CheckAnyPotentialMatches;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnGridObjectClicked -= CheckAnyPotentialMatches;
            EventManager.BoosterEvents.BoosterAnimationEnd -= CheckAnyPotentialMatches;

            foreach (var gridObject in _gridObjects) gridObject.OnGridObjectUpdated -= OnGridObjectUpdated;
        }
        
        private void Start()
        {
            _levelSuccessHandler = new LevelSuccessHandler();
            _levelFailureHandler = new LevelFailureHandler();
            
            _matrixSearcherQueries = new MatrixSearcherQueries(gridMatrixController.Matrix);
            _potentialMatchSearcher = new PotentialMatchSearcher(gridMatrixController.Matrix,
                gridMatrixController.startPoint, gridMatrixController.endPoint, boosterDataSo);

            _gridObjects = _matrixSearcherQueries.GetAllGridObjectsFromMatrix();

            foreach (var gridObject in _gridObjects) gridObject.OnGridObjectUpdated += OnGridObjectUpdated;
        }

        #endregion

        #region Public Methods

        [Button]
        public void FindMatches()
        {
            var gridConfig = gridManager.gridConfig;

            for (var x = 0; x < gridConfig.gridWidth; x++)
            for (var y = 0; y < gridConfig.gridHeight; y++)
            {
                var gridTile = gridConfig.grid[y * gridConfig.gridWidth + x];
                if (gridTile != null) CheckForMatch(gridTile);
            }
        }

        #endregion

        #region Private Methods

        private void CheckForMatch(GridTile gridTile)
        {
            if (FindAndProcessMatches(gridTile))
            {
                //CheckLevelCompletion();
                EventManager.GridEvents.OnGridObjectMatched?.Invoke(matchingTiles);
            }
            else if (playerMoveData.CheckMoveEqualsZero())
            {
                if (_isLastMove) return;
                {
                    _isLastMove = true;
                    //CheckLevelCompletionAfterLastMove();
                }
            }
        }

        private List<GridTile> FindDifferenceAndUpdate(ref List<GridTile> currentMatches, List<GridTile> newMatches)
        {
            var difference = newMatches.Except(currentMatches).ToList();

            currentMatches = newMatches;

            return difference;
        }

        private bool ProcessAndClearMatches(ref List<GridTile> currentMatches, List<GridTile> newMatches)
        {
            var tempMatches = new List<GridTile>(currentMatches);
            var uniqueMatches = FindDifferenceAndUpdate(ref currentMatches, newMatches);
            uniqueMatches = uniqueMatches.Except(tempMatches).ToList();

            var matchesProcessed = InitializeValidMatches(uniqueMatches);

            if (matchesProcessed) currentMatches.Clear();

            return matchesProcessed;
        }

        private bool FindAndProcessHorizontalMatches(GridTile gridTile)
        {
            // Searching for matches in the rightward (horizontal) direction from the current gridTile
            _newAllVerticalMatches = FindMatchesInDirection(gridTile, new Vector2Int(1, 0));

            if (_newAllVerticalMatches.Count <= 1)
                return ProcessAndClearMatches(ref _horizontalMatches, _newAllVerticalMatches);
            if (_newAllVerticalMatches.Contains(gridTile) == false)
                _newAllVerticalMatches.Add(gridTile);

            return ProcessAndClearMatches(ref _horizontalMatches, _newAllVerticalMatches);
        }

        private bool FindAndProcessVerticalMatches(GridTile gridTile)
        {
            // Searching for matches in the upward (vertical) direction from the current gridTile
            _newAllHorizontalMatches = FindMatchesInDirection(gridTile, new Vector2Int(0, 1));

            if (_newAllHorizontalMatches.Count > 1)
                if (_newAllHorizontalMatches.Contains(gridTile) == false)
                    _newAllHorizontalMatches.Add(gridTile);
            return ProcessAndClearMatches(ref _verticalMatches, _newAllHorizontalMatches);
        }

        private bool FindAndProcessMatches(GridTile gridTile)
        {
            var horizontalProcessed = FindAndProcessHorizontalMatches(gridTile);
            var verticalProcessed = FindAndProcessVerticalMatches(gridTile);

            return horizontalProcessed || verticalProcessed || gridTile.GetGridObjectType() == GridObjectType.Matched;
        }

        private bool InitializeValidMatches(List<GridTile> newMatches)
        {
            // Filter out tiles that have already been matched
            var unprocessedMatches = newMatches.Where(tile => !tile.HasBeenMatched).ToList();

            if (unprocessedMatches.Count < 2) return false;
            for (var i = 0; i < unprocessedMatches.Count; i++)
            {
                var i1 = i;
                DOVirtual.DelayedCall(0f * i, () =>
                {
                    if (!unprocessedMatches[i1].HasBeenMatched)
                    {
                        EventManager.GridEvents.GridObjectMatchedType?.Invoke(unprocessedMatches[i1].GetGridObjectType(), unprocessedMatches[i1].transform.position);
                        
                        unprocessedMatches[i1].MatchableAction();
                        unprocessedMatches[i1].HasBeenMatched = true; // Mark this tile as processed
                    }
                });
            }

            return true;
        }

        private void CheckLevelCompletion()
        {
            if (CheckIfThereIsPath()) TryProcessPath();
        }

        private void CheckLevelCompletionAfterLastMove()
        {
            DOVirtual.DelayedCall(levelCompletionCheckDelay, () =>
            {
                if (CheckIfThereIsPath() == false) GameEventService.TriggerLevelFail();
            });
        }

        [Button]
        private bool CheckIfThereIsPath()
        {
            if (gridMatrixController != null)
            {
                var path = gridMatrixController.CheckMatrixIfThereIsAPathFromStartToEnd();

                if (path.Count > 0) return true;
            }

            return false;
        }

        private IEnumerator ProcessTiles()
        {
            const float delay = 0.03f;
            foreach (var t in excludePathTiles)
            {
                yield return new WaitForSeconds(delay);

                t.ExcludePathAction();
            }
        }

        private IEnumerator ProcessPathRoutine(List<GridTile> path)
        {
            const float delay = 0.5f;
            yield return new WaitForSeconds(delay);

            ProcessPathTiles(path, OnPathProcessingComplete);
            PathAction(path);
        }

        private void TryProcessPath()
        {
            var path = gridMatrixController.CheckMatrixIfThereIsAPathFromStartToEnd();
            if (path.Count <= 0 || _isProcessingPath) return;

            excludePathTiles = gridManager.gridConfig.grid.Except(path).ToList();

            StartCoroutine(ProcessTiles());

            _isProcessingPath = true;

            EventManager.GridEvents.OnPathFound?.Invoke(path);
            EventManager.GridEvents.OnExcludePathFound?.Invoke(excludePathTiles);

            StartCoroutine(ProcessPathRoutine(path));
        }

        private void ProcessPathTiles(List<GridTile> path, TweenCallback onCompletion)
        {
            ActivateTilesAndPlaySound(path);
            ScheduleOnCompletionCallback(path.Count, onCompletion);
        }

        private void ActivateTilesAndPlaySound(List<GridTile> path)
        {
            for (var index = 0; index < path.Count; index++)
            {
                var delay = index * AnimationDelayPerTile;
                DOVirtual.DelayedCall(delay, PlaySuccessSound);
            }
        }

        private void PlaySuccessSound()
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true);
        }

        private void ScheduleOnCompletionCallback(int pathCount, TweenCallback onCompletion)
        {
            var totalDelay = pathCount * AnimationDelayPerTile;
            DOVirtual.DelayedCall(totalDelay, onCompletion);
        }

        private void OnPathProcessingComplete()
        {
            _levelSuccessHandler.TriggerLevelSuccess(SuccessDelay);
        }

        private List<GridTile> FindMatchesInDirection(GridTile startTile, Vector2Int direction)
        {
            var matches = new List<GridTile>();
            var current = startTile;
            var next = GetAdjacentGridTile(current, direction);

            while (IsAdjacentMatch(current, next))
            {
                matches.Add(next);
                current = next;
                next = GetAdjacentGridTile(current, direction);
            }

            return matches;
        }

        private bool IsAdjacentMatch(GridTile current, GridTile adjacent)
        {
            if (adjacent == null) return false;

            return adjacent.GetGridObjectType() switch
            {
                GridObjectType.Obstacle => false,
                GridObjectType.Matched => false,
                _ => current.GetGridObjectType() == adjacent.GetGridObjectType()
            };
        }

        private GridTile GetAdjacentGridTile(GridTile gridTile, Vector2Int direction)
        {
            var gridConfig = gridManager.gridConfig;
            var gridPosition = FindGridPosition(gridTile);
            var adjacentPosition = gridPosition + direction;

            if (adjacentPosition.x < 0 || adjacentPosition.x >= gridConfig.gridWidth ||
                adjacentPosition.y < 0 || adjacentPosition.y >= gridConfig.gridHeight)
                return null;

            return gridConfig.grid[adjacentPosition.y * gridConfig.gridWidth + adjacentPosition.x];
        }

        private Vector2Int FindGridPosition(GridTile gridTile)
        {
            var gridConfig = gridManager.gridConfig;

            for (var x = 0; x < gridConfig.gridWidth; x++)
            for (var y = 0; y < gridConfig.gridHeight; y++)
            {
                var tile = gridConfig.grid[y * gridConfig.gridWidth + x];
                if (tile == gridTile) return new Vector2Int(x, y);
            }

            return new Vector2Int(-1, -1);
        }

        private void PathAction(List<GridTile> pathTiles)
        {
            for (var index = 0; index < pathTiles.Count; index++)
            {
                var tile = pathTiles[index];
                DOVirtual.DelayedCall(0.1f * index, () => { tile.PathAction(); });
            }
        }

        [Button]
        private void CheckAnyPotentialMatches()
        {
            DOVirtual.DelayedCall(FailDelay, () =>
            {
                var potentialMatches = _potentialMatchSearcher.HasPotentialMatches();
                if(!potentialMatches) 
                    _levelFailureHandler.TriggerLevelFail(FailDelay);
            });
        }

        private void OnGridObjectUpdated(GridObject gridObject)
        {
            FindMatches();
        }

        #endregion
    }
}