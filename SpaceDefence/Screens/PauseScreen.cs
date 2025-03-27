using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefence.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence.Screens
{
    class PauseScreen
    {

        private SpriteFont font;

        private string pauseMessage = "Pauze - Druk op Space om door te gaan";
        private Vector2 positionPausemessage;
        private Vector2 sizePauseMessage;

        public PauseScreen()
        {
            positionPausemessage = new Vector2(0, 0);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            font = content.Load<SpriteFont>("Font");
            sizePauseMessage = font.MeasureString(pauseMessage);

            positionPausemessage = new Vector2(
                (graphicsDevice.Viewport.Width - sizePauseMessage.X) / 2,
                graphicsDevice.Viewport.Height / 2 - sizePauseMessage.Y / 2
            );
        }

        public void Update(SpaceDefence game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.ChangeState(GameState.Playing);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();

            // Semi-transparante achtergrond overlay
            Texture2D overlay = new Texture2D(graphicsDevice, 1, 1);

            // opacity toevoegen aan het pauze scherm
            overlay.SetData(new[] { new Color(0, 0, 0, 150) });

            // full screen maken
            spriteBatch.Draw(overlay,
                     new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height),
                     Color.White);

            spriteBatch.DrawString(font, pauseMessage, positionPausemessage, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
