using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class HintUIServiceProvider
    {
        #region Private Variables

        private readonly List<Image> _hintImages;
        
        private readonly GridObjectColorData _gridObjectColorData;

        #endregion

        #region Constructor

        public HintUIServiceProvider(List<Image> hintImages, GridObjectColorData gridObjectColorData)
        {
            _hintImages = hintImages;
            _gridObjectColorData = gridObjectColorData;
        }

        #endregion

        #region Public Methods

        public void InitializeHintUI()
        {
            InitializeCubeHintImages();
            DisableHintImagesBasedOnReduction();
        }

        #endregion
        
        #region Private Methods

        private void InitializeCubeHintImages()
        {
            foreach (var image in _hintImages)
            {
                image.gameObject.SetActive(true);
            }
        }

        private void DisableHintImagesBasedOnReduction()
        {
            int reduction = _gridObjectColorData.GetRotationReduction();
            int startIndex = _hintImages.Count - reduction;
            for (int i = startIndex; i < _hintImages.Count; i++)
            {
                _hintImages[i].gameObject.SetActive(false);
            }
        }

        #endregion
    }
}