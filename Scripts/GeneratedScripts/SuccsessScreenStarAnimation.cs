using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.GeneratedScripts
{
    public class SuccsessScreenStarAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject _star;
        private Sequence _sequence;
        
        private void Awake()
        {
            StarAnimation();
        }

        private void StarAnimation()
        {
            _sequence = DOTween.Sequence();
            
            RectTransform starRectTransform = _star.GetComponent<RectTransform>();

            _sequence.Append(starRectTransform.DOLocalMoveY(270, 0.5f)).SetEase(Ease.InBounce);
        }
    }
}