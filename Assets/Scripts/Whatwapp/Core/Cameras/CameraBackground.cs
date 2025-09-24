using DG.Tweening;
using UnityEngine;

namespace Whatwapp.Core.Cameras
{
    public class CameraBackground : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        
        [Header("Settings")]
        [SerializeField] private Color[] _colors;
        [SerializeField] private float _changeInterval = 6f;
        [SerializeField] private float _changeDuration = 2f;
        
        private int _currentColorIndex = 0;
        
        private void Start()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            if (_colors.Length == 0)
            {
                Debug.LogError("No colors set");
                return;
            }
            _currentColorIndex = Random.Range(0, _colors.Length);
            _camera.backgroundColor = _colors[_currentColorIndex];
            
            InvokeRepeating(nameof(ChangeColor), _changeInterval, _changeInterval);
        }

        private void ChangeColor()
        {
            Debug.Log("Changing color");
            _currentColorIndex =Random.Range(0, _colors.Length);
            _camera.DOColor(_colors[_currentColorIndex], _changeDuration);
        }
    }
}