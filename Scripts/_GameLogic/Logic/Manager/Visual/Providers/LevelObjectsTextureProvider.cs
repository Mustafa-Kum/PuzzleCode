using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers
{
    public class LevelObjectsTextureProvider
    {
        #region Private Variables

        private readonly LevelList_SO _levelListSo;
        private readonly Material _objectMaterial;

        #endregion
        
        #region Constructors
        
        public LevelObjectsTextureProvider(LevelList_SO levelListSo, Material objectMaterial)
        {
            _levelListSo = levelListSo;
            _objectMaterial = objectMaterial;
        }
        
        #endregion
        
        #region Public Methods
        
        public void SetGridObjectTexture()
        {
            var gridObjectTexture = GetGridObjectTexture();

            if (gridObjectTexture != null)
            {
                _objectMaterial.mainTexture = gridObjectTexture;
            }
        }
        
        #endregion
        
        #region Private Methods
        
        private Texture2D GetGridObjectTexture()
        {
            var levelSo = _levelListSo.GetCurrentLevelSO();
            return levelSo.inLevelObjectsConfigurationsData.CubeTexture;
        }
        
        #endregion
    }
}