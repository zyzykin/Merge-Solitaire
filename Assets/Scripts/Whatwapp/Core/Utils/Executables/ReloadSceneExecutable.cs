using UnityEngine;
using UnityEngine.SceneManagement;

namespace Whatwapp.Core.Utils.Executables
{
    public class ReloadSceneExecutable : MonoBehaviour, IExecutable
    {
        public void Execute()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}