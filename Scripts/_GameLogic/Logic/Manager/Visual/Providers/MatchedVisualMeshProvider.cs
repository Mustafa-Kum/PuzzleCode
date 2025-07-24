using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers
{
    public class MatchedVisualMeshProvider
    {
        #region Private Variables

        private readonly LevelList_SO _levelListSo;
        private readonly Transform _parentTransform;
        private readonly GameObject _currentMatchedObject;

        #endregion

        #region Constructors

        public MatchedVisualMeshProvider(LevelList_SO levelListSo, Transform parentTransform, GameObject currentMatchedObject)
        {
            _levelListSo = levelListSo;
            _parentTransform = parentTransform;
            _currentMatchedObject = currentMatchedObject;
        }
        
        #endregion

        #region Public Methods

        public void ProduceMatchedVisual() => ProduceNewObstaclePrefab();

        #endregion

        #region Private Methods
        
        private GameObject GetMatchedPrefab()
        {
            var levelSo = _levelListSo.GetCurrentLevelSO();
            return levelSo.inLevelObjectsConfigurationsData.LevelObstacle;
        }
        
        private void DisableCurrentMatchedMesh()
        {
            _currentMatchedObject.SetActive(false);
        }
        
        private void ProduceNewObstaclePrefab()
        {
            var matchedPrefab = GetMatchedPrefab();
    
            Object.Instantiate(matchedPrefab, _parentTransform.position, matchedPrefab.transform.rotation, _parentTransform);
            matchedPrefab.transform.localScale = 0.85f * Vector3.one;
            DisableCurrentMatchedMesh();
        }

        #endregion
    }
}