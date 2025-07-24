using _Game.Scripts._GameLogic.Logic.Manager.Visual.Base;
using _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Managers
{
    public class MatchedVisualManager : BaseObjectVisualManager
    {
        #region Unity Methods

        private void Awake() => PerformChangeObjectVisual();

        #endregion

        #region PUBLIC METHODS

        public override void PerformChangeObjectVisual()
        {
            MatchedVisualMeshProvider matchedVisualMeshProvider = new MatchedVisualMeshProvider(_levelListSO, _parentTransform, _currentObstacle);
            matchedVisualMeshProvider.ProduceMatchedVisual();
        }

        #endregion
    }
}