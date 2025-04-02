using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    class CapybaraGun : GameObject
    {
        private Texture2D _texture;
        private CircleCollider _circleCollider;
        private Vector2 _velocity;
        public float capiSize = 80;

        public CapybaraGun(Vector2 location, Vector2 direction, float speed)
        {
            _circleCollider = new CircleCollider(location, capiSize);
            SetCollider(_circleCollider);
            _velocity = direction * speed;
        }

        public override void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>("capybaraBullet");
            base.Load(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _circleCollider.Center += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!GameManager.GetGameManager().Game.GraphicsDevice.Viewport.Bounds.Contains(_circleCollider.Center))
                GameManager.GetGameManager().RemoveGameObject(this);

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Alien || other is Supply)
            {
                GameManager.GetGameManager().RemoveGameObject(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
