using DG.Tweening;
using UnityEngine;
using TMPro;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockVisual : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private AnimationSettings animationSettings;

        [Header("References")] [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField] private TextMeshPro text;

        private Vector3 _defaultScale;

        public void Init(BlockValue value, BlockSeed seed)
        {
            _defaultScale = transform.localScale;
            spriteRenderer.sprite = colorSettings.GetBlockSprite(seed);
            text.text = value.Symbol();
        }

        public void ShakeScale()
        {
            transform.DOShakeScale(animationSettings.BlockShakeDuration, animationSettings.BlockShakeStrength)
                .OnComplete(() => transform.localScale = _defaultScale);
        }
    }
}