using UnityEngine;
using Whatwapp.Core.FSM;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public abstract class BaseState : IState
    {
        protected readonly GameController GameController;

        protected BaseState(GameController gameController)
        {
            GameController = gameController;
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