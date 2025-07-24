using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.UI.Listeners;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class InGameBoosterRewardedButton : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        public UILevelStateAssigner levelStateAssigner;
        public InGameRewardedAdData inGameRewardedAdData;
        public BoosterType boosterType;
        
        [SerializeField] private BoosterDataSO boosterData;
        [SerializeField] private Button button;

        #endregion
        
        #region PRIVATE VARIABLES

        private bool _isAdShowed;
        
        private bool _isLevelCompleted;

        #endregion
        
        #region UNITY METHODS

        private void Awake()
        {
            button.onClick.AddListener(OnClicked);
        }

        private void OnEnable()
        {
            EventManager.BoosterEvents.BoosterAnimationEnd += CheckBoosterValues;
            EventManager.InGameEvents.LevelFail += LevelFailButtonVisibility;
            EventManager.InGameEvents.LevelSuccess += LevelSuccessButtonVisibility;
            EventManager.GridEvents.OnPathFound += OnPathFoundButtonVisibility;
            
            CheckBoosterValues();
        }

        private void OnDisable()
        {
            EventManager.BoosterEvents.BoosterAnimationEnd -= CheckBoosterValues;
            EventManager.InGameEvents.LevelFail -= LevelFailButtonVisibility;
            EventManager.InGameEvents.LevelSuccess -= LevelSuccessButtonVisibility;
            EventManager.GridEvents.OnPathFound -= OnPathFoundButtonVisibility;
        }

        #endregion

        #region PRIVATE METHODS
        
        private void CheckBoosterValues()
        {
            if (levelStateAssigner.IsLevelCompleted)
            {
                button.gameObject.SetActive(false);
                return;
            }
            
            button.gameObject.SetActive(boosterData.IsBoosterAvailableLevelRequirement(boosterType) && 
                                        boosterData.GetBoosterValue(boosterType) <= inGameRewardedAdData.rewardPanelActiveThreshold &&
                                        !_isAdShowed);   
            
            Vector3 punch = new Vector3(0.2f, 0.2f, 0.2f);
            button.transform.DOPunchScale(punch, 0.5f, 10, 0.5f).SetEase(Ease.OutQuad);
        }
        
        private void OnPathFoundButtonVisibility(List<GridTile> path)
        {
            gameObject.SetActive(false);
        }
        
        private void LevelFailButtonVisibility()
        {
            gameObject.SetActive(false);
        }
        
        private void LevelSuccessButtonVisibility()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region PUBLIC METHODS

        public void OnClicked()
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.AdButton, false);
            EventManager.AdEvents.ShowRewarded?.Invoke(OnRewardedAddSuccessful);   
        }
        
        public void OnRewardedAddSuccessful()
        {
            _isAdShowed = true;
            boosterData.AddBoosterValue( boosterType, inGameRewardedAdData.rewardAmount);
            EventManager.BoosterEvents.BoosterValueChanged?.Invoke();
            CheckBoosterValues();
        }

        #endregion
    }
}