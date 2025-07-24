using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts._GameLogic.Logic.Booster.UI;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Manager.Booster
{
    public class BoosterUIManager : SerializedMonoBehaviour
    {
        #region Public Variables
        
        [ListDrawerSettings(ShowFoldout = true)]
        public List<BoosterTypeEventsWrapper> boosterTypeUIEventsList = new List<BoosterTypeEventsWrapper>();
        public readonly Dictionary<BoosterType, Button> BoosterTypeButtonDictionary = new Dictionary<BoosterType, Button>();
        
        [SerializeField] private PlayerSavableData playerSavableData;
        [SerializeField] private BoosterUnlockLevels boosterUnlockLevels;
        [SerializeField] private Transform _boosterTutorialHand;
        [SerializeField] private LevelList_SO levelListSo;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.BoosterEvents.BoosterUIActivationRequested += OnActivateBoosterPanel;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested += OnDeactivateBoosterPanel;
            EventManager.BoosterEvents.BoosterAnimationBegan += OnDeactivateBoosterPanel;
            EventManager.UIEvents.OnBoosterButtonHighlighted += AnimateSelectedBoosterButton;
            EventManager.GridEvents.OnPathFound += DisableBoosterActivationButtons;
            EventManager.InGameEvents.LevelStart += UpdateBoosterButtonInteractivity;
            //EventManager.BoosterEvents.BoosterValueChanged += UpdateBoosterValue;
        }

        private void OnDisable()
        {
            EventManager.BoosterEvents.BoosterUIActivationRequested -= OnActivateBoosterPanel;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested -= OnDeactivateBoosterPanel;
            EventManager.BoosterEvents.BoosterAnimationBegan -= OnDeactivateBoosterPanel;
            EventManager.UIEvents.OnBoosterButtonHighlighted -= AnimateSelectedBoosterButton;
            EventManager.GridEvents.OnPathFound -= DisableBoosterActivationButtons;
            EventManager.InGameEvents.LevelStart -= UpdateBoosterButtonInteractivity;
            //EventManager.BoosterEvents.BoosterValueChanged -= UpdateBoosterValue;
        }

        #endregion

        #region Private Methods

        private void OnActivateBoosterPanel(BoosterType boosterType)
        {
            var boosterEventsWrapper = boosterTypeUIEventsList.Find(wrapper => wrapper.boosterType == boosterType);
            
            if (boosterEventsWrapper?.boosterUIEvents != null)
            {
                boosterEventsWrapper.boosterUIEvents.onActivationRequested?.Invoke();
            }
            else
            {
                TDebug.LogWarning($"BoosterType: {boosterType} not found in boosterTypeEventsList");
            }
        }
        
        private void OnDeactivateBoosterPanel(BoosterType boosterType)
        {
            var boosterEventsWrapper = boosterTypeUIEventsList.Find(wrapper => wrapper.boosterType == boosterType);
            
            if (boosterEventsWrapper?.boosterUIEvents != null)
            {
                boosterEventsWrapper.boosterUIEvents.onDeactivationRequested?.Invoke();
            }
            else
            {
                TDebug.LogWarning($"BoosterType: {boosterType} not found in boosterTypeEventsList");
            }
        }
        
        private void DisableBoosterActivationButtons(List<GridTile> gridTiles)
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                BoosterTypeButtonDictionary.ForEach((keyValuePair) =>
                {
                    keyValuePair.Value.interactable = false;
                });
                
                BoosterButtonAnimationProvider.StopHighlightAnimation();
            });
        }
        
        private void AnimateSelectedBoosterButton(BoosterType eventBoosterType)
        {
            if(BoosterTypeButtonDictionary.TryGetValue(eventBoosterType, out var button))
            {
                BoosterButtonAnimationProvider.AnimateHighlight(button.GetComponent<RectTransform>(), 1.2f, 0.5f, _boosterTutorialHand);
            }
            else 
            {
                Debug.Log($"No button was found for {eventBoosterType}");
            }
        }
        
        private Button GetBoosterButton(int index)
        {
            return BoosterTypeButtonDictionary[boosterTypeUIEventsList[index].boosterType];
        }
        
        [Button]
        private void UpdateBoosterButtonInteractivity()
        {
            bool allLevelsCompleted = levelListSo.IsAllLevelsCompleted();

            foreach (var mapping in boosterUnlockLevels.boosterLevelMappings)
            {
                int buttonIndex = mapping.boosterType switch
                {
                    BoosterType.RowChanger => 0,
                    BoosterType.GridTileMatcher => 1,
                    _ => -1
                };

                if (buttonIndex == -1) continue;

                var button = GetBoosterButton(buttonIndex);
                bool isBoosterUnlocked = allLevelsCompleted || playerSavableData.LevelIndex >= mapping.unlockLevel;

                button.interactable = isBoosterUnlocked;
                
                button.GetComponent<Image>().sprite = isBoosterUnlocked ? mapping.boosterSprite : mapping.boosterSpriteLocked;
                
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.sizeDelta = isBoosterUnlocked ? new Vector2(160, 160) : new Vector2(140, 140);
                rectTransform.anchoredPosition = new Vector2(0, -9.21f);

                if (!isBoosterUnlocked && !allLevelsCompleted)
                {
                    DOVirtual.DelayedCall(0.01f, () => button.interactable = false);
                }
            }
        }
        
        #endregion
    }

    
    #region Inner Class for Booster Events

    [Serializable]
    public class BoosterTypeUIEvents
    {
        public UnityEvent onActivationRequested = new UnityEvent();
        public UnityEvent onDeactivationRequested = new UnityEvent();
    }
    
    [Serializable]
    public class BoosterTypeEventsWrapper
    {
        public BoosterType boosterType;
        public BoosterTypeUIEvents boosterUIEvents;
    }

    #endregion
}
