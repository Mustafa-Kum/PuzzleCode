using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class MatrixSearcherQueries
    {
        #region Public Variables

        private Vector2Int _startPoint;
        private Vector2Int _endPoint;

        #endregion

        private readonly GridTile[,] Matrix;

        public MatrixSearcherQueries(GridTile[,] matrix)
        {
            Matrix = matrix;
        }

        #region Public Methods

        public List<GridTile> GetGridTilesByRowFromMatrix(int row)
        {
            var gridTiles = new List<GridTile>();
            for (var i = 0; i < Matrix.GetLength(1); i++) gridTiles.Add(Matrix[row, i]);

            return gridTiles;
        }

        public List<GridObject> GetGridObjectByRowFromMatrix(int row)
        {
            var gridObjects = new List<GridObject>();
            for (var i = 0; i < Matrix.GetLength(1); i++)
                gridObjects.Add(Matrix[row, i].GetComponentInChildren<GridObject>());

            return gridObjects;
        }

        public List<GridTile> GetAllGridTilesFromMatrix()
        {
            var gridTiles = new List<GridTile>();
            for (var i = 0; i < Matrix.GetLength(0); i++)
            for (var j = 0; j < Matrix.GetLength(1); j++)
                gridTiles.Add(Matrix[i, j]);

            return gridTiles;
        }

        public List<GridObject> GetAllGridObjectsFromMatrix()
        {
            var gridObjects = new List<GridObject>();
            for (var i = 0; i < Matrix.GetLength(0); i++)
            for (var j = 0; j < Matrix.GetLength(1); j++)
                gridObjects.Add(Matrix[i, j].GetComponentInChildren<GridObject>());

            return gridObjects;
        }
        
        #endregion
    }
}