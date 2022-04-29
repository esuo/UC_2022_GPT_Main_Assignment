using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using RC_Framework;

namespace MainAssignment
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        RC_GameStateManager levelManager;

        static public KeyboardState keyState;
        static public KeyboardState prevKeyState;

        SoundEffectInstance bgm;
        SoundEffect _bgm;


        bool showBB = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 700;
            _graphics.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _bgm = Content.Load<SoundEffect>("Caketown 1");
            bgm = _bgm.CreateInstance();
            bgm.Play();

            LineBatch.init(GraphicsDevice);

            levelManager = new RC_GameStateManager();

            levelManager.AddLevel(0, new GameLevel_Menu());
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();
            
            levelManager.AddLevel(1, new GameLevel_Level_1());

            levelManager.AddLevel(2, new GameLevel_Level_2());
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();

            levelManager.AddLevel(3, new GameLevel_Level_3());
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();

            levelManager.AddLevel(4, new GameLevel_Level_4());
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(4).LoadContent();

            levelManager.AddLevel(5, new GameLevel_Win());
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();

            levelManager.AddLevel(6, new GameLevel_Fail());
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(6).LoadContent();

            levelManager.AddLevel(7, new GameLevel_LeaderBoard());
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            levelManager.AddLevel(8, new GameLevel_LevelChoose());
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();

            levelManager.AddLevel(9, new GameLevel_Help());
            levelManager.getLevel(9).InitializeLevel(GraphicsDevice, _spriteBatch, Content, levelManager);
            levelManager.getLevel(9).LoadContent();

            levelManager.setLevel(0);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (keyState.IsKeyDown(Keys.F1) && !prevKeyState.IsKeyDown(Keys.F1))
                levelManager.pushLevel(9);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            levelManager.getCurrentLevel().Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            levelManager.getCurrentLevel().Draw(gameTime);

            // TODO: Add your drawing code here
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
