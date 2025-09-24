using UnityEngine;
using UnityEngine.Serialization;
using Whatwapp.Core.Cameras;
using Whatwapp.Core.FSM;
using Whatwapp.Core.Audio;
using Whatwapp.MergeSolitaire.Game.GameStates;
using Whatwapp.MergeSolitaire.Game.UI;

namespace Whatwapp.MergeSolitaire.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Board board;
        [SerializeField] private GridBuilder _gridBuilder;
        [SerializeField] private TargetBoundedOrthographicCamera _targetBoundedCamera;
        [SerializeField] private BlockFactory _blockFactory;
        [SerializeField] private NextBlockController _nextBlockController;
        [SerializeField] private FoundationsController _foundationsController;
        
        [SerializeField] private ScoreBox _scoreBox;

        [Header("Settings")]
        [SerializeField] private AnimationSettings _animationSettings;

        private StateMachine _stateMachine;
        private SFXManager _sfxManager;

        private int _score = 0;
        private int _highScore = 0;
        private bool _isPaused = false;
        
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
            }
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
                _scoreBox.SetScore(_score);
                PlayerPrefs.SetInt(Consts.PREFS_LAST_SCORE, _score);
            }
        }

        private void Start()
        {
            _stateMachine = new StateMachine();
            _sfxManager = SFXManager.Instance;

            var generateLevel = new GenerateLevelState(this, board, _gridBuilder, _blockFactory, _targetBoundedCamera);
            var extractBlock = new ExtractBlockState(this, _nextBlockController, _sfxManager);
            var moveBlocks = new MoveBlocksState(this, board, _animationSettings);
            var mergeBlocks = new MergeBlocksState(this, board, _blockFactory, _foundationsController, 
                _sfxManager, _animationSettings);
            var playBlockState = new PlayBlockState(this, board, _nextBlockController, _sfxManager);
            var gameOver = new GameOverState(this,  _sfxManager);
            var victory = new VictoryState(this,  _sfxManager);


            _stateMachine.AddTransition(generateLevel, extractBlock, 
                new Predicate(() => _gridBuilder.IsReady()));
            
            _stateMachine.AddTransition(extractBlock, moveBlocks, 
                new Predicate(() => extractBlock.ExtractCompleted));
            
            _stateMachine.AddTransition(moveBlocks, mergeBlocks, 
                new Predicate(() => moveBlocks.CanMoveBlocks() == false));
            
            _stateMachine.AddTransition(mergeBlocks, victory,
                new Predicate(() => _foundationsController.AllFoundationsCompleted));
            _stateMachine.AddTransition(mergeBlocks, moveBlocks,
                new Predicate(() =>
                    mergeBlocks.MergeCompleted && mergeBlocks.MergeCount > 0 
                                               && ! _foundationsController.AllFoundationsCompleted));  
            _stateMachine.AddTransition(mergeBlocks, playBlockState, 
                new Predicate(() => mergeBlocks.MergeCompleted && mergeBlocks.MergeCount == 0 
                                                               && ! _foundationsController.AllFoundationsCompleted));

            _stateMachine.AddTransition(playBlockState, extractBlock, 
                new Predicate(() =>  playBlockState.PlayBlockCompleted));
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