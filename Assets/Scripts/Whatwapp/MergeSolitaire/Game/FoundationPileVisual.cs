using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationPileVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        public void SetBackgroundSprite(Sprite foundationSprite)
        {
            _backgroundSpriteRenderer.sprite = foundationSprite;
        }
    }
}