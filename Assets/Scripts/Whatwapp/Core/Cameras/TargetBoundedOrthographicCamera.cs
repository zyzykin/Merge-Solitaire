using System.Collections.Generic;
using UnityEngine;

namespace Whatwapp.Core.Cameras
{
    public class TargetBoundedOrthographicCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _orthographicCamera; // Reference to the orthographic camera
        [SerializeField] private List<Transform> _targets; // List of targets to follow
        
        
        [Header("Settings")]
        [SerializeField] private float _topMargin = 2f;
        [SerializeField] private float _bottomMargin = 2f;
        [SerializeField] private float _leftMargin = 2f;
        [SerializeField] private float _rightMargin = 2f;

        [SerializeField] private bool _dynamicTargets = false;
        

        private void Start()
        {
            if (_orthographicCamera == null)
            {
                _orthographicCamera = Camera.main;
            }
            // Check if the camera is orthographic
            if (_orthographicCamera.orthographic == false)
            {
                Debug.LogError("The camera is not orthographic");
                return;
            }
        }
        
        private void Update()
        {
            if (_dynamicTargets)
            {
                UpdateCameraSize();
            }
        }

        public void SetTargets(List<Transform> newTargets)
        {
            _targets = newTargets;
            UpdateCameraSize();
        }
        
        public void AddTarget(Transform target)
        {
            if (_targets == null)
            {
                _targets = new List<Transform>();
            }
            _targets.Add(target);
            UpdateCameraSize();
        }
        
        public void RemoveTarget(Transform target)
        {
            if (_targets == null) return;
            _targets.Remove(target);
            UpdateCameraSize();
        }
        
        public void AddTargets(List<Transform> targets)
        {
            if (_targets == null)
            {
                _targets = new List<Transform>();
            }
            _targets.AddRange(targets);
            UpdateCameraSize();
        }
        
        public void RemoveTargets(List<Transform> targets)
        {
            if (_targets == null) return;
            foreach (var target in targets)
            {
                _targets.Remove(target);
            }
            UpdateCameraSize();
        }
        
        public void ClearTargets()
        {
            if (_targets == null) return;
            _targets.Clear();
            UpdateCameraSize();
        }
        
        
        

        private void UpdateCameraSize()
        {
            if (_targets == null || _targets.Count == 0) return;

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var target in _targets)
            {
                if (target.position.x < minX) minX = target.position.x;
                if (target.position.y < minY) minY = target.position.y;
                if (target.position.x > maxX) maxX = target.position.x;
                if (target.position.y > maxY) maxY = target.position.y;
            }

            var width = maxX - minX + _leftMargin + _rightMargin;
            var height = maxY - minY + _topMargin + _bottomMargin;

            var aspectRatio = _orthographicCamera.aspect;
            var cameraSize = Mathf.Max(width / (2 * aspectRatio), height / 2);

            _orthographicCamera.orthographicSize = cameraSize;

            var cameraCenter = new Vector3((minX + maxX) / 2, (minY + maxY) / 2,
                _orthographicCamera.transform.position.z);
            _orthographicCamera.transform.position = cameraCenter;
        }

        private void OnDrawGizmos()
        {
            if (_targets == null || _targets.Count == 0) return;

            Gizmos.color = Color.red;
            foreach (var target in _targets)
            {
                Gizmos.DrawSphere(target.position, 0.1f);
            }

            Gizmos.color = Color.green;
            var bottomLeft =
                new Vector3(
                    _orthographicCamera.transform.position.x -
                    _orthographicCamera.aspect * _orthographicCamera.orthographicSize,
                    _orthographicCamera.transform.position.y - _orthographicCamera.orthographicSize, 0);
            var topRight =
                new Vector3(
                    _orthographicCamera.transform.position.x +
                    _orthographicCamera.aspect * _orthographicCamera.orthographicSize,
                    _orthographicCamera.transform.position.y + _orthographicCamera.orthographicSize, 0);

            Gizmos.DrawLine(bottomLeft, new Vector3(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(bottomLeft, new Vector3(topRight.x, bottomLeft.y, 0));
            Gizmos.DrawLine(topRight, new Vector3(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(topRight, new Vector3(topRight.x, bottomLeft.y, 0));
        }
    }
}