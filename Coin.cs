using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NKFinalProjectSem3
{
    public class Coin
    {

        private Texture2D _texture;
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        private int _speed = 3;

        public Coin(Vector2 position, Texture2D texture)
        {

            Position = position;
            _texture = texture;

        }

        public void Update()
        {

            Position.X -= _speed;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, Position, Color.Yellow);

        }

    }

}
