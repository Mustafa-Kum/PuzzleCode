using System;
using System.Collections.Generic;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts
{
    [Serializable]
    public class RemoteConfigValues
    {
        public bool isActive;

        // Add your other properties here based on your Remote Config data
        // For example:
        public string packName;
        public DateTime startDate;
        public DateTime endDate;
        public List<string> items;

        // Add any additional methods or constructors as needed.
    }
}