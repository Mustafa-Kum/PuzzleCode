using _Game.Scripts._GameLogic.Data.Game;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.UI
{
    public class PlayerMoveViewManager : MonoBehaviour
    {
        #region Public Variables

        public PlayerMoveData playerMoveData;
        public TextMeshProUGUI currentMovesText;

        #endregion
        
        #region Unity Methods
        
        private void OnEnable()
        {
            EventManager.InGameEvents.LevelLoaded += OnLevelLoaded;
            EventManager.UIEvents.OnMoveCounterChanged += UpdateCurrentMovesText;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= OnLevelLoaded;
            EventManager.UIEvents.OnMoveCounterChanged -= UpdateCurrentMovesText;
        }

        #endregion

        #region Private Methods

        private void OnLevelLoaded(GameObject level)
        {
            playerMoveData.ResetMoves();
            UpdateCurrentMovesText();
        }
        
        private void UpdateCurrentMovesText(int currentMoves = 0)
        {
            currentMovesText.text = playerMoveData.GetCurrentMoves().ToString();
            
            currentMovesText.transform.DOScale(1.5f, 0.2f).OnComplete(() =>
            {
                currentMovesText.transform.DOScale(1f, 0.2f);
            }).SetEase(Ease.OutQuad);
        }

        #endregion
    }
}