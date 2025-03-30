using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Engine;
using SpaceDefence.Screens;
using System;
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


        public SpaceDefence()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            // set the size of the screen
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 980;
            
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

            // place the player at the center of the screen
            Ship player = new Ship(new Point(GraphicsDevice.Viewport.Width/2,GraphicsDevice.Viewport.Height/2));

            // add the starting objects to the GameManager
            _gameManager.Initialize(Content, this, player);
            _gameManager.AddGameObject(player);
            _gameManager.AddGameObject(new Alien());

            // set the game state to the start screen   
            _gameManager.SetGameState(GameState.StartScreen);


            Random rnd = new Random();
            
            for (int i = 0; i < 5; i++)
            {
                int asteroidSize = rnd.Next(10, 30);
                _gameManager.AddGameObject(new Asteroid(asteroidSize));
            }

            _gameManager.AddGameObject(new Supply());
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
            GraphicsDevice.Clear(Color.Black);

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


            base.Draw(gameTime);
        }

    }
}
