using DG.Tweening;
using UnityEngine;

namespace Whatwapp.Core.Cameras
{
    public class CameraBackground : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        
        [Header("Settings")]
        [SerializeField] private Color[] colors;
        [SerializeField] private float changeInterval = 6f;
        [SerializeField] private float changeDuration = 2f;
        
        private int _currentColorIndex = 0;
        
        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            if (colors.Length == 0)
            {
                Debug.LogError("No colors set");
                return;
            }
            _currentColorIndex = Random.Range(0, colors.Length);
            mainCamera.backgroundColor = colors[_currentColorIndex];
            
            InvokeRepeating(nameof(ChangeColor), changeInterval, changeInterval);
        }

        private void ChangeColor()
        {
            Debug.Log("Changing color");
            _currentColorIndex =Random.Range(0, colors.Length);
            mainCamera.DOColor(colors[_currentColorIndex], changeDuration);
        }
    }
}