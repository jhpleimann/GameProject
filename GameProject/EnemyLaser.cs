using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject.Collisions;

namespace GameProject
{
    public class EnemyLaser
    {
        private Vector2 position;

        private Texture2D texture;

        private Texture2D texture2;

        private BoundingRectangle bounds;

        private float angle;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public EnemyLaser(Vector2 position, float angle)
        {
            this.position = position;
            this.angle = angle;
            //this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("LaserShot");
            texture2 = content.Load<Texture2D>("Pixel");
            this.bounds = new BoundingRectangle(position, texture.Width, texture.Height);
        }

        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            if(angle == 0)
            {
                this.position += new Vector2(5, 0);
            }
            else
            {
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                this.position += velocity * 3;
            }
            this.bounds = new BoundingRectangle(position, texture.Width, texture.Height);
            spriteBatch.Draw(texture, position, null, color, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
        }
    }
}