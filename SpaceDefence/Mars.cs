using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace SpaceDefence
{
    class Mars : GameObject
    {
        public List<Oppy> cargo = new List<Oppy>();

        private CircleCollider _circleCollider;
        private Texture2D _texture;

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("mars");

            GameManager gm = GameManager.GetGameManager();
            int screenWidth = gm.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = gm.Game.GraphicsDevice.Viewport.Height;

            float colliderRadius = (_texture.Width / 2);
            _circleCollider = new CircleCollider(Vector2.Zero, colliderRadius);

            //randomize the angle (hoek geen idee wat het juiste woord is, HelpContextType is 2:25 AM nu bro ik wil slapen) of the screen. uPDATE: het is nu 4:00 AM maar ik ben eindelijk klaar
            Random random = new Random();
            int corner = random.Next(4);

            Vector2 spawnPosition = gm.RandomScreenLocation();

            _circleCollider.Center = spawnPosition;

            SetCollider(_circleCollider);
        }
        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (other is Ship)
            {
                GameManager gm = GameManager.GetGameManager();
                Ship player = gm.Player;

                if (player.isCarryingCargo)
                {
                    cargo.Add(gm.Player.cargoOppy);
                    gm.Player.cargoPoints = cargo.Count;
                    gm.Player.cargoOppy = null;
                    gm.Player.isCarryingCargo = false;
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
    }
}
