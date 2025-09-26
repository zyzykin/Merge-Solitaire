using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private BlockVisual visual;
        [SerializeField] private ParticleSystem destroyParticlePrefab;

        private BlockValue _value;
        private BlockSeed _seed;

        public BlockValue Value => _value;
        public BlockSeed Seed => _seed;
        public BlockVisual Visual => visual;

        public void Init(BlockValue value, BlockSeed seed)
        {
            _value = value;
            _seed = seed;
            visual.Init(value, seed);
        }

        public void Remove()
        {
            if (destroyParticlePrefab != null)
            {
                var particleInstance = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
                
                if (!particleInstance.playOnAwake)
                {
                    particleInstance.Play();
                }
            }

            Destroy(gameObject);
        }
    }
}