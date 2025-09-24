using DG.Tweening;
using UnityEngine;
using TMPro;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockVisual : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ColorSettings _colorSettings;
        [SerializeField] private AnimationSettings _animationSettings;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshPro _text;
        
        
        private Vector3 _defaultScale;
        
        public void Init(BlockValue value, BlockSeed seed)
        {
            _defaultScale = transform.localScale;
            _spriteRenderer.sprite = _colorSettings.GetBlockSprite(seed);
            _text.text = value.Symbol();
        }
        
        public void ShakeScale()
        {
            transform.DOShakeScale(_animationSettings.BlockShakeDuration, _animationSettings.BlockShakeStrength)
                .OnComplete(() => transform.localScale = _defaultScale);
        }
    }
}