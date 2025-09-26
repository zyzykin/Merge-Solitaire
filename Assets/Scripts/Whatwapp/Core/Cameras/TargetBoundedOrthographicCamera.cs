using System.Collections.Generic;
using UnityEngine;

namespace Whatwapp.Core.Cameras
{
    public class TargetBoundedOrthographicCamera : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Camera orthographicCamera; // Reference to the orthographic camera

        [SerializeField] private List<Transform> targets; // List of targets to follow

        [Header("Settings")] [SerializeField] private float topMargin = 2f;
        [SerializeField] private float bottomMargin = 2f;
        [SerializeField] private float leftMargin = 2f;
        [SerializeField] private float rightMargin = 2f;

        [SerializeField] private bool dynamicTargets;

        private void Start()
        {
            if (orthographicCamera == null)
            {
                orthographicCamera = Camera.main;
            }

            // Check if the camera is orthographic
            if (orthographicCamera.orthographic == false)
            {
                Debug.LogError("The camera is not orthographic");
            }
        }

        private void Update()
        {
            if (dynamicTargets)
            {
                UpdateCameraSize();
            }
        }

        public void SetTargets(List<Transform> newTargets)
        {
            targets = newTargets;
            UpdateCameraSize();
        }

        public void AddTarget(Transform target)
        {
            if (targets == null)
            {
                targets = new List<Transform>();
            }

            targets.Add(target);
            UpdateCameraSize();
        }

        public void RemoveTarget(Transform target)
        {
            if (targets == null) return;
            targets.Remove(target);
            UpdateCameraSize();
        }

        public void AddTargets(List<Transform> targets)
        {
            if (this.targets == null)
            {
                this.targets = new List<Transform>();
            }

            this.targets.AddRange(targets);
            UpdateCameraSize();
        }

        public void RemoveTargets(List<Transform> targets)
        {
            if (this.targets == null) return;
            foreach (var target in targets)
            {
                this.targets.Remove(target);
            }

            UpdateCameraSize();
        }

        public void ClearTargets()
        {
            if (targets == null) return;
            targets.Clear();
            UpdateCameraSize();
        }

        private void UpdateCameraSize()
        {
            if (targets == null || targets.Count == 0) return;

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var target in targets)
            {
                if (target.position.x < minX) minX = target.position.x;
                if (target.position.y < minY) minY = target.position.y;
                if (target.position.x > maxX) maxX = target.position.x;
                if (target.position.y > maxY) maxY = target.position.y;
            }

            var width = maxX - minX + leftMargin + rightMargin;
            var height = maxY - minY + topMargin + bottomMargin;

            var aspectRatio = orthographicCamera.aspect;
            var cameraSize = Mathf.Max(width / (2 * aspectRatio), height / 2);

            orthographicCamera.orthographicSize = cameraSize;

            var cameraCenter = new Vector3((minX + maxX) / 2, (minY + maxY) / 2,
                orthographicCamera.transform.position.z);
            orthographicCamera.transform.position = cameraCenter;
        }

        private void OnDrawGizmos()
        {
            if (targets == null || targets.Count == 0) return;

            Gizmos.color = Color.red;
            foreach (var target in targets)
            {
                Gizmos.DrawSphere(target.position, 0.1f);
            }

            Gizmos.color = Color.green;
            var bottomLeft =
                new Vector3(
                    orthographicCamera.transform.position.x -
                    orthographicCamera.aspect * orthographicCamera.orthographicSize,
                    orthographicCamera.transform.position.y - orthographicCamera.orthographicSize, 0);
            var topRight =
                new Vector3(
                    orthographicCamera.transform.position.x +
                    orthographicCamera.aspect * orthographicCamera.orthographicSize,
                    orthographicCamera.transform.position.y + orthographicCamera.orthographicSize, 0);

            Gizmos.DrawLine(bottomLeft, new Vector3(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(bottomLeft, new Vector3(topRight.x, bottomLeft.y, 0));
            Gizmos.DrawLine(topRight, new Vector3(bottomLeft.x, topRight.y, 0));
            Gizmos.DrawLine(topRight, new Vector3(topRight.x, bottomLeft.y, 0));
        }
    }
}