using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Extensions;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationPileController : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private AnimationSettings animationSettings;

        [Header("References")] [SerializeField]
        private Transform attachPoint;

        [SerializeField] private FoundationPileVisual visual;

        private BlockSeed _seed;
        private Block _block;

        public BlockSeed Seed => _seed;
        public bool IsCompleted => _block != null && _block.Value == BlockValue.King;

        public BlockValue NextValue => _block == null ? BlockValue.Ace : _block.Value.Next();

        public void Init(BlockSeed seed, Sprite foundationSprite)
        {
            _seed = seed;
            visual.SetBackgroundSprite(foundationSprite);
        }

        private bool CanAttach(Block block)
        {
            return CanAttach(block.Seed, block.Value);
        }

        public bool CanAttach(BlockSeed seed, BlockValue value)
        {
            return _seed == seed && (_block == null && value == BlockValue.Ace) ||
                   (_block != null && _block.Value != BlockValue.King && _block.Value.Next() == value);
        }

        public void AttachBlock(Block block)
        {
            if (!CanAttach(block)) return;
            var sequence = DOTween.Sequence();
            if (_block != null)
            {
                sequence.Append(_block.transform.DOScale(Vector3.zero, animationSettings.AttachDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => { Destroy(_block.gameObject); }));
            }

            sequence.Join(block.transform.DOMove(attachPoint.position, animationSettings.AttachDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    block.transform.SetParent(attachPoint);
                    block.transform.localPosition = Vector3.zero;
                    _block = block;
                }));
            sequence.Play();
        }
    }
}