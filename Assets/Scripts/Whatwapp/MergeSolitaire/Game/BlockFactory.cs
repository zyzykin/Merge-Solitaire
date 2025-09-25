using UnityEngine;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockFactory : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private Block blockPrefab;

        public Block Create(BlockValue value, BlockSeed seed)
        {
            var block = Instantiate(blockPrefab, transform);
            block.Init(value, seed);
            return block;
        }

        public Block CreateStartingBlock()
        {
            var value = EnumUtils.GetRandom(BlockValue.Ace, BlockValue.King);
            var seed = EnumUtils.GetRandom<BlockSeed>();
            return Create(value, seed);
        }
    }
}