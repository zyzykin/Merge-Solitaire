using DG.Tweening;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class CellVisual : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ColorSettings _colorSettings;
        [SerializeField] private AnimationSettings _animationSettings;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        
        private Vector2Int _coordinates;
        private Color _defaultColor;
        private Color _highlightColor;
        
        public void Init(Vector2Int coordinates)
        {
            _coordinates = coordinates;
            _defaultColor = _colorSettings.GetCellColor(coordinates);
            _highlightColor = _colorSettings.GetCellHighlightColor(coordinates);
        }

        
        public void Highlight()
        {
            Debug.Log("Highlighting cell");
            
             var sequence = DOTween.Sequence();
             sequence.AppendInterval(_animationSettings.HighlightDelay * _coordinates.y);
             sequence.Append(_backgroundRenderer.DOColor(_highlightColor, _animationSettings.HighlightDuration));
             
             sequence.Append(_backgroundRenderer.DOColor(_defaultColor, _animationSettings.HighlightDuration));
             sequence.OnComplete(() =>
             {
                 Debug.Log("Highlight completed");
                 _backgroundRenderer.color = _defaultColor;
             });
             sequence.Play();
        }
        
        
    }
}