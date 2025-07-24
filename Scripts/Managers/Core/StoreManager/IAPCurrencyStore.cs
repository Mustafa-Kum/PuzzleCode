using System;
using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts._GameLogic.Data.Store.Product;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public sealed class IAPCurrencyStore : BasePurchaseProcessor
    {
        #region PRIVATE VARIABLES

        private readonly CurrencyValuesSO _currencyValuesSo;
        private readonly BoosterDataSO _boosterDataSo;
        private readonly ProductCatalogSO _productCatalogSo;
        private readonly HealthValuesSO _healthValuesSo;

        #endregion

        #region CONSTRUCTOR

        public IAPCurrencyStore(
            CurrencyValuesSO currencyValuesSo, 
            ProductCatalogSO productCatalogSo, 
            BoosterDataSO boosterDataSo,
            HealthValuesSO healthValuesSo)
        {
            _currencyValuesSo = currencyValuesSo;
            _productCatalogSo = productCatalogSo;
            _boosterDataSo = boosterDataSo;
            _healthValuesSo = healthValuesSo;
        }

        #endregion

        #region PUBLIC METHODS

        public void InitializeProducts()
        {
            InitializeProductActions();
        }

        #endregion

        #region PRIVATE METHODS

        private void InitializeProductActions()
        {
            foreach (var product in _productCatalogSo.products)
            {
                productActions.Add(product.productID, () => ExecuteProductActions(product));
            }
        }


        private void ExecuteProductActions(PurchasableProductSO product)
        {
            foreach (var action in product.actions)
            {
                switch (action.actionType)
                {
                    case ProductActionType.AddCoinCurrency:
                        _currencyValuesSo.AddValue(CurrencyType.Coin, action.amount);
                        break;
                    case ProductActionType.AddGemCurrency:
                        _currencyValuesSo.AddValue(CurrencyType.Gem, action.amount);
                        break;
                    case ProductActionType.AddBooster:
                        _boosterDataSo.AddBoosterValue(action.boosterType, action.amount);
                        break;
                    case ProductActionType.SetInfiniteHealth:
                        _healthValuesSo.SetInfiniteHealth(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        #endregion
    }
}