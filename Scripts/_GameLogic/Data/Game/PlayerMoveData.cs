using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Game
{
    [CreateAssetMenu(fileName = "PlayerMoveData", menuName = "PuzzleGame/PlayerMoveData", order = 0)]
    public class PlayerMoveData : ScriptableObject
    {
        #region Public Variables

        public int maxMoves;
        
        public int currentMoves;

        #endregion
        
        #region Public Methods

        public void DecreaseCurrentMoves()
        {
            if (currentMoves > 0)
            {
                currentMoves--;
            }
        }
        
        public int GetCurrentMoves() => currentMoves;
        
        public bool CheckMoveEqualsZero() => currentMoves == 0;
        
        public bool CanMove() => currentMoves > 0;

        public void ResetMoves() => currentMoves = maxMoves;
        
        public void AddMoves(int amount) => currentMoves += amount;

        #endregion
    }
}