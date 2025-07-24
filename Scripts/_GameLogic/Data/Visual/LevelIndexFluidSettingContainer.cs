using System;
using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.Saveable;
using FLOW;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

namespace _Game.Scripts._GameLogic.Data.Visual
{
    [CreateAssetMenu(fileName = "LevelIndexFluidSettingContainer", menuName = "ThisGame/LevelIndexFluidSettingContainer", order = 1)]
    public class LevelIndexFluidSettingContainer : SerializedScriptableObject
    {
        #region INSPECTOR VARIABLES

        public PlayerSavableData playerSavableData;
        
        public FlowFluid flowFluid;
        
        public readonly Dictionary<Vector2Int, FluidSettings> LevelIndexToColor = new();

        public readonly Dictionary<Vector2Int, MaterialSettings> LevelIndexToMaterial = new();

        #endregion

        #region PUBLIC METHODS

        public bool TryGetFluidSettings(int levelIndex, out FluidSettings settings)
        {
            settings = LevelIndexToColor.FirstOrDefault(x => levelIndex >= x.Key.x && levelIndex <= x.Key.y).Value;
            return settings != null;
        }
        
        public bool TryGetDefaultFluidSettings(out FluidSettings settings)
        {
            settings = LevelIndexToColor.FirstOrDefault(x => x.Key is { x: 0, y: 0 }).Value;
            return settings != null;
        }

        public bool TryGetMaterialSettings(int levelIndex, out MaterialSettings settings)
        {
            settings = LevelIndexToMaterial.FirstOrDefault(x => levelIndex >= x.Key.x && levelIndex <= x.Key.y).Value;
            return settings != null;
        }

        #endregion
    }
    
    [Serializable]
    public class FluidSettings
    {
        public Color color;
        [Range(0, 1)] public float smoothness;
        [Range(0, 1)] public float metallic;
        [Range(0, 1)] public float viscosity;
        [Range(0, 1)] public float opacity;
    }
    
    [Serializable]
    public class MaterialSettings
    {
        public Material material;
    }
}