using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using Whatwapp.MergeSolitaire.Game.GameStates;
using Whatwapp.MergeSolitaire.Game.Settings;

namespace Whatwapp.MergeSolitaire.Game.Presentation
{
    public class BlockAnimationPresenter : MonoBehaviour, IBlockAnimationPresenter
    {
        [Header("Settings")] [SerializeField] private AnimationSettings animationSettings;
        private ISFXPresenter _sfxPresenter;

        public void Initialize(ISFXPresenter sfxPresenter)
        {
            _sfxPresenter = sfxPresenter;
        }

        public void AnimateMerges(List<MergeGroup> mergeGroups, Action onAllComplete,
            Action<MergeGroup, Cell, Cell> onSubMergeComplete, Action<MergeGroup> onFirstRemoved,
            Action<MergeGroup> onGroupComplete)
        {
            var sequence = DOTween.Sequence();

            foreach (var mergeGroup in mergeGroups)
            {
                var group = mergeGroup.Cells;
                var firstCell = mergeGroup.FirstCell;

                var groupSequence = DOTween.Sequence();

                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    tremorSequence.Join(cell.Block.transform.DOShakeScale(
                        animationSettings.TremorDuration,
                        animationSettings.TremorStrength));
                }

                groupSequence.Append(tremorSequence);

                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;

                    var blockSequence = DOTween.Sequence();
                    blockSequence.Append(block.transform.DOMove(targetPos, animationSettings.MergeDuration));
                    blockSequence.Join(block.transform.DOScale(Vector3.zero, animationSettings.MergeDuration)
                        .OnStart(() => { _sfxPresenter.PlayOneShot(Consts.SFX_PlayBlock); }));

                    blockSequence.OnComplete(() =>
                    {
                        _sfxPresenter.PlayOneShot(Consts.SFX_MergeBlocks);
                        onSubMergeComplete?.Invoke(mergeGroup, cell, targetCell);
                    });

                    groupSequence.Append(blockSequence);
                    groupSequence.AppendInterval(animationSettings.BlockMergeDelay);
                }

                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstCell.Block.transform.DOScale(0, animationSettings.MergeDuration)
                    .OnComplete(() => { onFirstRemoved?.Invoke(mergeGroup); }));

                groupSequence.Append(finalSequence);

                groupSequence.OnComplete(() => { onGroupComplete?.Invoke(mergeGroup); });

                sequence.Append(groupSequence);
            }

            sequence.OnComplete(() => onAllComplete?.Invoke());
            sequence.Play();
        }

        public void AnimateMoves(List<(Block block, Vector3 targetPos)> movePairs, Action onComplete)
        {
            var sequence = DOTween.Sequence();

            foreach (var (block, targetPos) in movePairs)
            {
                sequence.Join(
                    DOTween.Sequence()
                        .AppendInterval(animationSettings.BlockMoveDelay)
                        .Append(block.transform.DOMove(targetPos, animationSettings.BlockMoveDuration))
                        .OnComplete(() => { block.Visual.ShakeScale(); }));
            }

            sequence.OnComplete(() => onComplete?.Invoke());
            sequence.Play();
        }

        public void AnimateColumnClick(List<Cell> columnCells)
        {
            foreach (var c in columnCells)
            {
                c.transform.DOShakeScale(animationSettings.CellShakeScaleDuration,
                    animationSettings.CellShakeScaleStrength);
            }
        }

        public void AnimateAppear(Block block)
        {
            block.transform.DOScale(Vector3.one, animationSettings.MergeDuration).SetEase(Ease.OutBack);
        }

        public void AnimateExplosion(Block block, Board board = null, Cell currentCell = null,
            HashSet<Cell> visitedCells = null, Action onComplete = null, Action<Block, Cell> onBlockAnimationComplete = null)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(block.transform.DOScale(Vector3.one * animationSettings.BombExplosionScale, animationSettings.ExplosionDuration)
                .SetEase(Ease.OutQuad));
            sequence.Append(block.transform.DOScale(Vector3.zero, animationSettings.ExplosionDuration / 2)
                .SetEase(Ease.InQuad));

            if (board != null && currentCell != null && block.Value == BlockValue.Bomb)
            {
                if (visitedCells == null)
                {
                    visitedCells = new HashSet<Cell>();
                }

                if (visitedCells.Contains(currentCell))
                {
                    sequence.OnComplete(() => onComplete?.Invoke());
                    sequence.Play();
                    return;
                }

                visitedCells.Add(currentCell);

                var bombBlock = block as BombBlock;
                var neighbors = board.GetNeighbors(currentCell, bombBlock.BombExplodeRadius);
                var sortedNeighbors = new List<(Cell cell, float distance)>();
                foreach (var neighborCell in neighbors)
                {
                    if (neighborCell != null && !neighborCell.IsEmpty)
                    {
                        float distance = Vector2Int.Distance(currentCell.Coordinates, neighborCell.Coordinates);
                        sortedNeighbors.Add((neighborCell, distance));
                    }
                }

                sortedNeighbors.Sort((a, b) => a.distance.CompareTo(b.distance));

                foreach (var (neighborCell, distance) in sortedNeighbors)
                {
                    var neighborBlock = neighborCell.Block;
                    if (neighborBlock != null)
                    {
                        sequence.AppendInterval(distance * animationSettings.ExplosionDelayPerDistance);
                        if (neighborBlock.Value == BlockValue.Bomb)
                        {
                            sequence.AppendCallback(() =>
                            {
                                _sfxPresenter.PlayOneShot(Consts.SFX_Explosion);
                                AnimateExplosion(neighborBlock, board, neighborCell, visitedCells, null, onBlockAnimationComplete);
                            });
                        }
                        else
                        {
                            sequence.AppendCallback(() =>
                            {
                                AnimateDestroy(neighborBlock, () => onBlockAnimationComplete?.Invoke(neighborBlock, neighborCell));
                            });
                        }
                    }
                }

                sequence.AppendCallback(() => onBlockAnimationComplete?.Invoke(block, currentCell));
            }

            sequence.OnComplete(() => onComplete?.Invoke());
            sequence.Play();
        }

        private void AnimateDestroy(Block block, Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(block.transform.DOScale(Vector3.one * animationSettings.DestroyScale, animationSettings.ExplosionDuration / 2)
                .SetEase(Ease.OutQuad));
            sequence.Append(block.transform.DOScale(Vector3.zero, animationSettings.ExplosionDuration / 2)
                .SetEase(Ease.InQuad));
            sequence.OnComplete(() => onComplete?.Invoke());
            sequence.Play();
        }
    }
}