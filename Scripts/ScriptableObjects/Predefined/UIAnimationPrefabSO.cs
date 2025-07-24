using System.Collections.Generic;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = "UIAnimationPrefabSO", menuName = "ThisGame/UIAnimationPrefabSO", order = 0)]
    public class UIAnimationPrefabSO : SerializedScriptableObject
    {
        [Title("Collectable Animation Object Prefabs")]
        public Dictionary<GridObjectType, GameObject> _uiPrefabs;
        
        public GameObject GetUIPrefab(GridObjectType gridObjectType)
        {
            if (_uiPrefabs.TryGetValue(gridObjectType, out GameObject animationPrefab))
            {
                return animationPrefab;
            }
            else
            {
                Debug.LogWarning("Animation Prefab not found for the given CollectableType.");
                return null;
            }
        }
    }
}