using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{
    public class EditorGridObjectManager : MonoBehaviour
    {
#if UNITY_EDITOR
        #region PUBLIC VARIABLES
        
        public GridObjectContainer gridObjectContainer;
        [SerializeField] private GridMatrixController _gridMatrixController;
        [HideInInspector] public List<GridObject> AllGridObjects;
        [ShowIf("@isSpawnFromJson == true")] public TextAsset levelJsonFile;
        public bool isSpawnFromJson;
        
        #endregion
        
        #region PUBLIC METHODS
        
        public void SetupGridObjects(GridConfig gridConfig)
        {
            EditorGridObjectFactory gridObjectFactory = new EditorGridObjectFactory(
                gridConfig, 
                isSpawnFromJson,
                gridObjectContainer,
                new EditorGridObjectInitializer(), 
                AllGridObjects, 
                _gridMatrixController);
            
            if (levelJsonFile != null && isSpawnFromJson)
                gridObjectFactory.CreateGridObjectsFromJson(levelJsonFile);
            else
                gridObjectFactory.CreateGridObjectsRandom();

            _gridMatrixController.SetGridObjects(AllGridObjects);
            _gridMatrixController.CreateGridMatrix();
            PerformMatrixPointSelection();
            _gridMatrixController.GeneratePoints();
        }
        
        #endregion

        #region PRIVATE METHODS

        private void PerformMatrixPointSelection()
        {
            var tileMarkerInitializer = new EditorMatrixPointSelector(_gridMatrixController.Matrix, _gridMatrixController.startPoint, _gridMatrixController.endPoint,
                _gridMatrixController._gridManager.ParticleContainer);

            tileMarkerInitializer.PerformTileIndicating();
        }

        #endregion
#endif
    }
}

[Serializable]
public enum GridObjectType
{
    Red,
    Green,
    Blue,
    Yellow,
    Matched,
    Obstacle
}