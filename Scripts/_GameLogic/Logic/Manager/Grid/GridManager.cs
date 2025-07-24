using System;
using UnityEngine;
using _Game.Scripts._GameLogic.Data.Game;
using _Game.Scripts._GameLogic.Data.Visual;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;

namespace _Game.Scripts._GameLogic.Logic.Manager
{
    public class GridManager : MonoBehaviour
    {
        #region Public Variables

        public ParticleContainer particleContainer;
        public GridConfig gridConfig;
        public GridMoveData gridMoveData;
        public PlayerMoveData playerMoveData;

        #endregion

        #region Properties

        public ParticleContainer ParticleContainer => particleContainer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            ParseMoveData();
            EventManager.GridEvents.OnGridConfigCreated.Invoke(gridConfig.GetGridConfig());   
        }

        #endregion

        #region Private Methods

        private void ParseMoveData()
        {
            playerMoveData.maxMoves = gridMoveData.maxMoves;
            playerMoveData.currentMoves = gridMoveData.currentMoves;
        }

        #endregion
    }
}

[Serializable]
public struct GridMoveData
{
    public int maxMoves;
    public int currentMoves;
}