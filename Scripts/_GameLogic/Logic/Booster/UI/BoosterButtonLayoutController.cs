using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class BoosterButtonLayoutController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private Vector2Int currentGridConfig;

        [SerializeField] private List<BoosterCommandButton> boosterCommandButtons;
        
        [SerializeField] private HorizontalLayoutGroup boosterButtonLayoutGroup;

        #endregion

        #region Const Variables

        const float minSpacing = -250f;
        const float maxSpacing = -250f;
        const float maxSize = 6f;
        const float minSize = 4f;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= OnLevelLoaded;
        }

        #endregion

        #region Private Methods

        private void OnLevelLoaded(GameObject _level)
        {
            currentGridConfig = _level.GetComponent<LevelReferenceHolder>().CurrentGridConfig;
            ConfigureBoosterButtons();
        }

        private void ConfigureBoosterButtons()
        {
            int activeButtonsCount = currentGridConfig.x;

            var buttonsToActivate = boosterCommandButtons.Take(activeButtonsCount);
            var buttonsToDeactivate = boosterCommandButtons.Skip(activeButtonsCount);
    
            foreach(var button in buttonsToActivate)
            {
                button.gameObject.SetActive(true);
            }
    
            foreach(var button in buttonsToDeactivate)
            {
                button.gameObject.SetActive(false);
            }
            
            float t = Mathf.InverseLerp(minSize, maxSize, activeButtonsCount);
            boosterButtonLayoutGroup.spacing = Mathf.Lerp(maxSpacing, minSpacing, t);
        }

        #endregion
    }
}
