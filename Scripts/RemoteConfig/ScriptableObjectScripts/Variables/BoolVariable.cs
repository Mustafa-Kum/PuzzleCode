using UnityEngine;

namespace _Game.Scripts.Managers.ScriptableObjectScripts
{
    [CreateAssetMenu]
    public class BoolVariable : ScriptableObject
    {
        #if UNITY_EDITOR
        
        [Multiline]
        public string DeveloperDescription = "";
        
        #endif
        
        public bool Value;

        #region PUBLIC METHODS

        public void SetValue(bool value)
        {
            Value = value;
        }
        
        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }
        
        public void ApplyChange(bool amount)
        {
            Value = amount;
        }
        
        public void ApplyChange(BoolVariable amount)
        {
            Value = amount.Value;
        }

        #endregion
       
    }
}