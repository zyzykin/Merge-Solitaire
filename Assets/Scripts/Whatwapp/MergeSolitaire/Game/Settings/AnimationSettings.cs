using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Animations", fileName = "AnimationSettings")]
    public class AnimationSettings : ScriptableObject
    {
        
        [Header("Block Spawn")]
        [SerializeField] private float _blockSpawnDuration = 0.25f;
        
        [Header("Block Move")]
        [SerializeField] private float _blockMoveDuration = 0.01f;
        [SerializeField] private float _blockMoveDelay = 0.0025f;
        
        [Header("Block Merge")]
        [SerializeField] private float _blockMergeDuration = 0.1f;
        [SerializeField] private float _blockMergeDelay = 0.2f;
        [SerializeField] private float _tremorDuration = 0.25f;
        [SerializeField] private float _tremorStrength = 0.1f;
        
        [Header("Block Shake")]
        [SerializeField] private float _blockShakeDuration = 0.25f;
        [SerializeField] private float _blockShakeStrength = 0.1f;
        
        [Header("Block To Foundation")]
        [SerializeField] private float _attachDuration = 0.35f;
        
        [Header("Cell")]
        [SerializeField] private float _highlightDuration = 0.05f;
        [SerializeField] private float _highlightDelay = 0.03f;
        
        [Header("Column")]
        [SerializeField]private float _cellShakeScaleDuration = 0.2f;
        [SerializeField] private float _cellShakeScaleStrength = 0.1f;
        
        public float BlockMoveDuration => _blockMoveDuration;
        public float BlockMoveDelay => _blockMoveDelay;
        public float MergeDuration => _blockMergeDuration;
        public float TremorDuration => _tremorDuration;
        public float TremorStrength => _tremorStrength;
        public float SpawnDuration => _blockSpawnDuration;
        public float AttachDuration => _attachDuration;
        public float HighlightDelay => _highlightDelay;
        public float HighlightDuration => _highlightDuration;
        public float BlockShakeDuration => _blockShakeDuration;
        public float BlockShakeStrength => _blockShakeStrength;
        public float CellShakeScaleDuration => _cellShakeScaleDuration;
        public float CellShakeScaleStrength => _cellShakeScaleStrength;
    }
}