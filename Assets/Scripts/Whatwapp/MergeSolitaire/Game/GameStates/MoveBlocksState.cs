using System.Collections.Generic;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksState : BaseState
    {
        private bool _isMovingBlocks;
        private readonly Board _board;
        private readonly IBlockAnimationPresenter _blockAnimationPresenter;
        private readonly ISFXPresenter _sfxPresenter;

        private readonly List<Cell> _movingCells;
        private int _startingRow;
        private int _activeExplosions;

        public MoveBlocksState(GameController gameController, Board board,
            IBlockAnimationPresenter blockAnimationPresenter, ISFXPresenter sfxPresenter) : base(gameController)
        {
            _board = board;
            _movingCells = new List<Cell>();
            _blockAnimationPresenter = blockAnimationPresenter;
            _sfxPresenter = sfxPresenter;
            _activeExplosions = 0;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _movingCells.Clear();
            _isMovingBlocks = false;
            _activeExplosions = 0;
            _startingRow = _board.Height - 2;
        }

        public override void OnExit()
        {
            base.OnExit();

            _isMovingBlocks = false;
            _activeExplosions = 0;
            HasMovableBlocks();
        }

        public override void Update()
        {
            if (_isMovingBlocks || _activeExplosions > 0) return;
            _isMovingBlocks = true;
            if (FindMovableCells())
            {
                MoveBlocks();
            }
            else
            {
                _isMovingBlocks = false;
                CheckAndExplodeBombs();
            }
        }

        private bool FindMovableCells()
        {
            _movingCells.Clear();
            for (var i = 0; i < _board.Width; i++)
            {
                for (var j = _startingRow; j >= 0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty)
                        continue;

                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;

                    _movingCells.Add(cell);
                }
            }

            return _movingCells.Count > 0;
        }

        private void MoveBlocks()
        {
            var movePairs = new List<(Block block, Vector3 targetPos)>();
            foreach (var cell in _movingCells)
            {
                var block = cell.Block;
                var targetCell = _board.GetCell(cell.Coordinates.x, cell.Coordinates.y + 1);
                targetCell.Block = block;
                cell.Block = null;
                movePairs.Add((block, targetCell.Position));
            }

            _blockAnimationPresenter.AnimateMoves(movePairs, () =>
            {
                _isMovingBlocks = false;
                CheckAndExplodeBombs();
            });
        }

        private void CheckAndExplodeBombs()
        {
            var visitedCells = new HashSet<Cell>();
            var bombsToExplode = new List<(BombBlock bomb, Cell cell)>();

            foreach (var cell in _board.Cells)
            {
                if (cell.IsEmpty || cell.Block.Value != BlockValue.Bomb) continue;

                if (cell.Coordinates.y == 0)
                {
                    var bombBlock = cell.Block as BombBlock;
                    if (bombBlock != null && !visitedCells.Contains(cell))
                    {
                        bombsToExplode.Add((bombBlock, cell));
                    }
                }
            }

            if (bombsToExplode.Count == 0) return;

            _activeExplosions = bombsToExplode.Count;

            foreach (var (bombBlock, cell) in bombsToExplode)
            {
                bombBlock.Explode(_board, cell, _blockAnimationPresenter, _sfxPresenter, visitedCells,
                    () => { _activeExplosions--; },
                    (block, targetCell) =>
                    {
                        block.Remove();
                        targetCell.Block = null;
                    });
            }
        }

        public bool CanMoveBlocks()
        {
            return _isMovingBlocks || _activeExplosions > 0 || HasMovableBlocks();
        }

        private bool HasMovableBlocks()
        {
            var startingRow = _board.Height - 2;
            for (var i = 0; i < _board.Width; i++)
            {
                for (var j = startingRow; j >= 0; j--)
                {
                    var cell = _board.GetCell(i, j);
                    if (cell == null || cell.IsEmpty) continue;

                    var upperCell = _board.GetCell(i, j + 1);
                    if (upperCell == null || !upperCell.IsEmpty) continue;

                    return true;
                }
            }

            return false;
        }
    }
}