using UnityEngine;
using Whatwapp.Core.Cameras;
using Whatwapp.Core.FSM;
using Whatwapp.Core.Audio;
using Whatwapp.MergeSolitaire.Game.GameStates;
using Whatwapp.MergeSolitaire.Game.UI;
using Whatwapp.MergeSolitaire.Game.Presentation;

namespace Whatwapp.MergeSolitaire.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Board board;

        [SerializeField] private GridBuilder gridBuilder;
        [SerializeField] private TargetBoundedOrthographicCamera targetBoundedCamera;
        [SerializeField] private BlockFactory blockFactory;
        [SerializeField] private NextBlockController nextBlockController;
        [SerializeField] private FoundationsController foundationsController;

        [SerializeField] private ScoreBox scoreBox;

        [Header("Presentation")] [SerializeField]
        private BlockAnimationPresenter blockAnimationPresenter;

        [SerializeField] private SFXPresenter sfxPresenter;

        private StateMachine _stateMachine;

        private int _score;
        private int _highScore;
        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set { _isPaused = value; }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                if (_score > _highScore)
                {
                    _highScore = _score;
                    PlayerPrefs.SetInt(Consts.PREFS_HIGHSCORE, _highScore);
                }

                scoreBox.SetScore(_score);
                PlayerPrefs.SetInt(Consts.PREFS_LAST_SCORE, _score);
            }
        }

        private void Start()
        {
            _stateMachine = new StateMachine();

            sfxPresenter.Initialize(SFXManager.Instance);
            blockAnimationPresenter.Initialize(sfxPresenter);

            var generateLevel = new GenerateLevelState(this, board, gridBuilder, blockFactory, targetBoundedCamera);
            var extractBlock = new ExtractBlockState(this, nextBlockController, sfxPresenter);
            var moveBlocks = new MoveBlocksState(this, board, blockAnimationPresenter);
            var mergeBlocks = new MergeBlocksState(this, board, blockFactory, foundationsController,
                sfxPresenter, blockAnimationPresenter);
            var playBlockState =
                new PlayBlockState(this, board, nextBlockController, sfxPresenter, blockAnimationPresenter);
            var gameOver = new GameOverState(this, sfxPresenter);
            var victory = new VictoryState(this, sfxPresenter);

            _stateMachine.AddTransition(generateLevel, extractBlock,
                new Predicate(() => gridBuilder.IsReady()));

            _stateMachine.AddTransition(extractBlock, moveBlocks,
                new Predicate(() => extractBlock.ExtractCompleted));

            _stateMachine.AddTransition(moveBlocks, mergeBlocks,
                new Predicate(() => moveBlocks.CanMoveBlocks() == false));

            _stateMachine.AddTransition(mergeBlocks, victory,
                new Predicate(() => foundationsController.AllFoundationsCompleted));
            _stateMachine.AddTransition(mergeBlocks, moveBlocks,
                new Predicate(() =>
                    mergeBlocks.MergeCompleted && mergeBlocks.MergeCount > 0
                                               && !foundationsController.AllFoundationsCompleted));
            _stateMachine.AddTransition(mergeBlocks, playBlockState,
                new Predicate(() => mergeBlocks.MergeCompleted && mergeBlocks.MergeCount == 0
                                                               && !foundationsController.AllFoundationsCompleted));

            _stateMachine.AddTransition(playBlockState, extractBlock,
                new Predicate(() => playBlockState.PlayBlockCompleted));
            _stateMachine.AddTransition(playBlockState, gameOver,
                new Predicate(() => playBlockState.GameOver));

            _stateMachine.SetState(generateLevel);

            _highScore = PlayerPrefs.GetInt(Consts.PREFS_HIGHSCORE, 0);
            Score = 0;
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
    }
}