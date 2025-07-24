using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Booster.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Manager.UI
{
    public class HintUIManager : MonoBehaviour
    {
        #region Public Variables

        public List<Image> cubeHintImages;
        
        public GridObjectColorData gridObjectColorData;

        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            HintUIServiceProvider hintUIServiceProvider = new HintUIServiceProvider(cubeHintImages, gridObjectColorData);
            hintUIServiceProvider.InitializeHintUI();
        }
        
        #endregion
    }
}