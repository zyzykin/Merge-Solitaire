using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class ExtractBlockState : BaseState
    {
        private readonly NextBlockController _nextBlockController;
        private readonly ISFXPresenter _sfxPresenter;

        public ExtractBlockState(GameController gameController, NextBlockController nextBlockController,
            ISFXPresenter sfxPresenter) : base(gameController)
        {
            _nextBlockController = nextBlockController;
            _sfxPresenter = sfxPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (_nextBlockController.HasBlock) return;
            _nextBlockController.ExtractNextBlock();
            _sfxPresenter.PlayOneShot(Consts.SFX_ExtractBlock);
        }

        public bool ExtractCompleted => _nextBlockController.IsReady;
    }
}