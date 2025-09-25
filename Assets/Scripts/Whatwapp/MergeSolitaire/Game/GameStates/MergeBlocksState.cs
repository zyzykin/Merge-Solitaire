using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whatwapp.Core.Extensions;
using Whatwapp.Core.Utils;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksState : BaseState
    {
        private readonly Board _board;
        private readonly BlockFactory _blockFactory;
        private readonly FoundationsController _foundationsController;
        private readonly ISFXPresenter _sfxPresenter;
        private readonly IBlockAnimationPresenter _blockAnimationPresenter;

        public bool MergeCompleted { get; private set; }
        public int MergeCount { get; private set; }

        private readonly Vector2Int[] _directions =
        {
            new(1, 0),
            new(-1, 0),
            new(0, 1),
            new(0, -1),
        };

        public MergeBlocksState(GameController gameController, Board board, BlockFactory blockFactory,
            FoundationsController foundationsController, ISFXPresenter sfxPresenter,
            IBlockAnimationPresenter blockAnimationPresenter) : base(gameController)
        {
            _board = board;
            _blockFactory = blockFactory;
            _foundationsController = foundationsController;
            _sfxPresenter = sfxPresenter;
            _blockAnimationPresenter = blockAnimationPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            MergeCompleted = false;
            MergeCount = 0;
            MergeBlocks();
        }

        private void MergeBlocks()
        {
            MergeCompleted = false;
            var mergeableGroups = GetAllMergableCells();
            MergeCount = mergeableGroups.Count;
            if (MergeCount == 0)
            {
                MergeCompleted = true;
                return;
            }

            MergeGroups(mergeableGroups);
        }

        private void MergeGroups(List<List<Cell>> mergeableGroups)
        {
            var mergeGroups = new List<MergeGroup>();
            foreach (var group in mergeableGroups)
            {
                var firstCell = group[0];
                var oldValue = firstCell.Block.Value;
                var nextValue = oldValue.Next(true);
                var randomSeed = EnumUtils.GetRandom<BlockSeed>();
                var seedHash = new HashSet<BlockSeed>();
                foreach (var cell in group)
                {
                    seedHash.Add(cell.Block.Seed);
                }

                mergeGroups.Add(new MergeGroup
                {
                    Cells = group,
                    FirstCell = firstCell,
                    OldValue = oldValue,
                    NextValue = nextValue,
                    RandomSeed = randomSeed,
                    SeedHash = seedHash
                });
            }

            _blockAnimationPresenter.AnimateMerges(mergeGroups,
                () => { MergeCompleted = true; },
                (mergeGroup, cell, targetCell) =>
                {
                    GameController.Score += MergeCount * mergeGroup.Cells.Count;
                    targetCell.Block = null;
                    cell.Block.Remove();
                },
                mergeGroup =>
                {
                    mergeGroup.FirstCell.Block.Remove();
                    mergeGroup.FirstCell.Block = null;
                },
                mergeGroup =>
                {
                    var newBlock = _blockFactory.Create(mergeGroup.NextValue, mergeGroup.RandomSeed);
                    mergeGroup.FirstCell.Block = newBlock;
                    newBlock.transform.position = mergeGroup.FirstCell.transform.position;
                    newBlock.transform.localScale = Vector3.zero;
                    _blockAnimationPresenter.AnimateAppear(newBlock);
                    foreach (var seed in mergeGroup.SeedHash)
                    {
                        Debug.Log("Seed in group: " + seed);
                        var info = new BlockToFoundationInfo(seed, mergeGroup.OldValue,
                            mergeGroup.FirstCell.transform.position);
                        if (_foundationsController.TryAndAttach(info))
                        {
                            _sfxPresenter.PlayOneShot(Consts.GetFoundationSFX(seed));
                            GameController.Score += Consts.FOUNDATION_POINTS;
                        }
                    }
                });
        }

        private List<List<Cell>> GetAllMergableCells()
        {
            var mergedGroups = new List<List<Cell>>();
            var visited = new bool[_board.Width, _board.Height];
            for (var x = 0; x < _board.Width; x++)
            {
                for (var y = _board.Height - 1; y >= 0; y--)
                {
                    if (visited[x, y]) continue;
                    var cell = _board.GetCell(x, y);
                    if (cell.IsEmpty || cell.Block.Value == BlockValue.Bomb) continue;
                    var group = GetMergeableCells(cell, visited);
                    if (group.Count > 1)
                    {
                        mergedGroups.Add(group);
                    }
                }
            }

            return mergedGroups;
        }

        private List<Cell> GetMergeableCells(Cell cell, bool[,] visited)
        {
            var mergeableCells = new List<Cell>();
            var value = cell.Block.Value;
            var queue = new Queue<Cell>();
            queue.Enqueue(cell);
            visited[cell.Coordinates.x, cell.Coordinates.y] = true;

            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();
                mergeableCells.Add(currentCell);
                foreach (var direction in _directions)
                {
                    var nextCell = _board.GetCell(currentCell.Coordinates + direction);
                    if (nextCell == null || nextCell.IsEmpty ||
                        visited[nextCell.Coordinates.x, nextCell.Coordinates.y] ||
                        nextCell.Block.Value != value ||
                        nextCell.Block.Value == BlockValue.Bomb) continue;
                    visited[nextCell.Coordinates.x, nextCell.Coordinates.y] = true;
                    queue.Enqueue(nextCell);
                }
            }

            return mergeableCells;
        }
    }

    public class MergeGroup
    {
        public List<Cell> Cells { get; set; }
        public Cell FirstCell { get; set; }
        public BlockValue OldValue { get; set; }
        public BlockValue NextValue { get; set; }
        public BlockSeed RandomSeed { get; set; }
        public HashSet<BlockSeed> SeedHash { get; set; }
    }
}