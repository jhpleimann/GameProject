using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameProject
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private MainPlayer player;
        private Texture2D texture2;

        private GrassFloor[] grass;

        private TrapGrassFloor trapGrass;

        private LevelEntrance levelEntrance;

        private SpriteFont spriteFont;

        private SoundEffect holyDeath;
        private SoundEffect levelEnd;
        private Song backgroundMusic;

        private int gameLevel = 0;

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
            trapGrass = new TrapGrassFloor(new Vector2(480,416));
            levelEntrance = new LevelEntrance(new Vector2(736, 288));
            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            foreach (var piece in grass) piece.LoadContent(Content);
            trapGrass.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("Arial");
            texture2 = Content.Load<Texture2D>("Pixel");
            levelEntrance.LoadContent(Content);
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
            if(player.GameStarted)
            {
                gameLevel = 1;
            }
            player.Update(gameTime);
            player.OnGround = false;
            foreach (var grassPiece in grass)
            {
                if(grassPiece.Bounds.CollidesWith(player.Bounds))
                {
                    player.OnGround = true;
                }
            }
            if(levelEntrance.Bounds.CollidesWith(player.Bounds))
            {
                levelEnd.Play();
                Exit();
            }
            if(trapGrass.Bounds.CollidesWith(player.Bounds) && !player.Dead)
            {
                trapGrass.PlayerCollide = true;
                player.Dead = true;
                holyDeath.Play();
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
        }
    }
}
