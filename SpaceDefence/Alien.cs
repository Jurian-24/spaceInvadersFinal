using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceDefence.Collision;
using System;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceDefence
{
    internal class Alien : GameObject
    {
        private CircleCollider _circleCollider;
        private Texture2D _texture;
        private float playerClearance = 100;

        private int level;

        public Alien() 
        {
            level = 1;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("Alien");
            _circleCollider = new CircleCollider(Vector2.Zero, _texture.Width / 2);
            SetCollider(_circleCollider);
            RandomMove();
        }

        public override void OnCollision(GameObject other)
        {
            System.Console.WriteLine("Alien collided with " + other.GetType());

            if (other is Supply || other is Asteroid)
            {
                return;
            }

            RandomMove();

            this.level++;
            base.OnCollision(other);
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            _circleCollider.Center = gm.RandomScreenLocation();

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
                _circleCollider.Center = gm.RandomScreenLocation();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            GameManager gm = GameManager.GetGameManager();

            Vector2 direction = gm.Player.GetPosition().Center.ToVector2() - this._circleCollider.Center;

                        

            if (direction != Vector2.Zero)
            {
                direction.Normalize(); 
            }

            float speed = 1.5f + (level * 0.3f); // adjust the speed based on the level of the alien
            this._circleCollider.Center += direction * speed;

        }

    }
}
