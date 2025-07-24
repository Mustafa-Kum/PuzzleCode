using System;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Booster
{
    public abstract class BoosterCommandBase
    {
        #region Protected Fields
        
        protected readonly GridMatrixController _gridMatrixController;

        protected readonly BoosterDataSO _boosterDataSO;

        protected readonly MatrixSearcherQueries _matrixSearcherQueries;
        
        #endregion

        #region Constructor

        protected BoosterCommandBase(GridMatrixController gridMatrixController, BoosterDataSO boosterDataSO)
        {
            _gridMatrixController = gridMatrixController;
            _boosterDataSO = boosterDataSO;
            _matrixSearcherQueries = new MatrixSearcherQueries(_gridMatrixController.Matrix);
        }

        #endregion

        #region Public Methods

        public abstract void Execute(Vector2Int value);

        protected virtual void AnimationEnd()
        {
            EventManager.BoosterEvents.BoosterAnimationEnd?.Invoke();
        }

        #endregion
    }
}

[Serializable]
public enum BoosterType
{
    RowChanger,
    GridTileMatcher,
}