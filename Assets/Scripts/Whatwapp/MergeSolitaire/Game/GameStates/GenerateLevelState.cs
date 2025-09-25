using System.Linq;
using Whatwapp.Core.Cameras;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class GenerateLevelState : BaseState
    {
        private readonly GridBuilder _gridBuilder;
        private readonly TargetBoundedOrthographicCamera _targetBoundedCamera;
        private readonly Board _board;
        private readonly BlockFactory _blockFactory;

        public GenerateLevelState(GameController gameController, Board board, GridBuilder gridBuilder,
            BlockFactory blockFactory,
            TargetBoundedOrthographicCamera targetBoundedCamera) : base(gameController)
        {
            _gridBuilder = gridBuilder;
            _targetBoundedCamera = targetBoundedCamera;
            _board = board;
            _blockFactory = blockFactory;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _gridBuilder.Init(_board, _blockFactory);
            _gridBuilder.CreateGrid();
            var targets = _board.Cells.Select(cell => cell.transform).ToList();
            _targetBoundedCamera.AddTargets(targets);
        }
    }
}