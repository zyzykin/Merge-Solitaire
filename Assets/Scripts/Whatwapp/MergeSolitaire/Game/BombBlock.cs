using System;
using System.Collections.Generic;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlock : Block
    {
        [SerializeField] private int bombExplodeRadius = 1;

        public int BombExplodeRadius => bombExplodeRadius;

        public void Explode(Board board, Cell currentCell, IBlockAnimationPresenter animationPresenter,
            ISFXPresenter sfxPresenter, HashSet<Cell> visitedCells = null, Action onComplete = null,
            Action<Block, Cell> onBlockAnimationComplete = null)
        {
            sfxPresenter.PlayOneShot(Consts.SFX_Explosion);
            animationPresenter.AnimateExplosion(this, board, currentCell, visitedCells, onComplete,
                onBlockAnimationComplete);
        }
    }
}