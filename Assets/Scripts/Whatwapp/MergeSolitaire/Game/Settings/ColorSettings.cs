using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Colors", fileName = "ColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        [Header("Block Colors")]
        [SerializeField] private Color[] _blockColors;
        
        [Header("Block Sprites")]
        [SerializeField] private Sprite[] _blockSprites;
        
        [Header("Foundation Sprites")]
        [SerializeField] private Sprite[] _foundationSprites;
        
        public Color[] BlockColors => _blockColors;
        public Color GetBlockColor(BlockSeed seed) => _blockColors[(int) seed];
        public Sprite GetBlockSprite(BlockSeed seed) => _blockSprites[(int) seed];
        
        public Sprite GetFoundationSprite(BlockSeed seed) => _foundationSprites[(int) seed];
        
        [Header("Cell Colors")]
        [SerializeField] private Color _cellColor;
        [SerializeField] private Color _cellHighlightColor;

        public Color GetCellColor(Vector2Int coordinates)
        {
            return _cellColor;
        }
        
        public Color GetCellHighlightColor(Vector2Int coordinates)
        {
            return _cellHighlightColor;
        }
    }
}