using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GameProject
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private MainPlayer player;
        private EnemyPlayer enemyPlayerOne;
        private Texture2D texture2;

        private GrassFloor[] grass;

        private MetalFloor[] metal;

        private TrapGrassFloor trapGrass;

        private LevelEntrance levelEntrance;

        private SpriteFont spriteFont;

        private EnemyTurret enemyTurret;

        private Elevator elevator;

        private SoundEffect holyDeath;
        private SoundEffect levelEnd;
        private Song backgroundMusic;

        private int gameLevel = 0;

        public List<EnemyLaser> lasers = new List<EnemyLaser>();

        /// <summary>
        /// A basic game with a title screen
        /// </summary>
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game 
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Guard of Vengance";
            player = new MainPlayer();
            enemyPlayerOne = new EnemyPlayer(new Vector2(32, 352));
            grass = new GrassFloor[]
            {
                new GrassFloor(new Vector2(800, 416)),
                new GrassFloor(new Vector2(736, 416)),
                new GrassFloor(new Vector2(672, 416)),
                new GrassFloor(new Vector2(608, 416)),
                new GrassFloor(new Vector2(544, 416)),
                new GrassFloor(new Vector2(416, 416)),
                new GrassFloor(new Vector2(352, 416)),
                new GrassFloor(new Vector2(288, 416)),
                new GrassFloor(new Vector2(224, 416)),
                new GrassFloor(new Vector2(160, 416)),
                new GrassFloor(new Vector2(96, 416)),
                new GrassFloor(new Vector2(32, 416)),
                new GrassFloor(new Vector2(0, 416)),
            };
            metal = new MetalFloor[]
            {
                new MetalFloor(new Vector2(992, 416)),
                new MetalFloor(new Vector2(928, 416)),
                new MetalFloor(new Vector2(864, 416)),
                new MetalFloor(new Vector2(800, 416)),
                new MetalFloor(new Vector2(736, 416)),
                new MetalFloor(new Vector2(672, 416)),
                new MetalFloor(new Vector2(608, 416)),
                new MetalFloor(new Vector2(544, 416)),
                new MetalFloor(new Vector2(480, 416)),
                new MetalFloor(new Vector2(416, 416)),
                new MetalFloor(new Vector2(352, 416)),
                new MetalFloor(new Vector2(288, 416)),
                new MetalFloor(new Vector2(224, 416)),
                new MetalFloor(new Vector2(160, 416)),
                new MetalFloor(new Vector2(96, 416)),
                new MetalFloor(new Vector2(32, 416)),
                new MetalFloor(new Vector2(0, 416)),
            };
            trapGrass = new TrapGrassFloor(new Vector2(480,416));
            levelEntrance = new LevelEntrance(new Vector2(736, 288));
            elevator = new Elevator(new Vector2(864, 288));
            enemyTurret = new EnemyTurret(new Vector2(320, 64));
            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            enemyPlayerOne.LoadContent(Content);
            foreach (var piece in grass) piece.LoadContent(Content);
            foreach (var piece in metal) piece.LoadContent(Content);
            trapGrass.LoadContent(Content);
            enemyTurret.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("Arial");
            texture2 = Content.Load<Texture2D>("Pixel");
            levelEntrance.LoadContent(Content);
            elevator.LoadContent(Content);
            levelEnd = Content.Load<SoundEffect>("EndLevelSound");
            holyDeath = Content.Load<SoundEffect>("HolyDeathSound");
            backgroundMusic = Content.Load<Song>("GameMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = .01f;
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(player.GameStarted && gameLevel == 0)
            {
                gameLevel = 1;
            }
            player.Update(gameTime);
            player.OnGround = false;
            if(gameLevel == 1)
            {
                foreach (var grassPiece in grass)
                {
                    if (grassPiece.Bounds.CollidesWith(player.Bounds))
                    {
                        player.OnGround = true;
                    }
                }
                if (levelEntrance.Bounds.CollidesWith(player.Bounds))
                {
                    levelEnd.Play();
                    gameLevel = 2;
                }
                if (trapGrass.Bounds.CollidesWith(player.Bounds) && !player.Dead)
                {
                    trapGrass.PlayerCollide = true;
                    player.Dead = true;
                    holyDeath.Play();
                }
            }
            if(gameLevel == 2)
            {
                foreach (var metalPiece in metal)
                {
                    if (metalPiece.Bounds.CollidesWith(player.Bounds))
                    {
                        player.OnGround = true;
                    }
                }
                if (enemyPlayerOne.Bounds.CollidesWith(player.Bounds))
                {
                    lasers = new List<EnemyLaser>();
                    gameLevel++;
                    levelEnd.Play();
                }
                if(enemyPlayerOne.Shot)
                {
                    var laser = new EnemyLaser(new Vector2(32, 352), 0);
                    laser.LoadContent(Content);
                    lasers.Add(laser);
                }
                /*foreach(var laser in lasers)
                {
                    if (laser.Bounds.CollidesWith(player.Bounds))
                    {
                        holyDeath.Play();
                        Exit();
                    }
                }*/
            }
            if(gameLevel == 3)
            {
                player.BoundedScreen = false;
                foreach (var metalPiece in metal)
                {
                    if (metalPiece.Bounds.CollidesWith(player.Bounds))
                    {
                        player.OnGround = true;
                    }
                }
                if (enemyTurret.Shot)
                {
                    var laser = new EnemyLaser(new Vector2(320, 64), enemyTurret.Angle);
                    laser.LoadContent(Content);
                    lasers.Add(laser);
                }
                foreach (var laser in lasers)
                {
                    if (laser.Bounds.CollidesWith(player.Bounds))
                    {
                        holyDeath.Play();
                        Exit();
                    }
                }
                if (elevator.Bounds.CollidesWith(player.Bounds))
                {
                    levelEnd.Play();
                    gameLevel = 4;
                }
                enemyTurret.Update(gameTime);
            }
            if (gameLevel == 4)
            {
                player.BoundedScreen = true;
                foreach (var metalPiece in metal)
                {
                    if (metalPiece.Bounds.CollidesWith(player.Bounds))
                    {
                        player.OnGround = true;
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            if(gameLevel == 0)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                player.Draw(gameTime, spriteBatch);
                foreach (var grassPiece in grass)
                {
                    grassPiece.Draw(gameTime, spriteBatch);
                }
                levelEntrance.Draw(gameTime, spriteBatch);
                trapGrass.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(texture2, new Vector2(0,0), null, new Color(0,0,0,.5f), 0, new Vector2(0, 0), 1000.0f, SpriteEffects.None, 0);
                spriteBatch.DrawString(spriteFont, "Guard of Vengance", new Vector2(2, 2), Color.DarkRed);
                spriteBatch.DrawString(spriteFont, "Press escape to end game", new Vector2(2, 200), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Goal: Reach the exit", new Vector2(2, 300), Color.Gold);
                spriteBatch.DrawString(spriteFont, "Press up, left, or right to start the game", new Vector2(2, 350), Color.Gold);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameLevel == 1)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                player.Draw(gameTime, spriteBatch);
                foreach (var grassPiece in grass)
                {
                    grassPiece.Draw(gameTime, spriteBatch);
                }
                levelEntrance.Draw(gameTime, spriteBatch);
                trapGrass.Draw(gameTime, spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameLevel == 2)
            {
                GraphicsDevice.Clear(Color.AntiqueWhite);
                spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                //spriteBatch.Begin();
                player.Draw(gameTime, spriteBatch);
                enemyPlayerOne.Draw(gameTime, spriteBatch, new Color(0.5f, 0.2f, 0.4f, 0.5f));
                foreach (var metalPiece in metal)
                {
                    metalPiece.Draw(gameTime, spriteBatch);
                }
                foreach (var laser in lasers)
                {
                    laser.Draw(gameTime, spriteBatch, new Color(0.5f, 0.2f, 0.4f, 0.5f));
                }
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameLevel == 3)
            {
                float playerX = MathHelper.Clamp(player.Position.X, 300, 13600);
                float offsetX = 300 - playerX;

                Matrix transform = Matrix.CreateTranslation(offsetX * 0.333f, 0, 0);
                spriteBatch.Begin(transformMatrix: transform);
                GraphicsDevice.Clear(Color.AntiqueWhite);
                //spriteBatch.DrawString(spriteFont, "You WIN. Press escape to close the game", new Vector2(2, 200), Color.Black);
                player.Draw(gameTime, spriteBatch);
                foreach (var metalPiece in metal)
                {
                    metalPiece.Draw(gameTime, spriteBatch);
                }
                foreach (var laser in lasers)
                {
                    laser.Draw(gameTime, spriteBatch, Color.PaleVioletRed);
                }
                elevator.Draw(gameTime, spriteBatch);
                enemyTurret.Draw(gameTime, spriteBatch);
                spriteBatch.End();
                base.Draw(gameTime);
            }
            else if (gameLevel == 4)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.AntiqueWhite);
                spriteBatch.DrawString(spriteFont, "You WIN. Press escape to close the game", new Vector2(2, 200), Color.Black);
                player.Draw(gameTime, spriteBatch);
                foreach (var metalPiece in metal)
                {
                    metalPiece.Draw(gameTime, spriteBatch);
                }
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
