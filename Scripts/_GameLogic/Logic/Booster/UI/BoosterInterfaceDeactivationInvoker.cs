using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class BoosterInterfaceDeactivationInvoker : MonoBehaviour
    {
        #region Public Variables

        public BoosterType boosterType;

        #endregion
        
        #region Public Methods

        public void InvokeBoosterInterfaceDeactivation()
        {
            EventManager.BoosterEvents.BoosterUIDeactivationRequested?.Invoke(boosterType);
        }

        #endregion
    }
}