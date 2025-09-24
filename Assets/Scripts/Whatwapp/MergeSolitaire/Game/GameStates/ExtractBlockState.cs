using Whatwapp.Core.Audio;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class ExtractBlockState : BaseState
    {
        private NextBlockController _nextBlockController;
        private SFXManager _sfxManager;
        
        public ExtractBlockState(GameController gameController, NextBlockController nextBlockController,
            SFXManager sfxManager) : base(gameController)
        {
            _nextBlockController = nextBlockController;
            _sfxManager = sfxManager;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            if (_nextBlockController.HasBlock) return;
            _nextBlockController.ExtractNextBlock();
            _sfxManager.PlayOneShot(Consts.SFX_ExtractBlock);
        }

        public bool ExtractCompleted => _nextBlockController.IsReady;
    }
}