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
    public class EnemyPlayer
    {
        private Vector2 position;

        private Texture2D texture;

        private Texture2D texture2;

        private Texture2D texture3;

        private BoundingRectangle bounds;

        public bool Flipped = false;

        private double animationTimer;

        public bool Shot = false;

        private bool shootAnimation = false;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public EnemyPlayer(Vector2 position)
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
            texture = content.Load<Texture2D>("GuardShootingPose1");
            texture2 = content.Load<Texture2D>("GuardShootingPose2");
            texture3 = content.Load<Texture2D>("GuardGunPose");
            this.bounds = new BoundingRectangle(position, texture.Width, texture.Height);
        }

        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            SpriteEffects spriteEffects = (Flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, null, Color.LightGray, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            Shot = false;
            if (animationTimer > 2.0)
            {
                spriteBatch.Draw(texture3, position, null, Color.OrangeRed, 0, new Vector2(0, 0), 1.0f, spriteEffects, 0);
                animationTimer = 0;
            }
            else if(animationTimer < 1.0)
            {
                spriteBatch.Draw(texture3, position, null, Color.OrangeRed, 0, new Vector2(0, 0), 1.0f, spriteEffects, 0);
            }
            else
            {
                const double timerInterval = .4f;
                var aTimer = animationTimer % timerInterval;
                if(aTimer < timerInterval / 2)
                {
                    Shot = true;
                    if (shootAnimation)
                    {
                        spriteBatch.Draw(texture, position, null, Color.OrangeRed, 0, new Vector2(0, 0), 1.0f, spriteEffects, 0);
                        shootAnimation = false;
                    }
                    else
                    {
                        spriteBatch.Draw(texture2, position, null, Color.OrangeRed, 0, new Vector2(0, 0), 1.0f, spriteEffects, 0);
                        shootAnimation = true;
                    }
                }
                else
                {
                    spriteBatch.Draw(texture3, position, null, Color.OrangeRed, 0, new Vector2(0, 0), 1.0f, spriteEffects, 0);
                }
            }
        }
    }
}