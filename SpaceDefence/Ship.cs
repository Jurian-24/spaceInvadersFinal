using System;
using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class Ship : GameObject
    {
        private Texture2D ship_body;
        private Texture2D base_turret;
        private Texture2D laser_turret;
        private Texture2D capybaraGun;
        private float buffTimer = 0;
        private float buffDuration = 10f;
        private RectangleCollider _rectangleCollider;
        private Point target;

        private Vector2 velocity;
        private float rotation;
        private const float FRICTION = 0.99f;
        private const float acceleration = 0.1f;

        public string buffType = "laser";

        private float capybaraGunCooldown = 0f;
        private const float CAPYBARA_GUN_COOLDOWN_TIME = 2f;

        private Vector2 facingDirection;

        /// <summary>
        /// The player character
        /// </summary>
        /// <param name="Position">The ship's starting position</param>
        public Ship(Point Position)
        {
            _rectangleCollider = new RectangleCollider(new Rectangle(Position, Point.Zero));
            SetCollider(_rectangleCollider);
            velocity = Vector2.Zero;
            facingDirection = Vector2.Zero;
            rotation = 0;
        }

        public override void Load(ContentManager content)
        {
            // Ship sprites from: https://zintoki.itch.io/space-breaker
            ship_body = content.Load<Texture2D>("ship_body");
            base_turret = content.Load<Texture2D>("base_turret");
            laser_turret = content.Load<Texture2D>("laser_turret");
            capybaraGun = content.Load<Texture2D>("capybaraGun");
            _rectangleCollider.shape.Size = ship_body.Bounds.Size;
            _rectangleCollider.shape.Location -= new Point(ship_body.Width/2, ship_body.Height/2);
            base.Load(content);
        }



        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            target = inputManager.CurrentMouseState.Position;

            if (capybaraGunCooldown > 0)
            {
                capybaraGunCooldown -= (float)GameManager.GetGameManager().Game.TargetElapsedTime.TotalSeconds;
            }

            if (inputManager.LeftMousePress())
            {

                Vector2 aimDirection = LinePieceCollider.GetDirection(GetPosition().Center, target);
                Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + aimDirection * base_turret.Height / 2f;
                if (buffTimer <= 0)
                {
                    GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, aimDirection, 150));
                }
                else if (buffTimer > 0 && buffType == "laser")
                {
                    GameManager.GetGameManager().AddGameObject(new Laser(new LinePieceCollider(turretExit, target.ToVector2()),400));
                }
                else if (buffTimer > 0 && buffType == "capybaraGun" && capybaraGunCooldown <= 0)
                {
                    GameManager.GetGameManager().AddGameObject(new CapybaraGun(turretExit, new Vector2(aimDirection.X - 100, aimDirection.Y - 100), 150));
                    GameManager.GetGameManager().AddGameObject(new CapybaraGun(turretExit, aimDirection, 250));
                    GameManager.GetGameManager().AddGameObject(new CapybaraGun(turretExit, new Vector2(aimDirection.X + 100, aimDirection.Y + 100), 350));
                    
                    capybaraGunCooldown = CAPYBARA_GUN_COOLDOWN_TIME;
                }
            }

            // check for diagonal speed so that everything moves equally
            CheckMovement(inputManager);

            velocity *= FRICTION;

            _rectangleCollider.shape.Location += velocity.ToPoint();
        }

        public override void Update(GameTime gameTime)
        {
            

            // Update the Buff timer
            if (buffTimer > 0)
                buffTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                ship_body,
                _rectangleCollider.shape.Center.ToVector2(), 
                null,                                        
                Color.White,                                 
                rotation,                                    
                new Vector2(ship_body.Width / 2f, ship_body.Height / 2f), 
                1.0f,                                        
                SpriteEffects.None,                          
                0f                                            
            );

            float aimAngle = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(GetPosition().Center, target));

            if (buffTimer <= 0)
            {
                Rectangle turretLocation = base_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(base_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else if(buffTimer > 0 && buffType == "laser")
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(laser_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else if(buffTimer > 0 && buffType == "capybaraGun")
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(capybaraGun, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }

            base.Draw(gameTime, spriteBatch);
        }


        public void Buff()
        {
            buffTimer = buffDuration;
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }

        private void CheckMovement(InputManager inputManager)
        {
            checkIfPlayerIsOutOfBounds();
            Vector2 direction = Vector2.Zero;

            if (inputManager.IsKeyDown(Keys.W))
            {
                direction.Y -= 1;
                facingDirection = new Vector2(1, 0);
            }
            ;
            if (inputManager.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
                facingDirection = new Vector2(-1, 0);
            }
            if (inputManager.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
                facingDirection = new Vector2(0, -1);
            }
            if (inputManager.IsKeyDown(Keys.D))
            {
                direction.X += 1;
                facingDirection = new Vector2(0, 1);
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                velocity += direction * acceleration;
            }

            rotation = MathF.Atan2(facingDirection.Y, facingDirection.X);
            //velocity += facingDirection * acceleration;

            
        }

        private void checkIfPlayerIsOutOfBounds()
        {
            if (_rectangleCollider.shape.X <= 0)
            {
                _rectangleCollider.shape.X = GameManager.GetGameManager().Game.GraphicsDevice.Viewport.Width - 10;
            }
            if (_rectangleCollider.shape.X > GameManager.GetGameManager().Game.GraphicsDevice.Viewport.Width)
            {
                _rectangleCollider.shape.X = 0;
            }
            if (_rectangleCollider.shape.Y <= 0)
            {
                _rectangleCollider.shape.Y = GameManager.GetGameManager().Game.GraphicsDevice.Viewport.Height - 100;
            }
            if (_rectangleCollider.shape.Y > GameManager.GetGameManager().Game.GraphicsDevice.Viewport.Height)
            {
                _rectangleCollider.shape.Y = 0;
            }
        }
    }
}
