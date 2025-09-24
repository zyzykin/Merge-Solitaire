using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationsController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ColorSettings _colorSettings;
        
        [Header("References")]
        [SerializeField] private BlockFactory _blockFactory;
        [SerializeField] private FoundationPileController[] _foundationPiles;
        public bool AllFoundationsCompleted => _foundationPiles.All(x => x.IsCompleted);


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if ((_foundationPiles == null || _foundationPiles.Length == 0) ||
                (_foundationPiles.Length != Enum.GetValues(typeof(BlockSeed)).Length))
            {
                Debug.LogError("Foundation piles are not set correctly");
                return;
            }
            
            for (var i = 0; i < _foundationPiles.Length; i++)
            {
                var seed = (BlockSeed) i;
                _foundationPiles[i].Init(seed,  _colorSettings.GetFoundationSprite(seed));
            }           
        }

        public bool TryAndAttach(BlockToFoundationInfo info)
        {
            var foundationPile = _foundationPiles.FirstOrDefault(x => x.Seed == info.Seed);
            if ((foundationPile == null) || (!foundationPile.CanAttach(info.Seed, info.Value))) return false;
            
            var block = _blockFactory.Create(info.Value, info.Seed);
            block.transform.position = info.Position;
            foundationPile.AttachBlock(block);
            return true;
        }
        
        public List<(BlockSeed, BlockValue)> GetNextBlocks()
        {
            var nextBlocks = new List<(BlockSeed, BlockValue)>();
            foreach (var foundationPile in _foundationPiles)
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