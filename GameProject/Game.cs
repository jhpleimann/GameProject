using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        // private SpikeSprite[] spikes;
        private MainPlayer player;
        //private Texture2D playerImage;

        private GrassFloor[] grass;

        private TrapGrassFloor trapGrass;

        private LevelEntrance levelEntrance;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new MainPlayer();
            grass = new GrassFloor[]
            {//800 x 480
                new GrassFloor(new Vector2(800, 416)),
                new GrassFloor(new Vector2(736, 416)),
                new GrassFloor(new Vector2(672, 416)),
                new GrassFloor(new Vector2(608, 416)),
                new GrassFloor(new Vector2(544, 416)),
                //new GrassFloor(new Vector2(480, 416)),
                new GrassFloor(new Vector2(416, 416)),
                new GrassFloor(new Vector2(352, 416)),
                new GrassFloor(new Vector2(288, 416)),
                new GrassFloor(new Vector2(224, 416)),
                new GrassFloor(new Vector2(160, 416)),
                new GrassFloor(new Vector2(96, 416)),
                new GrassFloor(new Vector2(32, 416)),
                new GrassFloor(new Vector2(0, 416)),
                /*
                new GrassFloor(new Vector2(800, 480)),
                new GrassFloor(new Vector2(736, 480)),
                new GrassFloor(new Vector2(672, 128)),
                new GrassFloor(new Vector2(608, 192)),
                new GrassFloor(new Vector2(544, 256)),
                new GrassFloor(new Vector2(480, 320)),
                new GrassFloor(new Vector2(416, 384)),
                new GrassFloor(new Vector2(352, 384)),
                new GrassFloor(new Vector2(288, 384)),
                new GrassFloor(new Vector2(224, 384)),
                new GrassFloor(new Vector2(160, 384)),
                new GrassFloor(new Vector2(96, 384)),
                new GrassFloor(new Vector2(32, 384)),
                //new GrassFloor(new Vector2(, 384)),
                */

            };
            trapGrass = new TrapGrassFloor(new Vector2(480,416));
            levelEntrance = new LevelEntrance(new Vector2(736, 288));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            foreach (var piece in grass) piece.LoadContent(Content);
            trapGrass.LoadContent(Content);
            levelEntrance.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update(gameTime);
            // TODO: Add your update logic here
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
                Exit();
            }
            if(trapGrass.Bounds.CollidesWith(player.Bounds))
            {
                trapGrass.PlayerCollide = true;
                player.Dead = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //MainPlayer.Draw(gameTime, spriteBatch);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw(gameTime, spriteBatch);
            foreach(var grassPiece in grass)
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
