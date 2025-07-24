using System;
using DG.Tweening;
using PaintIn3D;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class ParticleCallbackProvider : MonoBehaviour
    {
        [SerializeField] public GridObjectTypeColorSettings colorSettings;
        
        #region Public Variables

        [SerializeField] private new ParticleSystem[] particleSystem;
        [SerializeField] private P3dPaintDecal _p3dPaintDecal;

        #endregion
        
        #region Public Methods

        public void PlayParticles(GridObjectType gridObjectType)
        {
            foreach (var particleSystem in particleSystem)
            {
                particleSystem.gameObject.SetActive(true);
                var main = particleSystem.main;
                _p3dPaintDecal.Color = ConvertGridObjectTypeToColor(gridObjectType);
                main.startColor = ConvertGridObjectTypeToColor(gridObjectType);
                particleSystem.Play();
                OnParticleEndCallback(particleSystem, () =>
                {
                    particleSystem.gameObject.SetActive(false);
                });
            }
        }

        #endregion

        #region Private Methods

        private Color ConvertGridObjectTypeToColor(GridObjectType gridObjectType)
        {
            return gridObjectType switch
            {
                GridObjectType.Red => colorSettings.Red,
                GridObjectType.Green => colorSettings.Green,
                GridObjectType.Blue => colorSettings.Blue,
                GridObjectType.Yellow => colorSettings.Yellow,
                GridObjectType.Matched => colorSettings.Matched,
                _ => Color.white
            };
        }
        
        private void OnParticleEndCallback(ParticleSystem _particleSystem, Action callback)
        {
            var sequence = DOTween.Sequence();
            var main = _particleSystem.main;
            var duration = main.duration;
            sequence.AppendInterval(duration).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        #endregion
    }
}

[System.Serializable]
public class GridObjectTypeColorSettings
{
    public Color Red = Color.red;
    public Color Green = Color.green;
    public Color Blue = Color.blue;
    public Color Yellow = Color.yellow;
    public Color Matched = Color.cyan;
}