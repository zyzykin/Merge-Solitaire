using UnityEngine;

namespace Whatwapp.MergeSolitaire.Game
{
    public class FoundationPileVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
        public void SetBackgroundSprite(Sprite foundationSprite)
        {
            backgroundSpriteRenderer.sprite = foundationSprite;
        }
    }
}