using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Game;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class InGameMoveAdvertiseButton : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private InGameRewardedAdData inGameRewardedAdData;
        
        [SerializeField] private PlayerMoveData playerMoveData;
        [SerializeField] private Button button;

        #endregion

        #region PRIVATE VARIABLES

        private bool _isAdShowed;

        #endregion
        
        #region UNITY METHODS

        private void Awake() => button.onClick.AddListener(OnClicked);

        protected void OnEnable()
        {
            EventManager.UIEvents.OnMoveCounterChanged += OnMoveCounterChanged;
            EventManager.GridEvents.OnPathFound += UpdateButtonVisibility;
            EventManager.InGameEvents.LevelFail += LevelFailButtonVisibility;
            EventManager.InGameEvents.LevelSuccess += LevelFailButtonVisibility;
        }

        protected void OnDisable()
        {
            EventManager.UIEvents.OnMoveCounterChanged -= OnMoveCounterChanged;
            EventManager.GridEvents.OnPathFound -= UpdateButtonVisibility;
            EventManager.InGameEvents.LevelFail -= LevelFailButtonVisibility;
            EventManager.InGameEvents.LevelSuccess -= LevelFailButtonVisibility;
        }

        #endregion

        #region PRIVATE METHODS

        private void OnRewardedAdSuccessful()
        {
            _isAdShowed = true;
            playerMoveData.AddMoves(inGameRewardedAdData.rewardAmount);
            EventManager.UIEvents.OnMoveCounterChanged?.Invoke(playerMoveData.GetCurrentMoves());
        }

        private void OnMoveCounterChanged(int currentMoveCount)
        {
            button.gameObject.SetActive(currentMoveCount <= inGameRewardedAdData.rewardPanelActiveThreshold && !_isAdShowed);
        }
        
        private void UpdateButtonVisibility(List<GridTile> path)
        {
            gameObject.SetActive(false);
        }
        
        private void LevelFailButtonVisibility()
        {
            gameObject.SetActive(false);
        }
        
        private void OnClicked()
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.AdButton, false);
            EventManager.AdEvents.ShowRewarded?.Invoke(OnRewardedAdSuccessful);   
        }
        
        #endregion
    }
    
    [Serializable]
    public struct InGameRewardedAdData
    {
        public int rewardPanelActiveThreshold;
        public int rewardAmount;
    }
}