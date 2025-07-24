using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts._GameLogic.Data.Store.Product
{
    [CreateAssetMenu(fileName = "PurchasableSO", menuName = "IAP/Purchasables", order = 0)]
    public class PurchasableProductSO : ScriptableObject
    {
        #region PUBLIC VARIABLES

        [Header("IAP Model")]
        public ProductType productType;
        public string productID;
        
        [Header("Product View")]
        public string productName;
        public double productPrice;
        public Sprite productImage; 

        [Header("Product Actions")]
        public List<ProductAction> actions;

        #endregion

        #region PUBLIC METHODS

        public void Init(ConfigurationBuilder builder)
        {
            builder.AddProduct(productID, productType);
        }

        #endregion
    }
    
    [Serializable]
    public class ProductAction
    {
        public ProductActionType actionType;
    
        [ShowIf("RequiresAmount")]
        public int amount;
    
        [ShowIf("IsBoosterAction")]
        public BoosterType boosterType;  // Booster türünü tanımlayın

        private bool RequiresAmount() => actionType != ProductActionType.SetInfiniteHealth;

        private bool IsBoosterAction() => actionType == ProductActionType.AddBooster;
    }

    
    public enum ProductActionType
    {
        AddCoinCurrency,
        AddGemCurrency,
        AddBooster,
        SetInfiniteHealth
    }
}