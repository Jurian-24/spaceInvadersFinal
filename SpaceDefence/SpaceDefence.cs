using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Engine;
using SpaceDefence.Screens;
namespace SpaceDefence
{
    public class SpaceDefence : Game
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        // start screen stuff bruv
        private GameState _currentState;
        private StartScreen _startScreen;

        // pause screen stuff bruv
        private PauseScreen _pauseScreen;


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

            _currentState = GameState.StartScreen;
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
            _gameManager.AddGameObject(new Supply());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameManager.Load(Content);
            _startScreen.LoadContent(GraphicsDevice, Content);
            _pauseScreen.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _currentState = GameState.Paused;

            switch (_currentState)
            {
                case GameState.StartScreen:
                    _startScreen.Update(this);
                    break;
                case GameState.Playing:

                    if(Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        _currentState = GameState.Paused;
                    }

                    _gameManager.Update(gameTime);
                    break;
                case GameState.Paused:
                    _pauseScreen.Update(this);
                    break;
                default:
                    break;
            }

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (_currentState)
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
                default:
                    break;
            }


            base.Draw(gameTime);
        }

        public void ChangeState(GameState newState)
        {
            _currentState = newState;
        }

    }
}
