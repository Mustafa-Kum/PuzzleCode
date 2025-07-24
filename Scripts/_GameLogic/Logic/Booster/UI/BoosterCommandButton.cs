using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class BoosterCommandButton : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private BoosterType boosterType; 
        [SerializeField] private Vector2Int boosterIndex;

        #endregion

        #region Private Variables

        private Button _button;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_button == null)
            {
                Debug.LogError("BoosterButton script is not attached to an object with a Button component.");
                return;
            }
        
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDestroy()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(OnButtonPressed);
            }
        }

        #endregion

        #region Private Methods

        private void OnButtonPressed() => EventManager.BoosterEvents.BoosterCommandExecute?.Invoke(boosterType, boosterIndex);

        #endregion
    }
}