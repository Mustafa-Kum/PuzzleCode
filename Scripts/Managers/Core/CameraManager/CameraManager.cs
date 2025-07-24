using System;
using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers
{
    public class CameraManager : SerializedMonoBehaviour
    {
        #region Public Variables

        public Dictionary<GameState, CinemachineVirtualCamera> _virtualCameraDictionary = new();

        public Dictionary<Vector2Int, List<CameraGroupedData>> _cameraOffsetDictionary = new();

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region Event Subscriptions

        private void SubscribeToEvents()
        {
            EventManager.InGameEvents.GameStarted += HandleOnGameStart;
            EventManager.InGameEvents.LevelStart += HandleOnLevelStart;
            EventManager.InGameEvents.LevelSuccess += HandleOnLevelEnd;
            EventManager.InGameEvents.EndMetaStart += HandleOnEndMetaStart;
            EventManager.InGameEvents.LevelLoaded += HandleOnLevelLoaded;
            EventManager.BoosterEvents.BoosterUIActivationRequested += BoosterUIActivated;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested += BoosterUIDeactivated;
            EventManager.GridEvents.OnGridConfigCreated += HandleOnGridConfigCreated;
        }

        private void BoosterUIDeactivated(BoosterType arg0)
        {
            foreach (var cameraGroupedData in _cameraOffsetDictionary)
            {
                foreach (var camera in cameraGroupedData.Value)
                {
                    camera.boosterCamera.enabled = false;
                }
            }
        }

        private void BoosterUIActivated(BoosterType arg0)
        {
            foreach (var cameraGroupedData in _cameraOffsetDictionary)
            {
                foreach (var camera in cameraGroupedData.Value)
                {
                    camera.boosterCamera.enabled = true;
                }
            }
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.InGameEvents.GameStarted -= HandleOnGameStart;
            EventManager.InGameEvents.LevelStart -= HandleOnLevelStart;
            EventManager.InGameEvents.LevelSuccess -= HandleOnLevelEnd;
            EventManager.InGameEvents.EndMetaStart -= HandleOnEndMetaStart;
            EventManager.InGameEvents.LevelLoaded -= HandleOnLevelLoaded;
            EventManager.BoosterEvents.BoosterUIActivationRequested -= BoosterUIActivated;
            EventManager.BoosterEvents.BoosterUIDeactivationRequested -= BoosterUIDeactivated;
            EventManager.GridEvents.OnGridConfigCreated -= HandleOnGridConfigCreated;
        }

        #endregion

        #region Event Handlers

        private void HandleOnGameStart() => SwitchVirtualCamera(GameState.LevelLoaded);

        private void HandleOnLevelStart() => SwitchVirtualCamera(GameState.LevelStart);

        private void HandleOnLevelLoaded(GameObject arg0) => HandleOnGameStart();

        private void HandleOnLevelEnd() => SwitchVirtualCamera(GameState.LevelEnd);

        private void HandleOnEndMetaStart() => SwitchVirtualCamera(GameState.EndMetaStart);
        
        
        #endregion

        #region Camera Management

        private void SwitchVirtualCamera(GameState state)
        {
            DisableAllCameras();

            if (_virtualCameraDictionary.ContainsKey(state)) _virtualCameraDictionary[state].gameObject.SetActive(true);
        }

        private void DisableAllCameras()
        {
            foreach (var cam in _virtualCameraDictionary.Values) cam.gameObject.SetActive(false);
        }

        private void HandleOnGridConfigCreated(Vector2Int containerGridConfigData)
        {
            if (_cameraOffsetDictionary.TryGetValue(containerGridConfigData, out var cameraGroupedDataList))
                foreach (var cameraGroupedData in cameraGroupedDataList)
                {
                    UpdateCameraPosition(cameraGroupedData.virtualCamera, cameraGroupedData.offset);
                    UpdateCameraPosition(cameraGroupedData.boosterCamera, cameraGroupedData.offset);

                    cameraGroupedData.boosterCamera.enabled = false;
                    
                    foreach (var camera in _virtualCameraDictionary.Values)
                    {
                        UpdateCameraPosition(camera, cameraGroupedData.offset);
                    }
                }
            else
                Debug.LogWarning($"Key {containerGridConfigData} not found in _cameraOffsetDictionary.");
        }

        private void UpdateCameraPosition(Component _camera, Vector3 offset)
        {
            if (_camera != null)
                _camera.transform.position = offset;
            else
                Debug.LogWarning($"{_camera} camera is null in CameraGroupedData.");
        }

        #endregion
    }
}

[Serializable]
public class CameraGroupedData
{
    public Camera boosterCamera;
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 offset;
}