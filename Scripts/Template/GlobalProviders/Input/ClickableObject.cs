using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Input
{
    public class ClickableObject : MonoBehaviour, IClickable, IMatchableAction
    {
        #region Serialized Fields

        [SerializeField] private bool canClick = true;

        [SerializeField] private bool isLazyInitialized = true;

        [ShowInInspector] private IClickableAction[] actions;
        
        #endregion

        #region Private Fields

        private bool isInitialized;

        #endregion

        #region Public Properties

        private bool CanClick => canClick;

        #endregion

        #region Private Methods

        private void EnsureInitialized()
        {
            if (isInitialized) return;

            actions = GetComponentsInChildren<IClickableAction>();
            isInitialized = true;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (isLazyInitialized) return;
            EnsureInitialized();
        }
        
        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += DisableClickAction;
            EventManager.InGameEvents.LevelFail += DisableClickAction;
            EventManager.InGameEvents.LevelStart += EnableClickAction;
            EventManager.InGameEvents.LevelRestart += DisableClickAction;
            EventManager.UIEvents.OnSettingsButtonActivated += DisableClickAction;
            EventManager.UIEvents.OnSettingsButtonDeactivated += EnableClickAction;
            EventManager.BoosterEvents.BoosterUIActivationRequested += DisableClickActionBoosterEvent;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested += EnableClickActionBoosterEvent;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= DisableClickAction;
            EventManager.InGameEvents.LevelFail -= DisableClickAction;
            EventManager.InGameEvents.LevelStart -= EnableClickAction;
            EventManager.InGameEvents.LevelRestart -= DisableClickAction;
            EventManager.UIEvents.OnSettingsButtonActivated += DisableClickAction;
            EventManager.UIEvents.OnSettingsButtonDeactivated += EnableClickAction;
            EventManager.BoosterEvents.BoosterUIActivationRequested -= DisableClickActionBoosterEvent;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested -= EnableClickActionBoosterEvent;
        }
        #endregion

        #region IClickable Implementation

        public void OnClickedDown()
        {
            EnsureInitialized();
            
            if (!CanClick || !isInitialized || actions == null || actions.Length == 0) return;

            foreach (var action in actions)
            {
                action.ClickableActionDown();
            }
        }

        public void OnClickedHold()
        {
            EnsureInitialized();
            
            if (!CanClick || !isInitialized || actions == null || actions.Length == 0) return;

            foreach (var action in actions)
            {
                action.ClickableActionHold();
            }
        }

        public void OnClickedUp()
        {
            //
        }
        
        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            canClick = false;
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            canClick = false;
        }

        #endregion

        #region Private Methods
        
        private void DisableClickAction()
        {
            canClick = false;
        }
        
        private void EnableClickAction()
        {
            canClick = true;
        }
        
        private void DisableClickActionBoosterEvent(BoosterType type)
        {
            if(type != BoosterType.RowChanger) return;
            canClick = false;
        }
        
        private void EnableClickActionBoosterEvent(BoosterType type)
        {
            if (transform.parent.TryGetComponent(out GridTile gridTile))
            {
                if (gridTile.gridObjectType is GridObjectType.Matched or GridObjectType.Obstacle) return;
                canClick = true;
            }
        }
        
        #endregion
    }
}