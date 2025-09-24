using UnityEngine;
using Whatwapp.Core.Audio;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class PlayBlockState : BaseState
    {
        private Board _board;
        private NextBlockController _nextBlockController;
        private SFXManager _sfxManager;
        private bool _blockPlayed;
        
        
        public PlayBlockState(GameController gameController, Board board, NextBlockController nextBlockController,
            SFXManager sfxManager) : base(gameController)
        {
            _board = board;
            _nextBlockController = nextBlockController;
            _sfxManager = sfxManager;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            PlayBlockCompleted = false;
            _blockPlayed = false;
            CheckGameOver();
        }

        public override void Update()
        { 
            if (GameOver || _blockPlayed) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (_gameController.IsPaused) return;
            
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var coord = _board.GetCellCoordinates(worldPosition);
            if ((coord.x>=0 && coord.x<_board.Width) && (coord.y>=0 && coord.y<_board.Height))
            {
                PlayBlock(coord.x);
            }
        }

        private void PlayBlock(int column)
        {
            _sfxManager.PlayOneShot(Consts.SFX_PlayBlock);
            // Check if the cell is empty
            var cell = _board.GetCell(new Vector2Int(column, 0));
            if (!cell.IsEmpty) return;
            
            var columnCells = _board.GetCellInColumn(column);
            foreach (var c in columnCells)
            {
                c.OnClick();
            }
            
            var block = _nextBlockController.PopBlock();
            cell.Block = block;
            _blockPlayed = true;
            PlayBlockCompleted = true;
        }

        private void CheckGameOver()
        {
            GameOver = false;
            var emptyCells = _board.GetEmptyCells();
            if (emptyCells.Count == 0)
            {
                GameOver = true;
            }
        }

        public bool PlayBlockCompleted { get; private set; }
        public bool GameOver { get; private set; }
    }
}