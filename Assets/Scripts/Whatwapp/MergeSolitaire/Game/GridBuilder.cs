using System.Collections.Generic;
using UnityEngine;
using Whatwapp.Core.Cameras;

namespace Whatwapp.MergeSolitaire.Game
{
    public class GridBuilder : MonoBehaviour
    {
        
        [Header("Grid Settings")] 
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 7;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Lane _lanePrefab;
        
        private bool _isReady;
        private Board _board;
        private BlockFactory _blockFactory;

        public void Init(Board board, BlockFactory blockFactory)
        {
            _isReady = false;
            _board = board;
            _blockFactory = blockFactory;
            _board.Init(_width, _height);
        }

        public void CreateGrid()
        {
            CreateCells();
            PrepareBlocks();

            _isReady = true;
        }

        private void CreateCells()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var coordinate = new Vector2Int(x, y);
                    
                    // Set the position of the cell so the grid is centered in _gridParent
                    var position = _board.transform.position;
                    position.x += x - _width / 2f;
                    position.y += y - _height / 2f;

                    var cell = Instantiate(_cellPrefab, position, Quaternion.identity, _board.transform);
                    cell.Init(new Vector2Int(x, y));
                    _board.AddCell(cell);
                }
                
                // Create the lane background
                var lanePosition = _board.transform.position;
                lanePosition.x += x - _width / 2f;
                lanePosition.y += -0.81f;
                var lane = Instantiate(_lanePrefab, lanePosition, Quaternion.identity,  _board.transform);
                lane.Initialize(x, _height);
            }

        }
        
        private void PrepareBlocks()
        {
            for(var i=0; i<_width; i++)
            {
                var numberOfBlocks = i + 1;
                for (var j = 0; j < numberOfBlocks; j++)
                {
                    var block = _blockFactory.CreateStartingBlock();
                    _board.AddStartingBlock(block, new Vector2Int(i, j));
                }
            }
        }
        

        public bool IsReady()
        {
            return _isReady;
        }
        
        public void DestroyGrid()
        {
            _isReady = false;
            _board.DestoryCells();
        }
    }
}