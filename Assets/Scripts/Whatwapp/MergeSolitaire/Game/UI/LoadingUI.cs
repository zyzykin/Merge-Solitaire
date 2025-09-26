using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class LoadingUI : MonoBehaviour
    {
        public Slider progressBar;
        public TextMeshProUGUI loadingText;
        public CanvasGroup canvasGroup;
        public float dotUpdateInterval = 0.5f;

        private int _dotCount;

        public void StartLoading(float minTime)
        {
            gameObject.SetActive(true);
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            progressBar.value = 0f;
            UpdateDots();
            StartCoroutine(UpdateDotsCoroutine());
            DOVirtual.Float(0f, 1f, minTime, (v) => { progressBar.value = v; });
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
            var dots = new string('.', _dotCount);
            loadingText.text = "Loading" + dots;
            _dotCount = (_dotCount + 1) % 4;
        }
    }
}