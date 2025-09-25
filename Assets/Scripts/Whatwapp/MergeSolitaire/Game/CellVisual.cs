using DG.Tweening;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game
{
    public class CellVisual : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private AnimationSettings animationSettings;

        [Header("References")] [SerializeField]
        private SpriteRenderer backgroundRenderer;

        private Vector2Int _coordinates;
        private Color _defaultColor;
        private Color _highlightColor;

        public void Init(Vector2Int coordinates)
        {
            _coordinates = coordinates;
            _defaultColor = colorSettings.GetCellColor();
            _highlightColor = colorSettings.GetCellHighlightColor();
        }

        public void Highlight()
        {
            Debug.Log("Highlighting cell");

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(animationSettings.HighlightDelay * _coordinates.y);
            sequence.Append(backgroundRenderer.DOColor(_highlightColor, animationSettings.HighlightDuration));

            sequence.Append(backgroundRenderer.DOColor(_defaultColor, animationSettings.HighlightDuration));
            sequence.OnComplete(() =>
            {
                Debug.Log("Highlight completed");
                backgroundRenderer.color = _defaultColor;
            });
            sequence.Play();
        }
    }
}