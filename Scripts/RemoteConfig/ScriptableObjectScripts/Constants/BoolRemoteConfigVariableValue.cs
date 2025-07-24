using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants
{
    [CreateAssetMenu(fileName = "BoolRemoteConfigValue", menuName = "This Game/Remote Config Value/Bool Remote Config Value", order = 1)]
    public class BoolRemoteConfigVariableValue : RemoteConfigVariableBase.RemoteConfigVariableBase
    {
        public override void Init(Dictionary<string, object> defaults)
        {
            defaults.Add(RemoteConfigName, bool.Parse(defaultValue)); 
        }

        public override string NameAndValueForDebug => $"{RemoteConfigName} - {Value.BooleanValue}";

        protected override bool TypeCheckForEditor(string value)
        {
            return bool.TryParse(value, out _); // More efficient and handles variations like "True", "False", etc.
        }
    }
}