using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceDefence.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    internal class TravisScottBatman : GameObject
    {
        private RectangleCollider _rectangleCollider;
        private Texture2D _texture;
        private float speed = 2.0f;      // Basis snelheid richting speler
        private float zigzagAmplitude = 50f; // Hoeveelheid zigzag
        private float zigzagFrequency = 6.0f; // Hoe snel hij zigzagt
        private float timeElapsed = 0f;  // Tijd voor sinusberekening

        public TravisScottBatman()
        {

        }
        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("travisScottBatman");
            _rectangleCollider = new RectangleCollider(new Rectangle(0, 0, 64, 64));

            SetCollider(_rectangleCollider);
            SetRandomStartPosition();
        }

        // source zigzagging: https://medium.com/nerd-for-tech/zig-zag-movement-in-unity-3c2762b1be61
        public override void Update(GameTime gameTime)
        {
            GameManager gm = GameManager.GetGameManager();
            Vector2 playerPosition = gm.Player.GetPosition().Center.ToVector2();
            Vector2 direction = playerPosition - _rectangleCollider.shape.Center.ToVector2();

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float zigzagOffset = MathF.Sin(timeElapsed * zigzagFrequency) * zigzagAmplitude;

            Vector2 movement = new Vector2(direction.X * speed, direction.Y * speed + zigzagOffset * 0.1f);

            _rectangleCollider.shape.Location += movement.ToPoint();
        }


        public override void OnCollision(GameObject other)
        {
            if (other is Asteroid || other is Supply || other is Alien)
            {
                return;
            }

            if (other is Ship)
            {
                GameManager.GetGameManager().SetGameState(Engine.GameState.GameOver);
            }

            // alleen gort kan travis aan
            if (other is CapybaraGun)
            {
                GameManager.GetGameManager().RemoveGameObject(this);
            }

            base.OnCollision(other);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangleCollider.shape, Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        private void SetRandomStartPosition()
        {
            GameManager gm = GameManager.GetGameManager();
            _rectangleCollider.shape.Location = gm.RandomScreenLocation().ToPoint();
        }
    }
}
