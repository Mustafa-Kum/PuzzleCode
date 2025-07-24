using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Booster;
using _Game.Scripts._GameLogic.Logic.Booster.Booster_Commands.RowAnimator;
using _Game.Scripts._GameLogic.Logic.Booster.Booster_Commands.TileMatcher;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Booster
{
    public class BoosterManager : SerializedMonoBehaviour
    {
        #region Public Variables

        public BoosterDataSO boosterDataSO;

        private readonly Dictionary<BoosterType, BoosterCommandBase> boosterCommandDictionary = new Dictionary<BoosterType, BoosterCommandBase>();
        
        #endregion

        #region Private Variables
        
        private GridMatrixController _gridMatrixController;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            EventManager.InGameEvents.LevelLoaded += OnStartViewInitiated;
            EventManager.BoosterEvents.BoosterCommandExecute += OnHandleBoosterExecute;
            //EventManager.InGameEvents.LevelSuccess += ResetBoosterDataDefault;
            //EventManager.InGameEvents.GameStarted += ResetBoosterDataDefault;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= OnStartViewInitiated;
            EventManager.BoosterEvents.BoosterCommandExecute -= OnHandleBoosterExecute;
            //EventManager.InGameEvents.LevelSuccess -= ResetBoosterDataDefault;
            //EventManager.InGameEvents.GameStarted -= ResetBoosterDataDefault;
        }

        #endregion

        #region Private Methods

        //private void ResetBoosterDataDefault() => boosterDataSO.ResetAllBoosterValuesToDefault();

        private void OnStartViewInitiated(GameObject level)
        {
            InitializeGridManagerFromLevel(level);
            InitializeBoosterCommandDictionary();
        }
        
        private void OnHandleBoosterExecute(BoosterType boosterType, Vector2Int targetValue)
        {
            if (boosterCommandDictionary.TryGetValue(boosterType, out var boosterCommand))
            {
                boosterCommand.Execute(targetValue);
                EventManager.BoosterEvents.BoosterAnimationBegan?.Invoke(boosterType);
            }
            else
            {
                Debug.LogError($"BoosterCommand not found for BoosterType: {boosterType}");
            }
        }
        
        private void InitializeBoosterCommandDictionary()
        {
            boosterCommandDictionary.Clear();
            boosterCommandDictionary.Add(BoosterType.RowChanger, new TileRowAnimatorBooster(_gridMatrixController, boosterDataSO));
            boosterCommandDictionary.Add(BoosterType.GridTileMatcher, new TileTapMatcherBooster(_gridMatrixController, boosterDataSO));
        }
        
        private void InitializeGridManagerFromLevel(GameObject level)
        {
            _gridMatrixController = level.GetComponent<LevelReferenceHolder>().gridMatrixController;
            
            if (_gridMatrixController == null)
            {
                Debug.LogError("GridObjectManager not found in level");
            }
        }

        #endregion
    }
}