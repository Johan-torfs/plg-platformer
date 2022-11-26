using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class PlatformController : MonoBehaviour
    {
        public PatrolPath path;
        public float maxSpeed = 2.0f;
        internal PatrolPath.Mover mover;
        SpriteRenderer spriteRenderer;
        BoxCollider2D collider2d;

        public Sprite[] platformTiles;
        public Sprite finalSprite;

        public int width = 3;
        public int tileResolution = 100; 
        public int pixelsPerUnit = 300;

        internal Vector2 move;

        void Awake() {
            Draw();
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider2d = GetComponent<BoxCollider2D>();

            spriteRenderer.sprite = finalSprite;
            collider2d.size = new Vector2(tileResolution * width / pixelsPerUnit, tileResolution * 0.8f / pixelsPerUnit);
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(maxSpeed * 0.5f);
                move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
                transform.Translate(move);
            }
        }

        private void Draw() {
            Resources.UnloadUnusedAssets();
            if (finalSprite == null)
            {
                Texture2D newTexture = new Texture2D(tileResolution * width, tileResolution);

                // Set all pixels to transparent
                for (int x = 0; x < newTexture.width; x++) 
                {
                    for (int y = 0; y < newTexture.width; y++) 
                    {
                        newTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
                    }
                }

                // Start with offset 0 (leftmost tile) and increase with every drawn tile
                int offset = 0;

                // Paint left tile
                for (int x = 0; x < tileResolution; x++) 
                {
                    for (int y = 0; y < tileResolution; y++) 
                    {
                        newTexture.SetPixel(x + offset * tileResolution, y, platformTiles[0].texture.GetPixel(x, y));
                    }
                }
                offset++;

                // Paint middle tiles
                for (int i = 1; i < width - 1; i++) 
                {
                    for (int x = 0; x < tileResolution; x++) 
                    {
                        for (int y = 0; y < tileResolution; y++) 
                        {
                            newTexture.SetPixel(x + offset * tileResolution, y, platformTiles[1].texture.GetPixel(x, y));
                        }
                    }
                    offset++;
                }

                // Paint right tile
                for (int x = 0; x < tileResolution; x++) 
                {
                    for (int y = 0; y < tileResolution; y++) 
                    {
                        newTexture.SetPixel(x + offset * tileResolution, y, platformTiles[2].texture.GetPixel(x, y));
                    }
                }

                // Apply texture
                newTexture.Apply();
                
                // Set final sprite
                finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            }
        }
    }
}
