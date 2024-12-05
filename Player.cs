using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NKFinalProjectSem3
{
    public class Player
    {

        private Texture2D _texture;
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Player(Vector2 position)
        {

            Position = position;

        }

        public void SetTexture(Texture2D texture)
        {

            _texture = texture;

        }

        public void Update(KeyboardState keyboard, Rectangle viewport)
        {

            float speed = 5f;
            if (keyboard.IsKeyDown(Keys.Up)) Position.Y -= speed;
            if (keyboard.IsKeyDown(Keys.Down)) Position.Y += speed;
            if (keyboard.IsKeyDown(Keys.Left)) Position.X -= speed;
            if (keyboard.IsKeyDown(Keys.Right)) Position.X += speed;

            Position.X = MathHelper.Clamp(Position.X, 0, viewport.Width - _texture.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, viewport.Height - _texture.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(_texture, Position, Color.White);

        }

    }

}
