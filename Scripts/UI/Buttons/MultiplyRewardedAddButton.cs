using System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using FortuneWheel;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.UI.Buttons
{
    public class MultiplyRewardedAddButton : ButtonBase
    {
        [SerializeField] private MultiplyRewardedAdData multiplyRewardedAdData;
        [SerializeField] private GameObject buttonToOpen;
        [SerializeField] private float secondsToWait = 3f;
        [SerializeField] private PlayerSavableData playerVariable;

        private new CoroutineService _coroutineService;

        public int multiplierCount = 2;
        
        protected override void OnClicked()
        {
            Debug.Log("MultiplyRewardedAddButton");
            EventManager.AdEvents.ShowRewarded?.Invoke(OnRewardedAddSuccessful);
        }

        private void OnEnable()
        {
            _coroutineService = new CoroutineService(this);

            WaitForSecondsToWaitAndActivateButtonToOpen();
        }

        private void WaitForSecondsToWaitAndActivateButtonToOpen()
        {
            _coroutineService.StartDelayedRoutine(ActivateButtonToOpen, secondsToWait);
            buttonToOpen.SetActive(false);
        }

        private void ActivateButtonToOpen()
        {
            buttonToOpen.SetActive(true);
        }
        
        private void OnRewardedAddSuccessful()
        {
            EventManager.InGameEvents.LoadLevel?.Invoke();
            
            EventManager.Resource.RewardedAd?.Invoke(new RewardedAdData
            {
                CurrencyType = multiplyRewardedAdData.currencyType,
                rewardAmount = multiplyRewardedAdData.RewardAmount * multiplierCount
            });

            buttonToOpen.GetComponent<NextLevelButton>().HandleClick();
        }

        private void NextLevelMethod()
        {
            buttonToOpen.GetComponent<NextLevelButton>().HandleClick();
        }
    }

    [Serializable]
    public struct MultiplyRewardedAdData
    {
        public int RewardAmount;
        public int MultiplyCount;
        [FormerlySerializedAs("CollectableType")] public CurrencyType currencyType;
    }
}