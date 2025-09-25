using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Utils;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game
{
    public class NextBlockController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BlockFactory blockFactory;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Board board;
        [SerializeField] private FoundationsController foundationsController;

        [Header("Settings")] [SerializeField] private AnimationSettings animationSettings;
        [SerializeField] [Range(0f, 1f)] private float probabilityOfGoodBlock = 0.5f;
        [SerializeField] [Range(0f, 1f)] private float probabilityToSpawnAttachableBlock = 0.1f;

        public bool IsReady => _nextBlock != null && _isReady;
        public bool HasBlock => _nextBlock != null;

        private Block _nextBlock;
        private bool _isReady;

        public void ExtractNextBlock()
        {
            if (_nextBlock != null) return;
            var seed = EnumUtils.GetRandom<BlockSeed>();
            var value = EnumUtils.GetRandom(BlockValue.Ace, BlockValue.King);

            if (Random.value < probabilityOfGoodBlock)
            {
                if (Random.value < probabilityToSpawnAttachableBlock)
                {
                    value = ExtractAttachableBlock(value);
                }
                else
                {
                    var nextBlocks = foundationsController.GetNextBlocks();
                    if (nextBlocks.Count > 0)
                    {
                        var item = nextBlocks[Random.Range(0, nextBlocks.Count)];
                        value = item.Item2;
                        seed = item.Item1;
                    }
                }
            }

            _nextBlock = blockFactory.Create(value, seed);
            _nextBlock.transform.SetParent(spawnPoint);
            _nextBlock.transform.localScale = Vector3.zero;
            _isReady = false;
            AnimateSpawn();
        }

        private BlockValue ExtractAttachableBlock(BlockValue value)
        {
            var attachableBlocks = board.GetAttachableBlocks();
            if (attachableBlocks.Count <= 0) return value;
            var block = attachableBlocks[Random.Range(0, attachableBlocks.Count)];
            value = block.Value;
            return value;
        }

        public Block PopBlock()
        {
            var block = _nextBlock;
            _nextBlock = null;
            return block;
        }

        private void AnimateSpawn()
        {
            _nextBlock.transform.localPosition = Vector3.zero;
            _nextBlock.transform.DOScale(Vector3.one, animationSettings.SpawnDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() => { _isReady = true; });
        }
    }
}