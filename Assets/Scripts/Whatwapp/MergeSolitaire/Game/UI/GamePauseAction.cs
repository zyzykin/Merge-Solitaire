using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game.UI
{
    public class GamePauseAction : MonoBehaviour
    {
        public void Execute(bool pause)
        {
            var gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.IsPaused = pause;
            }
        }
    }
}