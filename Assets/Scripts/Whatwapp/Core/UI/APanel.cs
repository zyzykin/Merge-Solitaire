using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Whatwapp.Core.UI
{
    public abstract class APanel<T> : MonoBehaviour, IPanel<T>
    {
        [Header("Settings")] 
        [SerializeField] protected bool _hideOnStart = true;
        [SerializeField] protected float _fadeDuration = 0.5f;

        [Header("References")] 
        [SerializeField] protected CanvasGroup _canvasGroup;

        protected TweenerCore<float,float,FloatOptions> _tween;

        protected virtual void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            Setup();
        }

        protected virtual void Setup()
        {
            if (_hideOnStart)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
                IsVisible = false;
            }
            else
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
                IsVisible = true;
            }
        }


        public bool IsVisible { get; protected set; }

        public void Toggle()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show(Action onShowComplete = null)
        {
            _tween?.Kill();
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = false;
            IsVisible = true;
            _tween = _canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.alpha = 1;
                    onShowComplete?.Invoke();
                });
        }

        public void Hide(Action onHideComplete = null)
        {
            _tween?.Kill();
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            IsVisible = false;
            _tween = _canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = false;
                    _canvasGroup.alpha = 0;
                    onHideComplete?.Invoke();
                });
        }

        public abstract void SetData(T context);
    }
}