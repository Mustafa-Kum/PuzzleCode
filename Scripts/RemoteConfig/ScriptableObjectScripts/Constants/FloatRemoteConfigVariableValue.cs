using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants
{
    [CreateAssetMenu(fileName = "FloatRemoteConfigValue", menuName = "This Game/Remote Config Value/Float Remote Config Value", order = 1)]
    public class FloatRemoteConfigVariableValue : RemoteConfigVariableBase.RemoteConfigVariableBase
    {
        public override void Init(Dictionary<string, object> defaults)
        {
            // Ensure defaultValue is a string representing a valid float
            if (float.TryParse(defaultValue, out var defaultValueFloat))
            {
                defaults.Add(RemoteConfigName, defaultValueFloat);
            }
            else
            {
                Debug.LogError($"Invalid default value for float Remote Config: {RemoteConfigName}");
            }
        }

        public override string NameAndValueForDebug 
        {    
            get    
            {        
                var floatValue = (float)Value.DoubleValue; // double to float conversion
                return $"{RemoteConfigName} - {floatValue:F2}";  
            } 
        }

        protected override bool TypeCheckForEditor(string value)
        {
            return float.TryParse(value, out _);
        }
    }
}