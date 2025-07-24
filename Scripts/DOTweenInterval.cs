using DG.Tweening;
using UnityEngine;
using _Game.Scripts.Helper.Services;

namespace _Game.Scripts
{
    [RequireComponent(typeof(DOTweenAnimation))]
    public class DoTweenInterval : MonoBehaviour
    {
        [SerializeField] private float _timeBetween = 5f;

        private DOTweenAnimation _doTweenAnimation;
        private Coroutine _repeatingCoroutine;
        private CoroutineService _coroutineService;

        private void Awake()
        {
            _coroutineService = new CoroutineService(this);
            
            _doTweenAnimation = GetComponent<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            if (_repeatingCoroutine != null)
            {
                StopCoroutine(_repeatingCoroutine);
            }

            _repeatingCoroutine = _coroutineService.StartIntervalRoutine(TweenCoroutineMethod, _timeBetween, () => true);
        }

        private void OnDisable()
        {
            if (_repeatingCoroutine != null)
            {
                StopCoroutine(_repeatingCoroutine);
            }
        }

        private void TweenCoroutineMethod()
        {
            _doTweenAnimation.DORestart();
            _doTweenAnimation.DOPlay();
        }
    }
}