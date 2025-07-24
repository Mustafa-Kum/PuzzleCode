using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Helper.Extensions.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    [CreateAssetMenu(fileName = "GridObjectContainer", menuName = "PuzzleGame/GridObjectContainer", order = 0)]
    public class GridObjectContainer : SerializedScriptableObject
    {
        #region Public Variables

        public Dictionary<GridObjectType, GridObject> gridObjectDictionary;

        #endregion
        
        #region Public Methods
        
        public GridObject GetRandomGridObject()
        {
            int randomIndex = Random.Range(0, gridObjectDictionary.Count);

            return gridObjectDictionary.ElementAt(randomIndex).Value;
        }
        
        public GridObject GetGridObjectByType(GridObjectType gridObjectType)
        {
            if (gridObjectDictionary.TryGetValue(gridObjectType, out GridObject gridObjectPrefab))
            {
                return gridObjectPrefab;
            }
            else
            {
                TDebug.LogError($"Grid object type {gridObjectType} not found in dictionary.");
                return null;
            }
        }

        
        #endregion
    }
}