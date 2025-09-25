using System.Collections.Generic;
using UnityEngine;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MoveBlocksState : BaseState
    {
        private bool _isMovingBlocks;
        private bool _canMoveBlocks;
        private readonly Board _board;
        private readonly IBlockAnimationPresenter _blockAnimationPresenter;

        private readonly List<Cell> _movingCells;
        private int _startingRow;

        public MoveBlocksState(GameController gameController, Board board,
            IBlockAnimationPresenter blockAnimationPresenter) : base(gameController)
        {
            _board = board;
            _movingCells = new List<Cell>();
            _blockAnimationPresenter = blockAnimationPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _movingCells.Clear();
            _isMovingBlocks = false;
            _startingRow = _board.Height - 2;
        }

        public override void OnExit()
        {
            base.OnExit();

            _isMovingBlocks = false;
            HasMovableBlocks();
        }

        public override void Update()
        {
            if (_isMovingBlocks) return;
            _isMovingBlocks = true;
            if (FindMovableCells())
            {
                MoveBlocks();
            }
            else
            {
                _isMovingBlocks = false;
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

            _blockAnimationPresenter.AnimateMoves(movePairs, () => { _isMovingBlocks = false; });
        }

        public bool CanMoveBlocks()
        {
            return _isMovingBlocks || HasMovableBlocks();
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