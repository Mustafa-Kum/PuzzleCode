using _Game.Scripts._GameLogic.Data.Visual;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Managers
{
    public class LevelIndexFluidColorManager : SerializedMonoBehaviour
    {
        #region PUBLIC VARIABLES
        
        public LevelIndexFluidSettingContainer levelIndexFluidSettingContainer;
        
        public MeshRenderer fluidMeshRenderer;
        
        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            SetFluidMaterialByLevelIndex();
            SetFluidColorByLevelIndex();
        }

        #endregion

        #region PRIVATE METHODS

        private void SetFluidColorByLevelIndex()
        {
            var fluidDataContainer = levelIndexFluidSettingContainer;

            if (fluidDataContainer.TryGetFluidSettings(fluidDataContainer.playerSavableData.LevelIndex, out var settings))
            {
                var colorWithOpacity = settings.color;
                colorWithOpacity.a = settings.opacity;

                fluidDataContainer.flowFluid.Color = colorWithOpacity;
                fluidDataContainer.flowFluid.Smoothness = settings.smoothness;
                fluidDataContainer.flowFluid.Metallic = settings.metallic;
                fluidDataContainer.flowFluid.Viscosity = settings.viscosity;
            }
            else
            {
                Debug.LogWarning($"Level index {fluidDataContainer.playerSavableData.LevelIndex} no color data.");
                
                if (fluidDataContainer.TryGetDefaultFluidSettings(out var defaultSettings))
                {
                    var colorWithOpacity = defaultSettings.color;
                    colorWithOpacity.a = defaultSettings.opacity;

                    fluidDataContainer.flowFluid.Color = colorWithOpacity;
                    fluidDataContainer.flowFluid.Smoothness = defaultSettings.smoothness;
                    fluidDataContainer.flowFluid.Metallic = defaultSettings.metallic;
                    fluidDataContainer.flowFluid.Viscosity = defaultSettings.viscosity;
                }
                else
                {
                    Debug.LogWarning("No default color data.");
                }
            }
        }

        private void SetFluidMaterialByLevelIndex()
        {
            var fluidDataContainer = levelIndexFluidSettingContainer;

            if (fluidDataContainer.TryGetMaterialSettings(fluidDataContainer.playerSavableData.LevelIndex, out var settings))
            {
                fluidMeshRenderer.material = settings.material;
            }
            else
            {
                Debug.LogWarning($"Level index {fluidDataContainer.playerSavableData.LevelIndex} no material data.");
            }
        }


        #endregion
    }
}