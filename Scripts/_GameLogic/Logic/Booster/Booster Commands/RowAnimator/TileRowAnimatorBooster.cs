using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Manager;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Booster.Booster_Commands.RowAnimator
{
    public sealed class TileRowAnimatorBooster : BoosterCommandBase
    {
        #region Private Fields
        
        private List<GridTile> gridTilesByRow = new List<GridTile>();
        private List<GridObject> gridObjectsByRow = new List<GridObject>();

        #endregion

        #region Constructor

        public TileRowAnimatorBooster(GridMatrixController gridMatrixController, BoosterDataSO boosterDataSo) : base(gridMatrixController, boosterDataSo) { }
        
        #endregion

        #region Public Methods
        
        public override void Execute(Vector2Int value) => TryAnimateRow(value.x);

        #endregion
        
        #region Private Methods
        
        private void TryAnimateRow(int rowIndex)
        {
            gridTilesByRow = _matrixSearcherQueries.GetGridTilesByRowFromMatrix(rowIndex);
            gridObjectsByRow = _matrixSearcherQueries.GetGridObjectByRowFromMatrix(rowIndex);

            if (AreAllTilesObstacleOrMatched(gridTilesByRow) || !DoTilesExist(gridTilesByRow))
            {
                Debug.LogWarning("Mismatch grid types or row is empty: " + rowIndex);
                ScheduleAnimationEnd(0.25f);
                
                return;
            }

            DoRowAnimation();
        }
        
        private void DoRowAnimation()
        {
            _boosterDataSO.DecreaseBoosterValue(BoosterType.RowChanger, 1);
            TDebug.LogGreen("RowChanger Booster Used" + _boosterDataSO.GetBoosterValue(BoosterType.RowChanger));

            (float delayIncrement, float totalAnimationTime) = CalculateAnimationTimes(gridTilesByRow.Count);

            for (int index = 0; index < gridTilesByRow.Count; index++)
            {
                ScheduleTileAnimation(gridTilesByRow[index], gridObjectsByRow[index], index * delayIncrement);
            }

            ScheduleAnimationEnd(totalAnimationTime);
        }
        
        private bool AreAllTilesObstacleOrMatched(List<GridTile> _gridTilesByRow) => _gridTilesByRow.All(x => x.gridObjectType is GridObjectType.Obstacle or GridObjectType.Matched);

        private bool DoTilesExist(List<GridTile> _gridTilesByRow) => _gridTilesByRow.Count > 0;

        private void ScheduleAnimationEnd(float delay) => DOVirtual.DelayedCall(delay, () =>
        {
            AnimationEnd();
            EventManager.BoosterEvents.BoosterUIDeactivationRequested?.Invoke(BoosterType.RowChanger);
        });

        private void ScheduleTileAnimation(GridTile gridTile, GridObject gridObject, float delay)
        {
            if (IsGridTileCanRowBoost(gridTile)) DOVirtual.DelayedCall(delay, gridObject.AnimateThisGridObject);
        }

        private (float, float) CalculateAnimationTimes(int count)
        {
            float delayIncrement = 0.15f;
            float totalAnimationTime = count * delayIncrement;

            return (delayIncrement, totalAnimationTime);
        }

        private bool IsGridTileCanRowBoost(GridTile gridTile)
        {
            return gridTile.gridObjectType switch
            {
                GridObjectType.Obstacle => false,
                GridObjectType.Matched => false,
                _ => true
            };
        }
        
        #endregion
    }
}