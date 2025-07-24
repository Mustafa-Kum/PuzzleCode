using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.ResourceManager
{
    public class HealthManager : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public HealthValuesSO healthValuesSo;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelFail += DecreaseHealth;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelFail -= DecreaseHealth;
        }

        #endregion

        #region PRIVATE METHODS

        [Button]
        private void DecreaseHealth()
        {
            healthValuesSo.DecreaseHealthValue(1);
            
            EventManager.Resource.HealthChanged?.Invoke();
            Debug.Log( $"Health Decreased: {healthValuesSo.GetHealthValue()}");
        }
        
        [Button]
        private void IncreaseHealth()
        {
            healthValuesSo.IncreaseHealthValue(1);
            
            EventManager.Resource.HealthChanged?.Invoke();
        }
        
        [Button]
        private void SetInfiniteHealth()
        {
            healthValuesSo.SetInfiniteHealth(true);
        }
        
        #endregion
    }
}