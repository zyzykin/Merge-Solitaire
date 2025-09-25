using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class GridBuilder : MonoBehaviour
    {
        [Header("Grid Settings")] [SerializeField]
        private int width = 7;

        [SerializeField] private int height = 10;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Lane lanePrefab;

        private bool _isReady;
        private Board _board;
        private BlockFactory _blockFactory;

        public void Init(Board board, BlockFactory blockFactory)
        {
            _isReady = false;
            _board = board;
            _blockFactory = blockFactory;
            _board.Init(width, height);
        }

        public void CreateGrid()
        {
            CreateCells();
            PrepareBlocks();

            _isReady = true;
        }

        private void CreateCells()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var position = _board.transform.position;
                    position.x += x - width / 2f;
                    position.y += y - height / 2f;

                    var cell = Instantiate(cellPrefab, position, Quaternion.identity, _board.transform);
                    cell.Init(new Vector2Int(x, y));
                    _board.AddCell(cell);
                }

                var lanePosition = _board.transform.position;
                lanePosition.x += x - width / 2f;
                lanePosition.y += -0.81f;
                var lane = Instantiate(lanePrefab, lanePosition, Quaternion.identity, _board.transform);
                lane.Initialize(x, height);
            }
        }

        private void PrepareBlocks()
        {
            for (var i = 0; i < width; i++)
            {
                var numberOfBlocks = i + 1;
                for (var j = 0; j < numberOfBlocks; j++)
                {
                    var block = _blockFactory.CreateStartingBlock(j == 0);
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
            _board.DestroyCells();
        }
    }
}