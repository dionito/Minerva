using UnityEngine;

namespace MinervaUI.Assets.Scripts
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        int width;

        [SerializeField]
        int height;

        [SerializeField]
        Tile tile;

        [SerializeField]
        Transform camera;

        void GenerateGrid()
        {
            // Generate a grid of tiles
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.height; x++)
                {
                    // Create a new tile
                    var gameTile = Instantiate(this.tile, new Vector3(x, y, 0), Quaternion.identity);
                    gameTile.name = $"{x}{y}";
                    gameTile.SetColor((x + y) % 2 == 0);
                }
            }

            Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            this.camera.transform.position = bottomLeft;

            // Center the camera
            this.camera.transform.position = new Vector3(
                this.width,
                (float)this.height / 2,
                0);
        }

        void Start()
        {
            this.GenerateGrid();
        }

    }
}
