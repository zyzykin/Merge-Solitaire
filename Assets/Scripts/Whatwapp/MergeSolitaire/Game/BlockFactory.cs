using UnityEngine;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BlockFactory : MonoBehaviour
    {
        [SerializeField] private Block blockPrefab;
        [Header("Bomb")] [SerializeField] private BombBlock bombPrefab;
        [SerializeField, Range(0f, 1f)] private float bombSpawnChance = 0.1f;

        public float BombSpawnChance => bombSpawnChance;

        public Block Create(BlockValue value, BlockSeed seed)
        {
            Block block;
            if (value == BlockValue.Bomb)
            {
                block = Instantiate(bombPrefab, transform);
            }
            else
            {
                block = Instantiate(blockPrefab, transform);
            }

            block.Init(value, seed);
            return block;
        }

        public Block CreateStartingBlock(bool isFirst)
        {
            if (isFirst)
            {
                var value = EnumUtils.GetRandom(BlockValue.Ace, BlockValue.King);
                var seed = EnumUtils.GetRandom(BlockSeed.Clubs, BlockSeed.Spades);
                return Create(value, seed);
            }
            else
            {
                var isBomb = Random.value < BombSpawnChance;
                var value = isBomb ? BlockValue.Bomb : EnumUtils.GetRandom(BlockValue.Ace, BlockValue.King);
                var seed = isBomb ? BlockSeed.Bomb : EnumUtils.GetRandom(BlockSeed.Clubs, BlockSeed.Spades);
                return Create(value, seed);
            }
        }
    }
}