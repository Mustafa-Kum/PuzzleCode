using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class BoosterEvents
        {
            #region Booster
            
            public static UnityAction<BoosterType> BoosterUIActivationRequested;
            
            public static UnityAction<BoosterType> BoosterUIDeactivationRequested;
            
            public static UnityAction<BoosterType, Vector2Int> BoosterCommandExecute;
            
            public static UnityAction<BoosterType> BoosterAnimationBegan;
            
            public static UnityAction BoosterAnimationEnd;
            
            public static UnityAction BoosterValueChanged;
            
            #endregion
        }
    }
}