using UnityEngine;

namespace MinervaUI.Assets.Scripts
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        Color lightColor;

        [SerializeField]
        Color darkColor;

        [SerializeField]
        SpriteRenderer renderer;

        [SerializeField]
        GameObject highlight;

        public void SetColor(bool isDark)
        {
            this.renderer.color = isDark ? this.darkColor : this.lightColor;
        }

        public void SetHighlight(bool isHighlighted)
        {
            this.highlight.SetActive(isHighlighted);
        }

        void OnMouseEnter()
        {
            this.SetHighlight(true);
        }

        void OnMouseExit()
        {
            this.SetHighlight(false);
        }
    }
}
