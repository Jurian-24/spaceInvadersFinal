
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Engine;
using System;

namespace SpaceDefence.Screens
{
    class StartScreen
    {
        private SpriteFont font;

        private string startMessage = "Druk op de spacebar/spatiebalk om te starten!";
        private Vector2 positionStartMessage;
        private Vector2 startMessageSize;

        private string quitMessage = "Druk op F1 om af te sluiten";
        private Vector2 quitMessagePos;
        private Vector2 quitMessageSize;

        public StartScreen()
        {
            positionStartMessage = new Vector2(0, 0);
            quitMessagePos = new Vector2(0, 0);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            font = content.Load<SpriteFont>("Font");

            startMessageSize = font.MeasureString(startMessage);
            quitMessageSize = font.MeasureString(quitMessage);

            positionStartMessage = new Vector2(
                 (graphicsDevice.Viewport.Width - startMessageSize.X) / 2,
                 graphicsDevice.Viewport.Height / 2 - startMessageSize.Y / 2 - 40 // -40 voor extra padding boven
             );

            quitMessagePos = new Vector2(
                (graphicsDevice.Viewport.Width - quitMessageSize.X) / 2,
                positionStartMessage.Y + startMessageSize.Y + 20 // 20px padding tussen de teksten
            );
        }

        public void Update(GameManager gm)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                gm.SetGameState(GameState.Playing);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                // TODO spel afluisten
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.DrawString(font, startMessage, positionStartMessage, Color.White);
            spriteBatch.DrawString(font, quitMessage, quitMessagePos, Color.White);


            //spriteBatch.End();
        }

    }
}
