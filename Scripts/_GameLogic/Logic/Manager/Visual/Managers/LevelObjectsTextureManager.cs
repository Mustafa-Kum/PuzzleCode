using _Game.Scripts._GameLogic.Logic.Manager.Visual.Providers;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Managers
{
    public class LevelObjectsTextureManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private LevelList_SO _levelListSO;
        
        [SerializeField] private Material _gridObjectMaterial;

        #endregion

        #region Unity Methods
        
        private void Awake()
        {
            LevelObjectsTextureProvider levelObjectsTextureProvider = new LevelObjectsTextureProvider(_levelListSO, _gridObjectMaterial);
            levelObjectsTextureProvider.SetGridObjectTexture();
        }

        #endregion
        

    }
}