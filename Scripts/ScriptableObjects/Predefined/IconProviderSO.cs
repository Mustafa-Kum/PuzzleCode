using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = "IconProviderSO", menuName = "ThisGame/IconProviderSO", order = 0)]
    public class IconProviderSO : ScriptableObject
    {
        public List<Sprite> cubeIcons;
        public Dictionary<CubeType, Sprite> cubeIconDict;
        
        public Sprite GetCollectableIcon(CubeType cubeType)
        {
            if (cubeIconDict.TryGetValue(cubeType, out Sprite icon))
            {
                return icon;
            }
            else
            {
                Debug.LogWarning("Icon not found for the given CollectableType.");
                return null;
            }
        }

        public Sprite GetGunSpriteWithIndex(int i)
        {
            return cubeIcons[i];
        }
    }

    [Serializable]
    public enum CubeType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Obstacle
    }
}