using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.UI.Listeners
{
    public class UILevelStateAssigner : MonoBehaviour
    {
        #region PRIVATE VARIABLES

        private bool _isLevelCompleted;
        
        #endregion

        #region PROPERTIES

        public bool IsLevelCompleted => _isLevelCompleted;

        #endregion
        
        #region UNITY METHODS
        
        private void OnEnable()
        {
            EventManager.GridEvents.OnPathFound += OnPathFound;
        }
        
        private void OnDisable()
        {
            EventManager.GridEvents.OnPathFound -= OnPathFound;
        }
        
        #endregion
        
        #region PRIVATE METHODS
        
        private void OnPathFound(List<GridTile> path)
        {
            _isLevelCompleted = true;
        }
        
        #endregion
    }
}