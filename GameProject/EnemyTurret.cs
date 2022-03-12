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
    public class EnemyTurret
    {
        private Vector2 position;

        private Texture2D texture;

        private Texture2D texture2;

        private BoundingRectangle bounds;

        private double animationTimer;

        private double anotherTimer;

        Vector2 direction;

        public bool Shot = false;

        public float Angle { get; set; } = -10;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public EnemyTurret(Vector2 position)
        {
            this.position = position;
            //this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("EnemyTurret");
            texture2 = content.Load<Texture2D>("Pixel");
            this.bounds = new BoundingRectangle(position, texture.Width, texture.Height);
        }

        /// <summary>
        /// Updates the sprite's angle based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                Angle += .005f;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Angle -= .005f;
            }
            direction.X = (float)Math.Sin(Angle);
            direction.Y = (float)-Math.Cos(Angle);



            anotherTimer += gameTime.ElapsedGameTime.TotalSeconds;
            Shot = false;
            if (anotherTimer > 3.0)
            {
                anotherTimer = 0;
            }
            else
            {
                //const double timerInterval = 0.2f;
                //var aTimer = anotherTimer % timerInterval;
                if (anotherTimer < .6)
                {
                    Shot = true;
                }
            }
        }

        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.2)
            {
                spriteBatch.Draw(texture, position, null, Color.Black, Angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                animationTimer -= 0.2;
            }
            else
            {
                spriteBatch.Draw(texture, position, null, Color.LightPink, Angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
