using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Engine;
using SpaceDefence.Screens;
using System;

/**
 * Even een kleine sidenote. Er zullen voor ene paar berekeningen chatgpt gebruikt zijn. Ik heb voor zo ver ik weet alleen de acceleratie van de alien
 * aangepast met chatgpt. Het is momenteel 7:15, ik heb niet geslapen en ik zie de zon op komen. Ik weet niet of ik morgen wakker wordt. Maar zoals Marcus Aurelius
 * ooit zei: "Wanneer je wakker wordt, bedenk dan dat je nooit meer wakker wordt. En wanneer je in slaap valt, bedenk dan dat je wakker wordt."
 */

namespace SpaceDefence
{
    public class SpaceDefence : Game
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        // start screen stuff bruv
        private StartScreen _startScreen;

        // pause screen stuff bruv
        private PauseScreen _pauseScreen;
        private GameOverScreen _gameOverScreen;

        private Texture2D _background;

        HUD hud;

        public SpaceDefence()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            // set the size of the screen
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _startScreen = new StartScreen();
            _pauseScreen = new PauseScreen();
            _gameOverScreen = new GameOverScreen();


            //_currentState = GameState.StartScreen;
        }

        protected override void Initialize()
        {
            //initialize the GameManager
            _gameManager = GameManager.GetGameManager();
            base.Initialize();
            
            _background = Content.Load<Texture2D>("travisScott");
            // place the player at the center of the screen
            Ship player = new Ship(new Point(GraphicsDevice.Viewport.Width / 2,GraphicsDevice.Viewport.Height / 2));


            // add the starting objects to the GameManager
            _gameManager.Initialize(Content, this, player);

            _gameManager.AddGameObject(new Earth());
            _gameManager.AddGameObject(new Mars());

            _gameManager.AddGameObject(player);
            _gameManager.AddGameObject(new Alien());

            //travis ttesting comment out for chance
            //_gameManager.AddGameObject(new TravisScottBatman());

            // set the game state to the start screen   
            _gameManager.SetGameState(GameState.StartScreen);


            Random rnd = new Random();
            
            for (int i = 0; i < 5; i++)
            {
                int asteroidSize = rnd.Next(10, 30);
                _gameManager.AddGameObject(new Asteroid(asteroidSize));
            }

            _gameManager.AddGameObject(new Supply());
            _gameManager.AddGameObject(new SupplyCapybaraGun());

            hud = new HUD(player);
            hud.Load(Content);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameManager.Load(Content);
            _startScreen.LoadContent(GraphicsDevice, Content);
            _pauseScreen.LoadContent(GraphicsDevice, Content);
            _gameOverScreen.LoadContent(GraphicsDevice, Content);

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _gameManager.SetGameState(GameState.Paused);

            hud.Update(gameTime);

            switch (_gameManager.GetCurrentGameState())
            {
                case GameState.StartScreen:
                    _startScreen.Update(_gameManager);
                    break;
                case GameState.Playing:

                    if(Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        _gameManager.SetGameState(GameState.Paused);
                    }

                    _gameManager.Update(gameTime);
                    break;
                case GameState.Paused:
                    _pauseScreen.Update(_gameManager);
                    break;
                case GameState.GameOver:
                    _gameOverScreen.Update(_gameManager);
                    break;
                default:
                    break;
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _background,
                new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                Color.White
            );

            //base.Draw(gameTime, spriteBatch);

            GraphicsDevice.Clear(Color.Black);

            hud.Draw(_spriteBatch);

            switch (_gameManager.CurrentState)
            {
                case GameState.StartScreen:
                    _startScreen.Draw(_spriteBatch);
                    break;
                case GameState.Playing:
                    _gameManager.Draw(gameTime, _spriteBatch);
                    break;
                case GameState.Paused:
                    _gameManager.Draw(gameTime, _spriteBatch);
                    _pauseScreen.Draw(_spriteBatch, _graphics.GraphicsDevice);
                    break;
                case GameState.GameOver:
                    _gameManager.Draw(gameTime, _spriteBatch);
                    _gameOverScreen.Draw(_spriteBatch, _graphics.GraphicsDevice);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

    }
}
