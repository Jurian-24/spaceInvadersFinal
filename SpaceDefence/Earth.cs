using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    class Earth : GameObject
    {
        public const int maxCargo = 10; // end score is max 10

        public List<Oppy> oppiesAvailable = new List<Oppy>();

        private CircleCollider _circleCollider;
        private Texture2D _texture;

        public Earth() {
            
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("earth");

            GameManager gm = GameManager.GetGameManager();
            int screenWidth = gm.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = gm.Game.GraphicsDevice.Viewport.Height;

            float colliderRadius = (_texture.Width / 2);
            _circleCollider = new CircleCollider(Vector2.Zero, colliderRadius);

            //randomize the angle (hoek geen idee wat het juiste woord is, HelpContextType is 2:25 am nu bro ik wil slapen) of the screen
            Random random = new Random();
            int corner = random.Next(4);

            Vector2 spawnPosition = gm.RandomScreenLocation();

            _circleCollider.Center = spawnPosition;

            this.spawnCargo();

            SetCollider(_circleCollider);
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);
            
            if (other is Ship)
            {
                GameManager gm = GameManager.GetGameManager();

                if(!gm.Player.isCarryingCargo && gm.Player.cargoPoints < 10)
                {
                    gm.Player.loadOppyOnShip(oppiesAvailable[0]);
                    oppiesAvailable.RemoveAt(0);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _circleCollider.Center,
                Color.White            
            );

            base.Draw(gameTime, spriteBatch);
        }

        private void spawnCargo()
        {
            for (int i = 0; i < maxCargo; i++)
            {
                Point position = new Point(100 + (i * 40), 500);
                oppiesAvailable.Add(new Oppy(
                    i,
                    position
                ));
            }
        }

    }
}
