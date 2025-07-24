using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers
{
    public class ObstacleVisualMeshProvider
    {
        #region Private Variables

        private readonly LevelList_SO _levelListSo;
        private readonly Transform _parentTransform;
        private readonly GameObject _currentObstacle;

        #endregion

        #region Constructors

        public ObstacleVisualMeshProvider(LevelList_SO levelListSo, Transform parentTransform, GameObject currentObstacle)
        {
            _levelListSo = levelListSo;
            _parentTransform = parentTransform;
            _currentObstacle = currentObstacle;
        }
        
        #endregion

        #region Public Methods

        public void ProduceObstacleVisual() => ProduceNewObstaclePrefab();

        #endregion

        #region Private Methods
        
        private GameObject GetObstaclePrefab()
        {
            var levelSo = _levelListSo.GetCurrentLevelSO();
            return levelSo.inLevelObjectsConfigurationsData.LevelObstacle;
        }
        
        private void DisableCurrentObstacleMesh()
        {
            _currentObstacle.SetActive(false);
        }
        
        private void ProduceNewObstaclePrefab()
        {
            var obstaclePrefab = GetObstaclePrefab();
    
            if (obstaclePrefab != null)
            {
                Object.Instantiate(obstaclePrefab, _parentTransform.position, obstaclePrefab.transform.rotation, _parentTransform);
                obstaclePrefab.transform.localScale = 0.85f * Vector3.one;
                DisableCurrentObstacleMesh();
            }
            else
            {
                Debug.LogWarning("LevelObstacle is null. Prefab could not be instantiated.");
            }
        }

        #endregion
    }
}