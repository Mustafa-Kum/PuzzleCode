using System.Collections.Generic;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants.RemoteConfigVariableBase;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "RemoteConfigVariablesHolder", menuName = "This Game/Remote Config Variables Holder", order = 1)]
    public class RemoteConfigValuesHolderSo : ScriptableObject
    {
        [SerializeField] private RemoteConfigVariableBase[] remoteConfigValues;

        public RemoteConfigVariableBase[] RemoteConfigValues => remoteConfigValues;
        public void InitRemoteConfigValue(Dictionary<string, object> defaults)
        {
            foreach (var item in remoteConfigValues)
            {
                item.Init(defaults);
            }
        }

        public void DisplayValuesForDebug()
        {
            foreach (var item in remoteConfigValues)
            {
                TDebug.Log("RemoteConfigManager - " + item.NameAndValueForDebug);
            }
        }

        public int IntValueWithRemoteConfigName(string remoteConfigName)
        {
            foreach (var item in remoteConfigValues)
            {
                if (item.RemoteConfigName == remoteConfigName)
                    return (int)item.Value.LongValue;
            }

            DebugLogError(remoteConfigName);
            return 0;
        }

        public float FloatValueWithRemoteConfigName(string remoteConfigName)
        {
            foreach (var item in remoteConfigValues)
            {
                if (item.RemoteConfigName == remoteConfigName)
                    return (float)item.Value.DoubleValue;
            }

            DebugLogError(remoteConfigName);
            return 0;
        }

        public bool BoolValueWithRemoteConfigName(string remoteConfigName)
        {
            foreach (var item in remoteConfigValues)
            {
                if (item.RemoteConfigName == remoteConfigName)
                    return (bool)item.Value.BooleanValue;
            }

            DebugLogError(remoteConfigName);
            return false;
        }

        public string StringValueWithRemoteConfigName(string remoteConfigName)
        {
            foreach (var item in remoteConfigValues)
            {
                if (item.RemoteConfigName == remoteConfigName)
                    return item.Value.StringValue;
            }

            DebugLogError(remoteConfigName);
            return "";
        }

        private void DebugLogError(string remoteConfigName)
        {
            Debug.LogError("!!!!!!! THERE IS NO REMOTE CONFIG ITEM WITH NAME " + remoteConfigName);
            Debug.LogError("!!!!!!! OR the remote config so is not dragged on RemoteConfigValuesSO " + remoteConfigName);
        }
    }
}
