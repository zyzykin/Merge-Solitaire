using System;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.Settings
{
    [Serializable]
    [CreateAssetMenu(menuName = "MergeSolitaire/Settings/Animations", fileName = "AnimationSettings")]
    public class AnimationSettings : ScriptableObject
    {
        [Header("Block Spawn")] [SerializeField]
        private float blockSpawnDuration = 0.25f;

        [Header("Block Move")] [SerializeField]
        private float blockMoveDuration = 0.01f;

        [SerializeField] private float blockMoveDelay = 0.0025f;

        [Header("Block Merge")] [SerializeField]
        private float blockMergeDuration = 0.1f;

        [SerializeField] private float blockMergeDelay = 0.2f;
        [SerializeField] private float tremorDuration = 0.25f;
        [SerializeField] private float tremorStrength = 0.1f;

        [Header("Block Shake")] [SerializeField]
        private float blockShakeDuration = 0.25f;

        [SerializeField] private float blockShakeStrength = 0.1f;

        [Header("Block Explosion")] [SerializeField]
        private float explosionDuration = 0.3f;

        [Header("Block To Foundation")] [SerializeField]
        private float attachDuration = 0.35f;

        [Header("Cell")] [SerializeField] private float highlightDuration = 0.05f;
        [SerializeField] private float highlightDelay = 0.03f;

        [Header("Column")] [SerializeField] private float cellShakeScaleDuration = 0.2f;
        [SerializeField] private float cellShakeScaleStrength = 0.1f;

        public float BlockMoveDuration => blockMoveDuration;
        public float BlockMoveDelay => blockMoveDelay;
        public float MergeDuration => blockMergeDuration;
        public float BlockMergeDelay => blockMergeDelay;
        public float TremorDuration => tremorDuration;
        public float TremorStrength => tremorStrength;
        public float SpawnDuration => blockSpawnDuration;
        public float AttachDuration => attachDuration;
        public float HighlightDelay => highlightDelay;
        public float HighlightDuration => highlightDuration;
        public float BlockShakeDuration => blockShakeDuration;
        public float BlockShakeStrength => blockShakeStrength;
        public float CellShakeScaleDuration => cellShakeScaleDuration;
        public float CellShakeScaleStrength => cellShakeScaleStrength;
        public float ExplosionDuration => explosionDuration;
    }
}