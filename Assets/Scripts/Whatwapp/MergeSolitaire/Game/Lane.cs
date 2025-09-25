using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class Lane : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private LaneVisual visual;

        private int _index;

        public void Initialize(int index, float height)
        {
            _index = index;
        }
    }
}