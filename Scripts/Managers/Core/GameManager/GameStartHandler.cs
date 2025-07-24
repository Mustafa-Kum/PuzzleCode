using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class GameStartHandler : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        public PlayerSavableData playerSavableData;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
             DOTween.SetTweensCapacity(1000, 1000);
             if (playerSavableData.IsLevelEqualsZero())
             {
                 StartLevel();
             }
             else
             {
                 InitializeGame();
             }
        }
        
        #endregion

        #region PRIVATE METHODS

        private void InitializeGame()
        {
            EventManager.InGameEvents.GameStarted?.Invoke();
        }
        
        private void StartLevel()
        {
            EventManager.InGameEvents.GameStarted?.Invoke();
            
            DOVirtual.DelayedCall(0.01f, () =>
            {
                EventManager.InGameEvents.LevelStart?.Invoke();
                EventManager.TutorialEvents.LevelStartTutorialCaching?.Invoke(1);
            });
        }
        
        #endregion
        
    }
}