using UnityEngine;

namespace Whatwapp.Core.Utils.Executables
{
    public class UnloadSceneExecutable : MonoBehaviour, IExecutable
    {
        [SerializeField] private string _sceneName;
        
        public void Execute()
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_sceneName);
        }
    }
}