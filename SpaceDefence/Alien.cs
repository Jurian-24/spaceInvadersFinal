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

        private int speedLevel;

        public Alien() 
        {
            speedLevel = 1;
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
            if (other is Bullet || other is Laser || other is CapybaraGun)
            {
                this.speedLevel++;
                DestroyAlien();
                RandomMove();
            }

            if (other is Supply)
            {
                return;
            }

            if(other is Ship)
            {
                GameManager.GetGameManager().SetGameState(Engine.GameState.GameOver);
            }

            base.OnCollision(other);
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            int screenWidth = gm.Game.GraphicsDevice.Viewport.Width;
            int screenHeight = gm.Game.GraphicsDevice.Viewport.Height;

            Random random = new Random();
            Vector2 spawnPosition = Vector2.Zero;

            // Kies een willekeurige zijde buiten het scherm (0 = links, 1 = rechts, 2 = boven, 3 = onder)
            int side = random.Next(4);

            switch (side)
            {
                case 0: // Links van het scherm
                    spawnPosition = new Vector2(-_circleCollider.Radius * 2, random.Next(0, screenHeight));
                    break;
                case 1: // Rechts van het scherm
                    spawnPosition = new Vector2(screenWidth + _circleCollider.Radius * 2, random.Next(0, screenHeight));
                    break;
                case 2: // Boven het scherm
                    spawnPosition = new Vector2(random.Next(0, screenWidth), -_circleCollider.Radius * 2);
                    break;
                case 3: // Onder het scherm
                    spawnPosition = new Vector2(random.Next(0, screenWidth), screenHeight + _circleCollider.Radius * 2);
                    break;
            }

            _circleCollider.Center = spawnPosition;

            // Controleer of de alien te dicht bij de speler spawnt, anders opnieuw spawnen
            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
            {
                // Kies opnieuw een spawnlocatie
                side = random.Next(4);
                switch (side)
                {
                    case 0: spawnPosition = new Vector2(-_circleCollider.Radius * 2, random.Next(0, screenHeight)); break;
                    case 1: spawnPosition = new Vector2(screenWidth + _circleCollider.Radius * 2, random.Next(0, screenHeight)); break;
                    case 2: spawnPosition = new Vector2(random.Next(0, screenWidth), -_circleCollider.Radius * 2); break;
                    case 3: spawnPosition = new Vector2(random.Next(0, screenWidth), screenHeight + _circleCollider.Radius * 2); break;
                }
                _circleCollider.Center = spawnPosition;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Level.level; i++)
            {
                spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
                
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            GameManager gm = GameManager.GetGameManager();

            Vector2 direction = gm.Player.GetPosition().Center.ToVector2() - this._circleCollider.Center;

            if (direction != Vector2.Zero)
            {
                direction.Normalize(); 
            }

            float speed = 1.5f + (speedLevel * 0.3f); // adjust the speed based on the level of the alien
            this._circleCollider.Center += direction * speed;

        }

        public void DestroyAlien()
        {
            GameManager.GetGameManager().RemoveGameObject(this);
            if(this.speedLevel % 3 == 0)
            {
                Level.IncreaseLevel();
            }
        }

    }
}
