using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Extensions;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationPileController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AnimationSettings _animationSettings;
        
        [Header("References")]
        [SerializeField]  private Transform _attachPoint;
        [SerializeField]  private FoundationPileVisual _visual;
        
        private BlockSeed _seed;
        private Block _block;
        
        public BlockSeed Seed => _seed;
        public bool IsCompleted => _block != null && _block.Value == BlockValue.King;
        
        public BlockValue NextValue => _block == null ? BlockValue.Ace : _block.Value.Next();

        public void Init(BlockSeed seed,Sprite foundationSprite)
        {
            _seed = seed;
            _visual.SetBackgroundSprite(foundationSprite);
        }
   
        
        public bool CanAttach(Block block)
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
                sequence.Append(_block.transform.DOScale(Vector3.zero, _animationSettings.AttachDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        Destroy(_block.gameObject);
                    }));
            }
            sequence.Join(block.transform.DOMove(_attachPoint.position, _animationSettings.AttachDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    block.transform.SetParent(_attachPoint);
                    block.transform.localPosition = Vector3.zero;
                    _block = block;
                }));
            sequence.Play();
        }
    }
}