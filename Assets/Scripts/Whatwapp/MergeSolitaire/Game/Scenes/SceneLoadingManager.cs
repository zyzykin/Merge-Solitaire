using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whatwapp.MergeSolitaire.Game.UI;

namespace Whatwapp.MergeSolitaire.Game.Scenes
{
    public class SceneLoadingManager : MonoBehaviour
    {
        [SerializeField] private float minLoadingTime = 3f;
        private static SceneLoadingManager _instance;

        public static SceneLoadingManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<SceneLoadingManager>();
                if (_instance != null) return _instance;
                var go = new GameObject("LoadingManager");
                _instance = go.AddComponent<SceneLoadingManager>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        public event Action OnLoadingComplete;

        private AsyncOperation _asyncOp;
        private bool _isLoading;
        private float _timer;

        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, float minLoadTime)
        {
            Debug.Log($"Starting LoadSceneAsync for {sceneName}, mode: {mode}, minLoadingTime: {minLoadTime}");

            var currentScene = SceneManager.GetActiveScene();
            var pauseScene = SceneManager.GetSceneByName(Consts.SCENE_PAUSE_MENU);
            var isAdditiveOnly = (mode == LoadSceneMode.Additive && sceneName == Consts.SCENE_PAUSE_MENU);
            var isFromPauseToGame = (sceneName == Consts.SCENE_GAME && pauseScene.isLoaded);
            var isFromPauseToMainMenu = (sceneName == Consts.SCENE_MAIN_MENU && pauseScene.isLoaded);

            var loadingScene = SceneManager.GetSceneByName(Consts.SCENE_LOADING);
            LoadingUI loadingUI = null;
            if (!isFromPauseToGame)
            {
                if (!loadingScene.isLoaded)
                {
                    AsyncOperation loadLoadingOp =
                        SceneManager.LoadSceneAsync(Consts.SCENE_LOADING, LoadSceneMode.Additive);
                    if (loadLoadingOp == null)
                    {
                        Debug.LogError($"Failed to load {Consts.SCENE_LOADING}. Check Build Settings.");
                        _isLoading = false;
                        yield break;
                    }

                    yield return loadLoadingOp;
                    Debug.Log($"Loaded {Consts.SCENE_LOADING} successfully.");
                }
                else
                {
                    Debug.Log($"Scene {Consts.SCENE_LOADING} already loaded.");
                }

                loadingUI = FindObjectOfType<LoadingUI>();
                if (loadingUI == null)
                {
                    Debug.LogError("LoadingUI not found in Loading scene!");
                }
                else
                {
                    loadingUI.StartLoading(minLoadTime);
                    Debug.Log("LoadingUI initialized.");
                }
            }

            if (isFromPauseToGame)
            {
                if (pauseScene.isLoaded)
                {
                    var unloadPauseOp = SceneManager.UnloadSceneAsync(pauseScene);
                    if (unloadPauseOp != null)
                    {
                        yield return unloadPauseOp;
                        Debug.Log($"Unloaded {Consts.SCENE_PAUSE_MENU} for transition to {sceneName}.");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to unload {Consts.SCENE_PAUSE_MENU}.");
                    }
                }

                _asyncOp = null;
            }
            else if (isFromPauseToMainMenu)
            {
                if (currentScene.isLoaded && currentScene.name != sceneName &&
                    currentScene.name != Consts.SCENE_LOADING)
                {
                    var unloadCurrentOp = SceneManager.UnloadSceneAsync(currentScene);
                    if (unloadCurrentOp != null)
                    {
                        yield return unloadCurrentOp;
                        Debug.Log($"Unloaded current scene: {currentScene.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to unload current scene: {currentScene.name}");
                    }
                }

                if (pauseScene.isLoaded)
                {
                    var unloadPauseOp = SceneManager.UnloadSceneAsync(pauseScene);
                    if (unloadPauseOp != null)
                    {
                        yield return unloadPauseOp;
                        Debug.Log($"Unloaded {Consts.SCENE_PAUSE_MENU} for transition to {sceneName}.");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to unload {Consts.SCENE_PAUSE_MENU}.");
                    }
                }
            }
            else if (!isAdditiveOnly)
            {
                if (currentScene.isLoaded && currentScene.name != sceneName &&
                    currentScene.name != Consts.SCENE_LOADING)
                {
                    var unloadCurrentOp = SceneManager.UnloadSceneAsync(currentScene);
                    if (unloadCurrentOp != null)
                    {
                        yield return unloadCurrentOp;
                        Debug.Log($"Unloaded current scene: {currentScene.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to unload current scene: {currentScene.name}");
                    }
                }
            }

            _timer = 0f;

            if (!isFromPauseToGame)
            {
                _asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                if (_asyncOp == null)
                {
                    Debug.LogError($"Failed to load scene {sceneName}.");
                    _isLoading = false;
                    yield break;
                }

                _asyncOp.allowSceneActivation = false;

                while (!_asyncOp.isDone)
                {
                    _timer += Time.deltaTime;

                    if (_asyncOp.progress >= 0.9f && _timer >= minLoadTime)
                    {
                        _asyncOp.allowSceneActivation = true;
                    }

                    yield return null;
                }
            }
            else
            {
                Debug.Log("Skipping Loading scene for Pause to Game transition.");
            }

            if (!isAdditiveOnly && !isFromPauseToGame)
            {
                var newScene = SceneManager.GetSceneByName(sceneName);
                if (newScene.isLoaded)
                {
                    var setActiveResult = SceneManager.SetActiveScene(newScene);
                    Debug.Log($"SetActiveScene {sceneName}: {(setActiveResult ? "Success" : "Failed")}");
                }
                else
                {
                    Debug.LogError($"New scene {sceneName} not loaded properly.");
                }
            }
            else if (isFromPauseToGame)
            {
                var gameScene = SceneManager.GetSceneByName(Consts.SCENE_GAME);
                if (gameScene.isLoaded)
                {
                    var setActiveResult = SceneManager.SetActiveScene(gameScene);
                    Debug.Log($"SetActiveScene {Consts.SCENE_GAME}: {(setActiveResult ? "Success" : "Failed")}");
                }
                else
                {
                    Debug.LogError($"Scene {Consts.SCENE_GAME} not loaded for transition from Pause.");
                }
            }

            if (!isFromPauseToGame)
            {
                if (loadingUI != null)
                {
                    loadingUI.gameObject.SetActive(false);
                    Debug.Log("LoadingUI deactivated.");
                }

                loadingScene = SceneManager.GetSceneByName(Consts.SCENE_LOADING);
                if (loadingScene.isLoaded)
                {
                    AsyncOperation unloadLoadingOp = SceneManager.UnloadSceneAsync(loadingScene);
                    if (unloadLoadingOp != null)
                    {
                        yield return unloadLoadingOp;
                        Debug.Log($"Successfully unloaded {Consts.SCENE_LOADING}.");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to start unloading {Consts.SCENE_LOADING}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"{Consts.SCENE_LOADING} was not loaded, cannot unload.");
                }
            }

            OnLoadingComplete?.Invoke();
            _isLoading = false;
            Debug.Log($"LoadSceneAsync completed for {sceneName}.");
        }

        private void LoadScene(string sceneName, LoadSceneMode mode, float minLoadTime)
        {
            if (_isLoading)
            {
                Debug.LogWarning($"LoadScene {sceneName} ignored: already loading.");
                return;
            }

            _isLoading = true;
            StartCoroutine(LoadSceneAsync(sceneName, mode, minLoadTime));
        }

        public void LoadMainMenu()
        {
            LoadScene(Consts.SCENE_MAIN_MENU, LoadSceneMode.Additive, minLoadingTime);
        }

        public void LoadGame()
        {
            LoadScene(Consts.SCENE_GAME, LoadSceneMode.Additive, minLoadingTime);
        }

        public void LoadEndGame()
        {
            LoadScene(Consts.SCENE_END_GAME, LoadSceneMode.Additive, minLoadingTime);
        }

        public void LoadPauseMenu()
        {
            LoadScene(Consts.SCENE_PAUSE_MENU, LoadSceneMode.Additive, 0f);
        }
    }
}