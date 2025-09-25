using System.Collections.Generic;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : Block
    {
        [SerializeField] private int bombExplodeRadius = 1;

        public void Explode(Board board, Cell currentCell, IBlockAnimationPresenter animationPresenter,
            ISFXPresenter sfxPresenter, HashSet<Cell> visitedCells = null)
        {
            if (visitedCells == null)
            {
                visitedCells = new HashSet<Cell>();
            }

            if (visitedCells.Contains(currentCell)) return;
            visitedCells.Add(currentCell);
            animationPresenter.AnimateExplosion(this);
            sfxPresenter.PlayOneShot(Consts.SFX_Explosion);

            var neighbors = board.GetNeighbors(currentCell, bombExplodeRadius);
            foreach (var neighborCell in neighbors)
            {
                if (neighborCell != null && !neighborCell.IsEmpty)
                {
                    var neighborBlock = neighborCell.Block;
                    if (neighborBlock.Value == BlockValue.Bomb)
                    {
                        var bombBlock = neighborBlock as BombBlock;
                        bombBlock.Explode(board, neighborCell, animationPresenter, sfxPresenter, visitedCells);
                    }
                    else
                    {
                        neighborBlock.Remove();
                        neighborCell.Block = null;
                    }
                }
            }

            Remove();
            currentCell.Block = null;
        }
    }
}