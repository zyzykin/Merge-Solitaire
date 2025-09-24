using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Audio;
using Whatwapp.Core.Extensions;
using Whatwapp.Core.Utils;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksState : BaseState
    {
        private readonly Board _board;
        private readonly AnimationSettings _animationSettings;
        private readonly BlockFactory _blockFactory;
        private FoundationsController _foundationsController;
        private SFXManager _sfxManager;

        public bool MergeCompleted { get; private set; }
        public int MergeCount { get; private set; }


        private Vector2Int[] _directions = new[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
        };


        public MergeBlocksState(GameController gameController, Board board, BlockFactory blockFactory,
            FoundationsController foundationsController, SFXManager sfxManager, AnimationSettings animationSettings) : base(gameController)
        {
            _board = board;
            _animationSettings = animationSettings;
            _blockFactory = blockFactory;
            _foundationsController = foundationsController;
            _sfxManager = sfxManager;
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
            // Check for all cells that are not empty 
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
            var sequence = DOTween.Sequence();
            foreach (var group in mergeableGroups)
            {
                var seedHash = new HashSet<BlockSeed>();
                var firstCell = group[0];
                var value = firstCell.Block.Value;
                var seed = firstCell.Block.Seed;
                seedHash.Add(seed);
                var groupSequence = DOTween.Sequence();
                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    seedHash.Add(cell.Block.Seed);
                    tremorSequence.Join(cell.Block.transform.DOShakeScale(_animationSettings.TremorDuration,
                        _animationSettings.TremorStrength));
                }
                Debug.Log("Seed hash count: " + seedHash.Count);
                
                groupSequence.Append(tremorSequence);
                for(var i= group.Count-1; i>0; i--)
                {
                    var cell = group[i];
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = DOTween.Sequence();
                    blockSequence.Append(block.transform.DOMove(targetPos, _animationSettings.MergeDuration));
                    blockSequence.Join(block.transform.DOScale(Vector3.zero, _animationSettings.MergeDuration).OnStart(
                        () =>
                        {
                            _sfxManager.PlayOneShot(Consts.SFX_PlayBlock);
                        }));
                    blockSequence.SetDelay(_animationSettings.MergeDuration);
                    blockSequence.OnComplete(() =>
                    {
                        _sfxManager.PlayOneShot(Consts.SFX_MergeBlocks);
                        targetCell.Block = null;
                        _gameController.Score += mergeableGroups.Count * group.Count;
                        block.Remove();
                    });
                    groupSequence.Join(blockSequence);
                }
                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstCell.Block.transform.DOScale(0, _animationSettings.MergeDuration)
                    .OnComplete(() =>
                    {
                        firstCell.Block.Remove();
                        firstCell.Block = null;
                    }));
                groupSequence.Join(finalSequence);
                groupSequence.OnComplete(() =>
                {
                    var nextValue = value.Next(true);
                    var randomSeed = EnumUtils.GetRandom<BlockSeed>();
                    var newBlock = _blockFactory.Create(nextValue, seed);
                    firstCell.Block = newBlock;
                    newBlock.transform.localScale = Vector3.zero;
                    newBlock.transform.DOScale(Vector3.one, _animationSettings.MergeDuration).SetEase(Ease.OutBack);

                    foreach (var seedInGroup in seedHash)
                    {
                        Debug.Log("Seed in group: " + seedInGroup);
                        var info = new BlockToFoundationInfo(seedInGroup, value, firstCell.Position);
                        if (_foundationsController.TryAndAttach(info))
                        {
                            _sfxManager.PlayOneShot(Consts.GetFoundationSFX(seedInGroup));
                            _gameController.Score += Consts.FOUNDATION_POINTS;
                        }
                    }
                    
                });
                
                sequence.Append(groupSequence);
            }
            
            sequence.OnComplete(() =>
            {
                MergeCompleted = true;
            });
            sequence.Play();
        }


        private List<List<Cell>> GetAllMergableCells()
        {
            var mergedGroups = new List<List<Cell>>();
            var visited = new bool[_board.Width, _board.Height];
            for(var x= 0; x < _board.Width; x++)
            {
                for(var y = _board.Height-1; y>=0; y--)
                {
                    if (visited[x, y]) continue;
                    var cell = _board.GetCell(x, y);
                    if (cell.IsEmpty) continue;
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
                        nextCell.Block.Value != value) continue;
                    visited[nextCell.Coordinates.x, nextCell.Coordinates.y] = true;
                    queue.Enqueue(nextCell);
                }
            }
            
            return mergeableCells;
        }
    }
}