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
    internal class Asteroid : GameObject
    {
        private CircleCollider _circleCollider;
        private Texture2D _texture;
        private float playerClearance = 100;

        private int _size;
        private float _scale;

        public Asteroid(int size)
        {
            _size = size;
            _scale = size / 100f;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("asteroid");
            
            float colliderRadius = (_texture.Width / 2) * _scale;
            _circleCollider = new CircleCollider(Vector2.Zero, colliderRadius);

            SetCollider(_circleCollider);
            RandomMove();
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Ship)
            {
                GameManager.GetGameManager().GameOver();
                return;
            }

            base.OnCollision(other);
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            Vector2 newPosition = gm.RandomScreenLocation();

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();

            while ((newPosition - centerOfPlayer).Length() < playerClearance)
            {
                newPosition = gm.RandomScreenLocation();
            }

            _circleCollider.Center = newPosition;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _circleCollider.Center,            
                null,                
                Color.White,         
                0f,                  
                new Vector2(_texture.Width / 2, _texture.Height / 2), 
                _scale,              
                SpriteEffects.None,  
                0f                   
            );
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
        }

    }
}
