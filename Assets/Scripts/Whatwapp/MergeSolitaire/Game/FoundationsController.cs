using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationsController : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private ColorSettings colorSettings;

        [Header("References")] [SerializeField]
        private BlockFactory blockFactory;

        [SerializeField] private FoundationPileController[] foundationPiles;

        public bool AllFoundationsCompleted => foundationPiles.All(x => x.IsCompleted);

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            var validSeeds = new[] { BlockSeed.Clubs, BlockSeed.Diamonds, BlockSeed.Hearts, BlockSeed.Spades };
            if ((foundationPiles == null || foundationPiles.Length == 0) ||
                (foundationPiles.Length != validSeeds.Length))
            {
                Debug.LogError(
                    $"Foundation piles are not set correctly. Expected {validSeeds.Length} piles, got {foundationPiles?.Length ?? 0}.");
                return;
            }

            for (var i = 0; i < foundationPiles.Length; i++)
            {
                var seed = validSeeds[i];
                foundationPiles[i].Init(seed, colorSettings.GetFoundationSprite(seed));
            }
        }

        public bool TryAndAttach(BlockToFoundationInfo info)
        {
            if (info.Seed == BlockSeed.Bomb)
            {
                Debug.LogWarning("Cannot attach Bomb to foundation");
                return false;
            }

            var foundationPile = foundationPiles.FirstOrDefault(x => x.Seed == info.Seed);
            if ((foundationPile == null) || (!foundationPile.CanAttach(info.Seed, info.Value))) return false;

            var block = blockFactory.Create(info.Value, info.Seed);
            block.transform.position = info.Position;
            foundationPile.AttachBlock(block);
            return true;
        }

        public List<(BlockSeed, BlockValue)> GetNextBlocks()
        {
            var nextBlocks = new List<(BlockSeed, BlockValue)>();
            foreach (var foundationPile in foundationPiles)
            {
                if (foundationPile.IsCompleted) continue;
                var nextValue = foundationPile.NextValue;
                nextBlocks.Add((foundationPile.Seed, nextValue));
            }

            return nextBlocks;
        }
    }

    public class BlockToFoundationInfo
    {
        private BlockSeed _seed;
        private BlockValue _value;
        private Vector3 _position;

        public BlockSeed Seed => _seed;
        public BlockValue Value => _value;
        public Vector3 Position => _position;

        public BlockToFoundationInfo(BlockSeed seed, BlockValue value, Vector3 position)
        {
            _seed = seed;
            _value = value;
            _position = position;
        }
    }
}