using UnityEngine;

namespace Whatwapp.Core.Utils.Executables
{
    public class LoadSceneExecutable : MonoBehaviour, IExecutable
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private bool _additive;
        
        public void Execute()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName,
                _additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive :
                    UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}