using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class InGameScreenBlackBgAnimation : MonoBehaviour
    {
        [SerializeField] private Image _blackBG;
        [SerializeField] private GameObject _royalBoy;

        private Sequence _blackBgSequence;

        private void OnEnable()
        {
            RectTransform blackBgRectTransform = _blackBG.GetComponent<RectTransform>();
            RectTransform royalBoyRectTransform = _royalBoy.GetComponent<RectTransform>();

            _blackBgSequence = DOTween.Sequence();

            _blackBgSequence.Append(blackBgRectTransform.DOSizeDelta(Vector2.zero, 0.8f)).SetEase(Ease.Linear);

            _blackBgSequence.Append(royalBoyRectTransform.DOLocalMoveY(0, 0.8f));
        }
    }
}