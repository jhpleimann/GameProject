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
    public class MainPlayer
    {
        Game game;

        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private Texture2D texture2;

        private Vector2 velocity;

        private Vector2 JumpVelocity = new Vector2(0, 7);

        private Vector2 GravityVelocity = new Vector2(0, 10);

        private bool flipped;

        private BoundingRectangle bounds;

        private double animationTimer;

        private bool animationPose = false;

        private bool jumping = false;

        /// <summary>
        /// True if player should be bound to the screen
        /// </summary>
        public bool BoundedScreen = true;

        /// <summary>
        /// The position of the player
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// True if the player is on the ground, false otherwise
        /// </summary>
        public bool OnGround = false;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Returns true if the plauer is deadl;
        /// </summary>
        public bool Dead { get; set; } = false;

        /// <summary>
        /// Returns if the game has started yet
        /// </summary>
        public bool GameStarted { get; set; } = false;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        ContentManager con;
        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            con = content;
            Position = new Vector2(200, 200);
            velocity = new Vector2(0, 0);
            texture = content.Load<Texture2D>("PlayerPose1");
            texture2 = content.Load<Texture2D>("PlayerPose2");
            bounds = new BoundingRectangle(Position, texture.Width, texture.Height);//new Vector2(200 - 16, 200 - 16)
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            Position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);

            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            if(!GameStarted)
            {
                if((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) ||
                    (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) ||
                    (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)))
                {
                    GameStarted = true;
                }
            }    

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && !jumping && OnGround)
            {
                jumping = true;
                OnGround = false;
                velocity -= JumpVelocity;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-2, 0);
                flipped = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(2, 0);
                flipped = false;
            }
            if(!OnGround && GameStarted)
            {
                velocity += GravityVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(GameStarted)
            {
                velocity.Y = 0;
                jumping = false;
            }
            Position += velocity;
            bounds.X = Position.X;
            bounds.Y = Position.Y;

            if(BoundedScreen)
            {
                if (Position.Y < 0) Position.Y = 800;
                if (Position.Y > 800) Position.Y = 0;
                if (Position.X < 0) Position.X = 800;
                if (Position.X > 800) Position.X = 0;
            }
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Dead) return;
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //Update animation frame
            if (animationTimer > 0.2)
            {
                animationPose = !animationPose;
                animationTimer -= 0.2;
            }
            if(animationPose)
            {
                spriteBatch.Draw(texture, Position, null, Color, 0, new Vector2(0,0), 1.0f, spriteEffects, 0);//64,64
            }
            else
            {
                spriteBatch.Draw(texture2, Position, null, Color, 0, new Vector2(0,0), 1.0f, spriteEffects, 0);
            }
        }
    }
}
