using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants
{
    [CreateAssetMenu(fileName = "String Remote Config Value",
        menuName = "This Game/Remote Config Value/String Remote Config Value", order = 1)]
    public class StringRemoteConfigVariable : RemoteConfigVariableBase.RemoteConfigVariableBase
    {
        public override void Init(Dictionary<string, object> defaults)
        {
            defaults.Add(RemoteConfigName, defaultValue);
        }

        public override string NameAndValueForDebug => (RemoteConfigName + " - " + Value.StringValue);

        protected override bool TypeCheckForEditor(string value)
        {
            int number;

            if (value == "true" || value == "false")
                return false;
            else if (int.TryParse(value, out number))
            {
                return false;
            }

            return true;
        }
    }
}