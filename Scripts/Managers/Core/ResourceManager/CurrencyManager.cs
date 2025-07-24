using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts.Managers.Core.StoreManager;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using FortuneWheel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core.ResourceManager
{
    public class CurrencyManager : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private CurrencyValuesSO currencyValuesSo;
        [SerializeField] private ProductCatalogSO productCatalog;
        [SerializeField] private BoosterDataSO boosterDataSo;
        [SerializeField] private HealthValuesSO healthValuesSo;

        #endregion

        #region PRIVATE VARIABLES

        private IAPCurrencyStore _iapCurrencyStore;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _iapCurrencyStore = new IAPCurrencyStore(currencyValuesSo, productCatalog, boosterDataSo, healthValuesSo);
            _iapCurrencyStore.InitializeProducts();
        }

        private void OnEnable()
        {
            EventManager.Resource.TryToBuy += HandleTryToBuyEvent;
            EventManager.Resource.RewardedAd += AddValueOfRewardedAdEvent;
            EventManager.IAPEvents.PurchaseSuccess += HandlePurchaseSuccessEvent;
        }

        private void OnDisable()
        {
            EventManager.Resource.TryToBuy -= HandleTryToBuyEvent;
            EventManager.Resource.RewardedAd -= AddValueOfRewardedAdEvent;
            EventManager.IAPEvents.PurchaseSuccess -= HandlePurchaseSuccessEvent;
        }

        #endregion

        #region PRIVATE METHODS

        private void HandlePurchaseSuccessEvent(PurchaseEventArgs arg0)
        {
            _iapCurrencyStore.ProcessPurchase(arg0.purchasedProduct.definition.id);
        }

        private void HandleTryToBuyEvent(int cost, CurrencyType currencyType, UnityAction<bool> callback)
        {
            if (currencyValuesSo.GetValue(currencyType) >= cost)
            {
                currencyValuesSo.SpendValue(CurrencyType.Coin, cost);
                callback?.Invoke(true);
                EventManager.Resource.CollectableSpent?.Invoke(currencyType);
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        private void AddValueOfRewardedAdEvent(RewardedAdData rewardedAdData)
        {
            currencyValuesSo.AddValue(rewardedAdData.CurrencyType, rewardedAdData.rewardAmount);
            EventManager.Resource.CollectableSpent?.Invoke(rewardedAdData.CurrencyType);
        }

        #endregion
    }
}