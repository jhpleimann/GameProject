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
    public class LevelEntrance
    {
        private Vector2 position;

        private Texture2D texture;

        private Texture2D texture2;

        private BoundingRectangle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public LevelEntrance(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("DoorEntrance");
            texture2 = content.Load<Texture2D>("Pixel");
            this.bounds = new BoundingRectangle(position, texture.Width, texture.Height);
        }

        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture2, new Vector2(400, 400), Color.Red);
            //spriteBatch.Draw(texture2, new Vector2(position.X, position.Y / 2), null, Color.Purple, 0, new Vector2(0,0), 16.0f, SpriteEffects.None, 0);
        }
    }
}
