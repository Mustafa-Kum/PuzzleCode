using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class RetryLevelButton : ButtonBase
    {
        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.GridEvents.OnPathFound += OnPathFound;
            EventManager.InGameEvents.LevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnPathFound -= OnPathFound;
            EventManager.InGameEvents.LevelStart -= OnLevelStart;
        }

        #endregion
        
        #region INHERITED METHODS

        protected override void OnClicked() {
        }
        
        #endregion

        #region PRIVATE METHODS
        
        private void OnPathFound(List<GridTile> path)
        {
            targetButton.interactable = false;
        }
        
        private void OnLevelStart()
        {
            targetButton.interactable = true;
        }

        #endregion

    }
}
