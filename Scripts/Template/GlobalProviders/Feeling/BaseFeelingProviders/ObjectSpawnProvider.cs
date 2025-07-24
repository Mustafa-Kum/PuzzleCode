using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using DG.Tweening;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders
{
    public class ObjectSpawnProvider : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField] private LevelList_SO _levelListSO;
        [SerializeField] private Transform _parentTransform;
        
        #endregion

        #region Private Region

        private Sequence _spawnSequence;
        private GameObject _objectToSpawnPrefab;
        private GameObject _spawnedObject;
        private Level_SO _levelSO;
        private List<GridTile> _gridTileList;
        #endregion

        private void Awake()
        {
            _levelSO = _levelListSO.GetCurrentLevelSO();
            
            _objectToSpawnPrefab = _levelSO.inLevelObjectsConfigurationsData.ObjectSpawn;
            
            SpawnObjectInitiliazer();
        }

        private void SpawnObjectInitiliazer()
        {
            if (_objectToSpawnPrefab != null)
            {
                _spawnedObject = Instantiate(_objectToSpawnPrefab, _parentTransform.position, _objectToSpawnPrefab.transform.rotation, _parentTransform);
                _spawnedObject.transform.localScale = Vector3.zero;
            }
        }
        
        public void ObjectSpawnAnimation()
        {
            if (_spawnSequence == null || !_spawnSequence.IsActive() || _spawnSequence.IsComplete())
            {
                _spawnSequence = DOTween.Sequence();
            }
            else
            {
                _spawnSequence.Kill();
                _spawnSequence = DOTween.Sequence();
            }

            if (_spawnedObject != null)
            {
                EventManager.GridEvents.OnPathObjectSpawned?.Invoke();
                _spawnSequence.Append(_spawnedObject.transform.DOScale(new Vector3(0.1f,0.1f, 0.1f) , 0f)
                    .SetEase(Ease.InOutBack));
                _spawnSequence.Append(_spawnedObject.transform.DOScale(new Vector3(0.9f,0.9f, 0.9f) , 2f)
                    .SetEase(Ease.InOutBack));
            }
        }

    }
}
