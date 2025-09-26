using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Whatwapp.Core.UI
{
    public abstract class APanel<T> : MonoBehaviour, IPanel<T>
    {
        [Header("Settings")] [SerializeField] protected bool hideOnStart = true;
        [SerializeField] protected float fadeDuration = 0.5f;

        [Header("References")] [SerializeField]
        protected CanvasGroup canvasGroup;

        protected TweenerCore<float, float, FloatOptions> Tween;

        protected virtual void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            Setup();
        }

        protected virtual void Setup()
        {
            if (hideOnStart)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                IsVisible = false;
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
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
            Tween?.Kill();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = false;
            IsVisible = true;
            Tween = canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = true;
                    canvasGroup.alpha = 1;
                    onShowComplete?.Invoke();
                });
        }

        public void Hide(Action onHideComplete = null)
        {
            Tween?.Kill();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            IsVisible = false;
            Tween = canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.alpha = 0;
                    onHideComplete?.Invoke();
                });
        }

        public abstract void SetData(T context);
    }
}