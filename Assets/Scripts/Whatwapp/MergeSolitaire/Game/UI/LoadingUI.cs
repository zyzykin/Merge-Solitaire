using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class LoadingUI : MonoBehaviour
    {
        public Slider progressBar; // Прогресс-бар
        public TextMeshProUGUI loadingText; // Текст "Loading..."
        public CanvasGroup canvasGroup; // Для fade-out
        public float dotUpdateInterval = 0.5f; // Интервал обновления точек

        private float _minLoadingTime;
        private int _dotCount = 0;
        private Coroutine _dotCoroutine;

        public void StartLoading(float minTime)
        {
            _minLoadingTime = minTime;
            gameObject.SetActive(true);
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            progressBar.value = 0f;
            UpdateDots();
            _dotCoroutine = StartCoroutine(UpdateDotsCoroutine());
            DOVirtual.Float(0f, 1f, minTime, (v) =>
            {
                progressBar.value = v;
            });
        }

        private IEnumerator UpdateDotsCoroutine()
        {
            while (true)
            {
                UpdateDots();
                yield return new WaitForSeconds(dotUpdateInterval);
            }
        }

        private void UpdateDots()
        {
            string dots = new string('.', _dotCount);
            loadingText.text = "Loading" + dots;
            _dotCount = (_dotCount + 1) % 4;
        }
    }
}