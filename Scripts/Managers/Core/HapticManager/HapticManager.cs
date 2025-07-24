using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.ScriptableObjects.Saveable;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.HapticManager
{
    [RequireComponent(typeof(HapticController))]
    public class HapticManager : MonoBehaviour
    {
        #region Public Variables

        [SerializeField] private SettingsDataSO settingsData;
        
        [SerializeField] private HapticController hapticController;

        #endregion
        
        #region Unity Lifecycle Methods
        
        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += OnSuccess;
            EventManager.InGameEvents.LevelFail += OnFail;
            EventManager.GridEvents.OnGridObjectAscend += OnGridObjectAscend;
            EventManager.GridEvents.OnGridObjectDescend += OnGridObjectDescend;
            EventManager.GridEvents.OnPathFound += OnPathFound;
            EventManager.BoosterEvents.BoosterUIActivationRequested += OnBoosterUIActivationRequested;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested += OnBoosterUIDeactivationRequested;
            EventManager.BoosterEvents.BoosterCommandExecute += OnBoosterCommandExecute;
            EventManager.UIEvents.OnMenuTabsChanged += OnMainMenuTabChanged;
            EventManager.InGameEvents.LevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= OnSuccess;
            EventManager.InGameEvents.LevelFail -= OnFail;
            EventManager.GridEvents.OnGridObjectAscend -= OnGridObjectAscend;
            EventManager.GridEvents.OnGridObjectDescend -= OnGridObjectDescend;
            EventManager.GridEvents.OnPathFound -= OnPathFound;
            EventManager.BoosterEvents.BoosterUIActivationRequested -= OnBoosterUIActivationRequested;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested -= OnBoosterUIDeactivationRequested;
            EventManager.BoosterEvents.BoosterCommandExecute -= OnBoosterCommandExecute;
            EventManager.UIEvents.OnMenuTabsChanged -= OnMainMenuTabChanged;
            EventManager.InGameEvents.LevelStart -= OnLevelStart;
        }
        
        #endregion
        
        #region Event Callbacks
        
        private void OnLevelStart()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        private void OnSuccess()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Success);
        }
        
        private void OnFail()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Failure);
        }
        
        private void OnGridObjectAscend()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.HeavyImpact);
        }
        
        private void OnGridObjectDescend()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.HeavyImpact);
        }
        
        private void OnPathFound(List<GridTile> path)
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHapticCount(HapticPatterns.PresetType.HeavyImpact, 10);
        }
        
        private void OnBoosterUIActivationRequested(BoosterType _boosterType)
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        private void OnBoosterUIDeactivationRequested(BoosterType _boosterType)
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        private void OnBoosterCommandExecute(BoosterType _boosterType, Vector2Int _gridPosition)
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        private void OnMainMenuTabChanged()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        #endregion
    }
}
