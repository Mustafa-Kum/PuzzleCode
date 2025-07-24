using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts;
using Firebase.Crashlytics;

namespace _Game.Scripts.RemoteConfig.Base
{
    public sealed class RemoteConfigManager : BaseRemoteConfigService
    {
        // Initialize remote config, and set the default values.
        public override void Init(GlobalVariablesHolderSo globalVariables)
        {
            base.Init(globalVariables);
            TDebug.Log("RemoteConfigManager - Initializing");

            Dictionary<string, object> defaults = new Dictionary<string, object>();
            if (globalVariables.RemoteConfigValuesHolder == null)
                TDebug.LogError("RemoteConfigManager - globalVariables.RemoteConfigValuesHolder is null");
            
            globalVariables.InitRemoteConfigValues(defaults);
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(_ =>
            {
                FetchDataAsync(defaults);
            });
        }

        private Task FetchDataAsync(Dictionary<string, object> defaults)
        {
            TDebug.Log("RemoteConfigManager - Fetching data...");

            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);

            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                TDebug.Log("RemoteConfigManager - Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                TDebug.LogError("RemoteConfigManager - Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activationTask =>
                {
                    TDebug.Log("RemoteConfigManager - Activated successfully!");
                    ApplyConfigValues();
                    //DisplayData();
                    isFirebaseInitialized.SetValue(true);
                    EventManager.RemoteEvents.RemoteConfigFetched?.Invoke();
                });
            }
        }

        private void ApplyConfigValues()
        {
            try
            {
                foreach (var variable in globalVariables.RemoteConfigValuesHolder.RemoteConfigValues)
                {
                    string key = variable.RemoteConfigName;
                    if (RemoteConfigKeyExists(key))
                    {
                        var value = GetRemoteConfigValue(key);
                        TDebug.Log(value.Source != ValueSource.DefaultValue // Check that it does not return the default value
                            ? $"RemoteConfigManager - {key} - {value.DoubleValue}"
                            : $"RemoteConfigManager - Default value used for {key}");
                    }
                    else
                    {
                        TDebug.LogError($"RemoteConfigManager - Key not found: {key}");
                    }
                }
            }
            catch (Exception ex)
            {
                TDebug.LogError($"RemoteConfigManager - Error fetching remote config values: {ex.Message}");
                Crashlytics.LogException(ex);
            }
        }

        
        private void DisplayData()
        {
            globalVariables.RemoteConfigValuesHolder.DisplayValuesForDebug();
        }
    }
}
