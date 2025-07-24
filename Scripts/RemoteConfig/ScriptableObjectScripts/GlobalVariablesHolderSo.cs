using System.Collections.Generic;
using _Game.Scripts.Managers.ScriptableObjectScripts;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "GlobalVariablesHolderSO", menuName = "FirebaseRemoteConfig/GlobalVariablesHolderSO", order = 0)]
    public class GlobalVariablesHolderSo : ScriptableObject
    {
        public BoolVariable IsFirebaseInitialized;
        public RemoteConfigValuesHolderSo RemoteConfigValuesHolder;

        public void InitRemoteConfigValues(Dictionary<string, object> defaults)
        {
            RemoteConfigValuesHolder.InitRemoteConfigValue(defaults);
        }
    }
}