using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "BoosterData", menuName = "PuzzleGame/BoosterData", order = 1)]
    public class BoosterDataSO : PersistentSaveManager<BoosterDataSO>, IResettable
    {
        #region Public Variables

        public Dictionary<BoosterType, int> boosterValues = new Dictionary<BoosterType, int>();
        
        public PlayerSavableData playerSavableData;
        public BoosterUnlockLevels boosterUnlockLevels;

        #endregion

        #region Public Methods

        public int GetBoosterValue(BoosterType boosterType) => boosterValues.ContainsKey(boosterType) ? boosterValues[boosterType] : 0;

        public void DecreaseBoosterValue(BoosterType boosterType, int decreasedValue)
        {
            if (decreasedValue <= 0)
            {
                return;
            }
            
            if (boosterValues.ContainsKey(boosterType))
            {
                boosterValues[boosterType] -= decreasedValue;
            }
            else
            {
                boosterValues.Add(boosterType, -decreasedValue);
            }
        }
        
        public void AddBoosterValue(BoosterType type, int addedValue)
        {
            if (boosterValues.ContainsKey(type))
            {
                boosterValues[type] += addedValue;
            }
            else
            {
                boosterValues.Add(type, addedValue);
            }
            
            EventManager.Resource.BoosterValueChanged?.Invoke();
        }
        
        public void AddValueToAllBoosters(int addedValue)
        {
            var keys = new List<BoosterType>(boosterValues.Keys);
            foreach (BoosterType key in keys)
            {
                boosterValues[key] += addedValue;
            }
            
            EventManager.Resource.BoosterValueChanged?.Invoke();
        }
        
        public bool IsBoosterAvailableWithDictionary(BoosterType boosterType)
        {
            var boosterValueExists = boosterValues.TryGetValue(boosterType, out int boosterValue);
            var levelRequirementExists = boosterUnlockLevels.boosterLevelMappings.Any(m => m.boosterType == boosterType);

            if(boosterValueExists && levelRequirementExists)
            {
                var unlockLevel = boosterUnlockLevels.boosterLevelMappings
                    .Where(m => m.boosterType == boosterType)
                    .Select(m => m.unlockLevel)
                    .FirstOrDefault();

                return boosterValue > 0 && playerSavableData.LevelIndex >= unlockLevel;
            } 

            return false;
        }
        
        public bool IsBoosterAvailableLevelRequirement(BoosterType boosterType)
        {
            var levelRequirementExists = boosterUnlockLevels.boosterLevelMappings.Any(m => m.boosterType == boosterType);

            if(levelRequirementExists)
            {
                var unlockLevel = boosterUnlockLevels.boosterLevelMappings
                    .Where(m => m.boosterType == boosterType)
                    .Select(m => m.unlockLevel)
                    .FirstOrDefault();

                return playerSavableData.LevelIndex >= unlockLevel;
            }

            return false;
        }
        
        #endregion
    }
}
