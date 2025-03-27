using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    public class Camera : GameObject
    {
        public Matrix worldTransform { get; private set; }
        public Vector2 position { get; private set; }

        private Viewport _viewport;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            position = Vector2.Zero;
        }

        public void followPlayer(Ship player)
        {
            position = new Vector2(
                player.GetPosition().X - (_viewport.Width / 2),
                player.GetPosition().Y - (_viewport.Height / 2)
            );

            worldTransform = Matrix.CreateTranslation(new Vector3(-position, 0f));
        }

        public Rectangle GetWorldBounds()
        {
            return new Rectangle(
                (int)(position.X - _viewport.Width / 2),
                (int)(position.Y - _viewport.Height / 2),
                _viewport.Width,
                _viewport.Height
            );
        }

    }
}
