using System;
using System.Collections.Generic;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.GameStates;

namespace Whatwapp.MergeSolitaire.Game.Presentation
{
    public interface IBlockAnimationPresenter
    {
        void Initialize(ISFXPresenter sfxPresenter);

        void AnimateMerges(List<MergeGroup> mergeGroups, Action onAllComplete,
            Action<MergeGroup, Cell, Cell> onSubMergeComplete, Action<MergeGroup> onFirstRemoved,
            Action<MergeGroup> onGroupComplete);

        void AnimateMoves(List<(Block block, Vector3 targetPos)> movePairs, Action onComplete);
        void AnimateColumnClick(List<Cell> columnCells);
        void AnimateAppear(Block block);
        void AnimateExplosion(Block block, Board board = null, Cell currentCell = null, 
            HashSet<Cell> visitedCells = null, Action onComplete = null, 
            Action<Block, Cell> onBlockAnimationComplete = null);
    }
}