using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class Board : MonoBehaviour
    {
        private Cell[,] _cells;
        public List<Cell> Cells { get; } = new();

        public int Width => _width;
        public int Height => _height;

        private int _width;
        private int _height;

        public void Init(int width, int height)
        {
            if (_cells != null)
            {
                DestroyCells();
            }

            _width = width;
            _height = height;
            _cells = new Cell[_width, _height];
        }

        public void AddCell(Cell cell)
        {
            _cells[cell.Coordinates.x, cell.Coordinates.y] = cell;
            Cells.Add(cell);
        }

        public void DestroyCells()
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }

            _cells = null;
            Cells.Clear();
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return null;
            }

            return _cells[x, y];
        }

        public Cell GetCell(Vector2Int coord)
        {
            return GetCell(coord.x, coord.y);
        }

        public void AddStartingBlock(Block block, Vector2Int vector2Int)
        {
            var cell = GetCell(vector2Int);
            cell.Block = block;
            block.transform.position = cell.Position;
        }

        public List<Cell> GetEmptyCells()
        {
            return Cells.Where(cell => cell.IsEmpty).ToList();
        }

        public Cell GetCellAtPosition(Vector3 worldPosition)
        {
            var coordinates = GetCellCoordinates(worldPosition);
            return GetCell(coordinates.x, coordinates.y);
        }

        public Vector2Int GetCellCoordinates(Vector3 worldPosition)
        {
            var cellSize = 1f;
            var halfCellSize = cellSize / 2f;
            worldPosition += new Vector3((_width * halfCellSize) + halfCellSize, 0, -_height * halfCellSize);
            worldPosition -= transform.position;

            var x = Mathf.FloorToInt(worldPosition.x / cellSize);
            var y = _height / 2 - Mathf.FloorToInt(-worldPosition.y / cellSize);

            return new Vector2Int(x, y);
        }

        public List<Block> GetAttachableBlocks()
        {
            var result = new List<Block>();
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var cell = GetCell(x, y);
                    if (cell.IsEmpty) continue;
                    result.Add(cell.Block);
                    break;
                }
            }

            return result;
        }

        public List<Cell> GetCellInColumn(int x)
        {
            var result = new List<Cell>();
            for (var y = 0; y < _height; y++)
            {
                var cell = GetCell(x, y);
                result.Add(cell);
            }

            return result;
        }

        public List<Cell> GetNeighbors(Cell cell, int distance)
        {
            var result = new List<Cell>();
            var directions = new Vector2Int[]
            {
                new(distance, 0), new(-distance, 0), new(0, distance), new(0, -distance)
            };
            foreach (var dir in directions)
            {
                var neighbor = GetCell(cell.Coordinates + dir);
                if (neighbor != null)
                {
                    result.Add(neighbor);
                }
            }

            return result;
        }
    }
}