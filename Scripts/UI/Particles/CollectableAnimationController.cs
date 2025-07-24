using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Logic.Manager.Objective;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using AssetKits.ParticleImage;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.UI.Particles
{
    public class CollectableAnimationController : SerializedMonoBehaviour
    {
        #region Inspector Fields

        public UIAnimationPrefabSO UIAnimationPrefabSo;

        public Canvas UICanvas;

        public Dictionary<GridObjectType, RectTransform> UIPrefabDictionary;

        #endregion

        #region Private Variables

        private Sequence _sequence;

        private RectTransform _uiCanvasRectTransform;

        public LevelProgressionItems _currentLevelObjectives;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _uiCanvasRectTransform = UICanvas.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            EventManager.GridEvents.GridObjectMatchedType += AnimateCoroutine;
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated += FetchLevelProgressItems;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.GridObjectMatchedType -= AnimateCoroutine;
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated -= FetchLevelProgressItems;
        }

        #endregion

        #region Private Methods

        private ParticleImage CreateParticleImage(GridObjectType type, Vector2 position)
        {
            var _viewTransform = _uiCanvasRectTransform;

            if (_viewTransform == null) return null;

            var _uiPrefab = UIAnimationPrefabSo.GetUIPrefab(type);

            var particleImage = Instantiate(_uiPrefab, position, Quaternion.identity, _viewTransform)
                .GetComponent<ParticleImage>();

            if (UIPrefabDictionary.TryGetValue(type, out var value))
            {
                particleImage.attractorTarget = value;
            }

            return particleImage;
        }


        private void AnimateCoroutine(GridObjectType type, Vector3 worldPosition)
        {
            if (!_currentLevelObjectives.Objectives.Any(o => o.Type == type && o.RequiredCount > 0)) return;
            {
                var canvasPosition = ConvertWorldPositionToCanvasPosition(worldPosition);
            
                var currentParticleImage = CreateParticleImage(type, canvasPosition);

                if (currentParticleImage == null) return;

                currentParticleImage.onLastParticleFinished.AddListener(() =>
                {
                    Destroy(currentParticleImage.gameObject);
                });
            }
        }


        private Vector2 ConvertWorldPositionToCanvasPosition(Vector3 worldPosition)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

            return screenPoint;
        }


        private void FetchLevelProgressItems(List<ObjectiveItem> items)
        {
            _currentLevelObjectives.Objectives = items;
        }

        #endregion
    }
}