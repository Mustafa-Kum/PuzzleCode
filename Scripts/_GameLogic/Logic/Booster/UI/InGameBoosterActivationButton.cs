using _Game.Scripts._GameLogic.Data.Booster;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class InGameBoosterActivationButton : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private BoosterDataSO boosterDataSO;
        [SerializeField] public BoosterType buttonBoosterType;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Button button;
        
        #endregion

        #region Private Variables

        private Sprite boosterSprite;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            UpdateButtonDisplay();
            
            button.onClick.AddListener(OnBoosterButtonClicked);
         
            EventManager.UIEvents.OnBoosterButtonExistOnUI?.Invoke(buttonBoosterType, button);
            EventManager.BoosterEvents.BoosterAnimationBegan += OnBoosterAnimationBegan;
            EventManager.BoosterEvents.BoosterAnimationEnd += OnBoosterAnimationEnded;
            EventManager.BoosterEvents.BoosterValueChanged += UpdateButtonDisplay;
        }
        
        private void OnDestroy()
        {
            EventManager.BoosterEvents.BoosterAnimationBegan -= OnBoosterAnimationBegan;
            EventManager.BoosterEvents.BoosterAnimationEnd -= OnBoosterAnimationEnded;
            EventManager.BoosterEvents.BoosterValueChanged -= UpdateButtonDisplay;
        }

        #endregion

        #region Private Methods

        private void UpdateButtonDisplay()
        {
            int boosterCount = boosterDataSO.GetBoosterValue(buttonBoosterType);
            button.interactable = boosterDataSO.IsBoosterAvailableWithDictionary(buttonBoosterType);
            countText.text = boosterCount.ToString();
        }
        
        private void OnBoosterButtonClicked()
        {
            EventManager.BoosterEvents.BoosterUIActivationRequested?.Invoke(buttonBoosterType);
            UpdateButtonDisplay();
        }
        
        private void OnBoosterAnimationEnded()
        {
            UpdateButtonDisplay();
        }

        private void OnBoosterAnimationBegan(BoosterType arg0)
        {
            button.interactable = false;
        }
        
        #endregion
    }
}
