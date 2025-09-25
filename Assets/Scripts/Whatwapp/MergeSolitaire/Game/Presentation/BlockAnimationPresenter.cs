using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using Whatwapp.MergeSolitaire.Game.GameStates;

namespace Whatwapp.MergeSolitaire.Game.Presentation
{
    public class BlockAnimationPresenter : MonoBehaviour, IBlockAnimationPresenter
    {
        private AnimationSettings _animationSettings;
        private ISFXPresenter _sfxPresenter;

        public void Initialize(AnimationSettings animationSettings, ISFXPresenter sfxPresenter)
        {
            _animationSettings = animationSettings;
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
                    tremorSequence.Join(cell.Block.transform.DOShakeScale(_animationSettings.TremorDuration,
                        _animationSettings.TremorStrength));
                }

                groupSequence.Append(tremorSequence);
                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = DOTween.Sequence();
                    blockSequence.Append(block.transform.DOMove(targetPos, _animationSettings.MergeDuration));
                    blockSequence.Join(block.transform.DOScale(Vector3.zero, _animationSettings.MergeDuration)
                        .OnStart(() => { _sfxPresenter.PlayOneShot(Consts.SFX_PlayBlock); }));
                    blockSequence.SetDelay(_animationSettings.MergeDuration);
                    blockSequence.OnComplete(() =>
                    {
                        _sfxPresenter.PlayOneShot(Consts.SFX_MergeBlocks);
                        onSubMergeComplete?.Invoke(mergeGroup, cell, targetCell);
                    });
                    groupSequence.Join(blockSequence);
                }

                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstCell.Block.transform.DOScale(0, _animationSettings.MergeDuration)
                    .OnComplete(() => { onFirstRemoved?.Invoke(mergeGroup); }));
                groupSequence.Join(finalSequence);
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
                        .AppendInterval(_animationSettings.BlockMoveDelay)
                        .Append(block.transform.DOMove(targetPos, _animationSettings.BlockMoveDuration))
                        .OnComplete(() => { block.Visual.ShakeScale(); }));
            }

            sequence.OnComplete(() => onComplete?.Invoke());
            sequence.Play();
        }

        public void AnimateColumnClick(List<Cell> columnCells)
        {
            foreach (var c in columnCells)
            {
                c.transform.DOShakeScale(_animationSettings.CellShakeScaleDuration,
                    _animationSettings.CellShakeScaleStrength);
            }
        }

        public void AnimateAppear(Block block)
        {
            block.transform.DOScale(Vector3.one, _animationSettings.MergeDuration).SetEase(Ease.OutBack);
        }
    }
}