using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridTile : MonoBehaviour
    {
        #region Public Variables
        
        [ReadOnly] public GridObjectType gridObjectType;
        
        [ShowInInspector] private IMatchableAction[] matchableActions;
        
        [ShowInInspector] private IPathAction[] pathActions;
        
        public Vector2Int gridPosition;
        
        public bool HasBeenMatched { get; set; }

        #endregion

        #region Properties

        private GameObject GridObjectVariant { get; set; }

        private GridObjectType GridObjectType
        {
            get => gridObjectType;
            set => gridObjectType = value;
        }
        
        public Vector2Int GridPosition
        {
            get => gridPosition;
            set => gridPosition = value;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            matchableActions = GetComponentsInChildren<IMatchableAction>();
            pathActions = GetComponentsInChildren<IPathAction>();
        }

        #endregion

        #region Public Methods
        
        public void SetGridPosition(Vector2Int _gridPosition) => GridPosition = _gridPosition;

        public void SetGridObjectType(GridObjectType _gridObjectType) => GridObjectType = _gridObjectType;

        public void SetGridObjectVariant(GameObject _gridObjectVariant) => GridObjectVariant = _gridObjectVariant;
        
        public GridObjectType GetGridObjectType() => gridObjectType;
        
        public void MatchableAction()
        {
            var prevGridObjectType = gridObjectType;
            
            SetGridObjectType(GridObjectType.Matched);
            
            foreach (var action in matchableActions)
            {
                action.MatchAction(this, prevGridObjectType);
            }
        }
        
        public void ExcludePathAction()
        {
            var prevGridObjectType = gridObjectType;
            
            SetGridObjectType(GridObjectType.Matched);
            
            foreach (var action in matchableActions)
            {
                action.ExcludePathFoundAction(this, prevGridObjectType);
            }
        }
        
        public void PathAction()
        {
            foreach (var action in pathActions)
            {
                action.PathAction();
            }
        }

        #endregion
    }
}