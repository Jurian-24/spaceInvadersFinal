using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceDefence.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    internal class SupplyCapybaraGun : GameObject
    {
        private RectangleCollider _rectangleCollider;
        private Texture2D _texture;
        private float playerClearance = 100;

        public SupplyCapybaraGun()
        {

        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("CapybaraGun");
            _rectangleCollider = new RectangleCollider(new Rectangle(0, 0, 64, 64));

            SetCollider(_rectangleCollider);
            RandomMove();
        }

        public override void OnCollision(GameObject other)
        {
            RandomMove();
            GameManager.GetGameManager().Player.Buff();
            GameManager.GetGameManager().Player.buffType = "capybaraGun";
            base.OnCollision(other);
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            _rectangleCollider.shape.Location = (gm.RandomScreenLocation() - _rectangleCollider.shape.Size.ToVector2() / 2).ToPoint();

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_rectangleCollider.shape.Center.ToVector2() - centerOfPlayer).Length() < playerClearance)
                _rectangleCollider.shape.Location = gm.RandomScreenLocation().ToPoint();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture, 
                _rectangleCollider.shape, 
                Color.White
            );

            base.Draw(gameTime, spriteBatch);
        }


    }
}
