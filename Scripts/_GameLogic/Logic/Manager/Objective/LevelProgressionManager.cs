using _Game.Scripts._GameLogic.Data.ObjectiveProgression;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Objective
{
    public class LevelProgressionManager : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        [SerializeField] private LevelProgressionData _levelProgressionData;
        
        [SerializeField] private PlayerSavableData _playerSavableData;

        [SerializeField] private LevelProgressionItems _currentLevelObjectives;
        
        #endregion

        #region PRIVATE VARIABLES

        private LevelSuccessHandler _levelSuccessHandler;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _levelSuccessHandler = new LevelSuccessHandler();
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelLoaded += OnLevelStarted;
            EventManager.GridEvents.GridObjectMatchedType += OnGridObjectMatchedType;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= OnLevelStarted;
            EventManager.GridEvents.GridObjectMatchedType -= OnGridObjectMatchedType;
        }

        #endregion
        
        #region PRIVATE METHODS
        
        private void OnLevelStarted(GameObject levelObject)
        {
            LevelProgressionItems levelObjectives = _levelProgressionData.GetLevelObjectives(_playerSavableData.LevelIndex);
            _currentLevelObjectives = new LevelProgressionItems(levelObjectives);
            
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated.Invoke(_currentLevelObjectives.Objectives);
        }

        private void OnGridObjectMatchedType(GridObjectType type, Vector3 position)
        {
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated.Invoke(_currentLevelObjectives.Objectives);

            DOVirtual.DelayedCall(2f, () => CheckObjectivesAndTriggerLevelSuccess(type));
        }

        private void CheckObjectivesAndTriggerLevelSuccess(GridObjectType type)
        {
            _currentLevelObjectives.ReduceObjective(type);
            if (_currentLevelObjectives.IsObjectivesCompleted())
            {
                _levelSuccessHandler.TriggerLevelSuccess(0);
                Debug.Log("Level Success");
            }
        }
        
        #endregion
    }
}