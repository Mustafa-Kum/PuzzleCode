using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts.Constants;
using UnityEngine;

namespace _Game.Scripts.RemoteConfig
{
    public abstract class RemoteMonoManager : MonoBehaviour
    {
        [SerializeField] public IntRemoteConfigVariableValue configVariable;
        [SerializeField] protected IRemoteable Remoteable;

        private void Awake()
        {
            Remoteable ??= GetComponent<IRemoteable>();
            
            AssignValueToMono();
        }

        private void OnEnable()
        {
            EventManager.RemoteEvents.RemoteConfigFetched += AssignValueToMono;
        }

        private void OnDisable()
        {
            EventManager.RemoteEvents.RemoteConfigFetched -= AssignValueToMono;
        
        }

        protected abstract void AssignValueToMono();
    }

    public interface IRemoteable
    {
        void AssignValue(int value);
    }
}