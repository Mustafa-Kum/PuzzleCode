using System;
using System.Linq;
using _Game.Scripts.Managers.ScriptableObjectScripts;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts;
using Firebase.RemoteConfig;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.Base
{
    public abstract class BaseRemoteConfigService
    {
        protected BoolVariable isFirebaseInitialized;
        protected GlobalVariablesHolderSo globalVariables;

        // Initialize service with Remote Config data.
        public virtual void Init(GlobalVariablesHolderSo globalVariables)
        {
            this.globalVariables = globalVariables;
            isFirebaseInitialized = globalVariables.IsFirebaseInitialized;
        }
        
        protected ConfigValue GetRemoteConfigValue(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        }

        // Helper method to check for Remote Config key existence.
        protected bool RemoteConfigKeyExists(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.Keys.Contains(key);
        }
    }
}