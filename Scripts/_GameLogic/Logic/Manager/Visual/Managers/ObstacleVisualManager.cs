using _Game.Scripts._GameLogic.Logic.Manager.Visual.Base;
using _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Managers
{
    public class ObstacleVisualManager : BaseObjectVisualManager
    {
        #region Unity Methods

        private void Awake() => PerformChangeObjectVisual();

        #endregion

        #region PUBLIC METHODS

        public override void PerformChangeObjectVisual()
        {
            ObstacleVisualMeshProvider obstacleVisualMeshProvider = new ObstacleVisualMeshProvider(_levelListSO, _parentTransform, _currentObstacle);
            obstacleVisualMeshProvider.ProduceObstacleVisual();
        }

        #endregion
    }
}