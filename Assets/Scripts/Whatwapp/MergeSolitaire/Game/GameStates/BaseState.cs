using UnityEngine;
using Whatwapp.Core.FSM;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public abstract class BaseState : IState
    {
        protected GameController _gameController;

        protected BaseState(GameController gameController)
        {
            _gameController = gameController;
        }

        public virtual void OnEnter()
        {
            Debug.Log("Entering state: " + GetType().Name);
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}