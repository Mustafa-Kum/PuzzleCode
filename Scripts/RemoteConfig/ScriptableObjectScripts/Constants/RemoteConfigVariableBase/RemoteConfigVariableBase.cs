using System.Collections.Generic;
using Firebase.RemoteConfig;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants.RemoteConfigVariableBase
{
    public abstract class RemoteConfigVariableBase : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [FormerlySerializedAs("DeveloperDescription")]
        [InfoBox("The name of the ScriptabeObject should be same with the key value in FirebaseRemote Config",
            InfoMessageType.Warning)]

        [Multiline] public string developerDescription = "";
#endif

        //[Space(12), HideLabel]
        //[Title("Dynamic default message", "Use $ to indicate a member string as default message")]
        [ValidateInput("TypeCheckForEditor", "$WrongTypeMessage", InfoMessageType.Error)]
        [SerializeField] protected string defaultValue;


        public string RemoteConfigName => name;

        public abstract void Init(Dictionary<string, object> defaults);

        public abstract string NameAndValueForDebug { get; }

        public ConfigValue Value => FirebaseRemoteConfig.DefaultInstance.GetValue(RemoteConfigName);
        
        public int GetValueAsInt()
        {
            return (int) Value.DoubleValue;
        }

        protected abstract bool TypeCheckForEditor(string value);

        protected string WrongTypeMessage => "Default Value type is wrong!!!";
    }
}
