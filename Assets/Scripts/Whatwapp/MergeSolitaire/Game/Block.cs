using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private BlockVisual _visual;
        
        
        private BlockValue _value;
        private BlockSeed _seed;
        
        public BlockValue Value => _value;
        public BlockSeed Seed => _seed;
        public BlockVisual Visual => _visual;

        public void Init(BlockValue value, BlockSeed seed)
        {
            _value = value;
            _seed = seed;
            _visual.Init(value, seed);
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}