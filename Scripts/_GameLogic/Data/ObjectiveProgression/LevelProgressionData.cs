using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Manager.Objective;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.ObjectiveProgression
{
    [CreateAssetMenu(fileName = nameof(LevelProgressionData), menuName = "Level Progression/Level Data", order = 1)]
    public class LevelProgressionData : ScriptableObject
    {
        #region PUBLIC VARIABLES

        public List<LevelProgressionGroup> LevelProgressionGroups; 

        #endregion

        #region CONSTRUCTORS

        public LevelProgressionItems GetLevelObjectives(int levelId)
        {
            var level = LevelProgressionGroups.Find(l => l.LevelIndex == levelId);
            return level.LevelObjectives;
        }

        #endregion

        #region DATA

        [Serializable]
        public struct LevelProgressionGroup
        {
            public int LevelIndex; 
            public LevelProgressionItems LevelObjectives; 
        }

        #endregion
        
    }
}