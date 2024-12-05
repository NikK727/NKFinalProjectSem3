using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NKFinalProjectSem3
{
    public class Enemy
    {

        private Texture2D _texture;
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        private int _speed;

        public Enemy(Vector2 position, Texture2D texture, int speed)
        {

            Position = position;
            _texture = texture;
            _speed = speed;

        }

        public void Update()
        {
            Position.X -= _speed;

            if (Position.X < 0)
            {

                Position.X = 800;
                Position.Y = new Random().Next(0, 600);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, Position, Color.White);

        }

    }

}
