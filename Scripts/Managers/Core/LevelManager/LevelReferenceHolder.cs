using System;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Manager;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class LevelReferenceHolder : MonoBehaviour
    {
        #region Public Variables

        public GridManager gridManager;
        
        public GridMatrixController gridMatrixController;
        
        #endregion

        #region Private Variables

        private Vector2Int currentGridConfig;

        #endregion

        #region Properties

        public Vector2Int CurrentGridConfig => currentGridConfig;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            GetCurrentLevelGridConfig();
        }

        #endregion

        #region Private Methods

        private void GetCurrentLevelGridConfig()
        {
            currentGridConfig = gridManager.gridConfig.GetGridConfig();
        }

        #endregion
    }
}
