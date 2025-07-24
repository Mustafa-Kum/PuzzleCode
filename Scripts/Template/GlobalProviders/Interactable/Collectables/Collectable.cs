using System;
using _Game.Scripts.Managers.Core;
using Handler.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Collectables
{
    public sealed class Collectable : MonoBehaviour
    {
        #region Public Variables
        
        public CollectableData collectableData;
        
        #endregion

        #region Private Variables
        private bool CanCollect { get; set; } = true;

        #endregion

    }
    
    [Serializable]
    public struct CollectableData
    {
        public GridObjectType Type;
        public int ScoreAmount;
        [HideInInspector] public Vector3 collectedPosition;
    }
    
    [Serializable]
    public enum CurrencyType
    {
        Coin,
        Gem,
    }
}