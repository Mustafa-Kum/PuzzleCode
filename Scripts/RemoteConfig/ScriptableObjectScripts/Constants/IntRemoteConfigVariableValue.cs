using System.Collections.Generic;
using UnityEngine;

// ... (namespace declaration remains the same)

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants
{
    [CreateAssetMenu(fileName = "IntRemoteConfigValue", menuName = "This Game/Remote Config Value/Int Remote Config Value", order = 1)]
    public class IntRemoteConfigVariableValue : RemoteConfigVariableBase.RemoteConfigVariableBase
    {
        public override void Init(Dictionary<string, object> defaults)
        {
            // Assuming RemoteConfigName is a public string property in the base class
            defaults.Add(RemoteConfigName, (long)int.Parse(defaultValue));
        }

        public override string NameAndValueForDebug => $"{RemoteConfigName} - {(int)Value.LongValue}"; // String interpolation

        protected override bool TypeCheckForEditor(string value)
        {
            return int.TryParse(value, out _); // Use discard '_' as we only care about success/failure
        }

        // Additional Considerations:

        // 1. Error Handling: Consider adding error handling or validation
        //    in case the default value is not a valid integer.

        // 2. Accessing Value: Ensure you have a way to access the actual 
        //    Remote Config value (e.g., through a property or method). 
        //    This might involve fetching the value from FirebaseRemoteConfig.DefaultInstance
        //    using the RemoteConfigName and then casting it to an int.
    }
}