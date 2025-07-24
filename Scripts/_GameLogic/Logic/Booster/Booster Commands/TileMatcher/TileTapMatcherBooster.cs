using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Manager;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Booster.Booster_Commands.TileMatcher
{
    public sealed class TileTapMatcherBooster : BoosterCommandBase
    {
        #region Constructors

        public TileTapMatcherBooster(GridMatrixController gridMatrixController, BoosterDataSO boosterDataSo) : base(gridMatrixController, boosterDataSo) { }

        #endregion

        #region Public Methods

        public override void Execute(Vector2Int value) => TileTapMatcher(value);

        #endregion

        #region Private Methods

        private void TileTapMatcher(Vector2Int value)
        {
            _boosterDataSO.DecreaseBoosterValue(BoosterType.GridTileMatcher, 1);
            ProcessGridObjects(value);
            PlaySound();
            DOVirtual.DelayedCall(0.25f, AnimationEnd);
        }

        private void ProcessGridObjects(Vector2Int value)
        {
            _matrixSearcherQueries.GetAllGridTilesFromMatrix().ForEach(gridTile =>
            {
                if (gridTile.GridPosition != value) return;
                gridTile.MatchableAction();
            });
        }
        
        private static void PlaySound() => EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Placing, false);

        #endregion
        

    }
}