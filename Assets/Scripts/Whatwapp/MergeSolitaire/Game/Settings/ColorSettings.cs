using System;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.Settings
{
    [Serializable]
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Colors", fileName = "ColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        [Header("Block Colors")]
        [SerializeField] private Color[] blockColors;
        
        [Header("Block Sprites")]
        [SerializeField] private Sprite[] blockSprites;
        
        [Header("Foundation Sprites")]
        [SerializeField] private Sprite[] foundationSprites;
        
        public Color[] BlockColors => blockColors;
        public Color GetBlockColor(BlockSeed seed) => blockColors[(int) seed];
        public Sprite GetBlockSprite(BlockSeed seed) => blockSprites[(int) seed];
        
        public Sprite GetFoundationSprite(BlockSeed seed) => foundationSprites[(int) seed];
        
        [Header("Cell Colors")]
        [SerializeField] private Color cellColor;
        [SerializeField] private Color cellHighlightColor;

        public Color GetCellColor(Vector2Int coordinates)
        {
            return cellColor;
        }
        
        public Color GetCellHighlightColor(Vector2Int coordinates)
        {
            return cellHighlightColor;
        }
    }
}