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
    public class Oppy : GameObject
    {
        public int id;
        private Texture2D texture;
        private RectangleCollider _rectangleCollider;

        public Oppy(int id, Point position)
        {
            this.id = id;
            _rectangleCollider = new RectangleCollider(new Rectangle(position, new Point(64, 64))); 
            SetCollider(_rectangleCollider);
        }

        public override void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("oppyRoverCargo");
            _rectangleCollider.shape.Size = new Point(64, 64);
            base.Load(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameManager gm = GameManager.GetGameManager();

            if (gm.Player.isCarryingCargo)
            {
                // geen tijd meer lmaooo
                //spriteBatch.Draw(texture, _rectangleCollider.shape.Location.ToVector2(), Color.White);
            }
        }

        public void SetPosition(Vector2 position)
        {
            _rectangleCollider.shape.Location = new Point((int)position.X, (int)position.Y);
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }
    }
}
