using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardiousWeb
{
    public class LevelChunk
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; } // World position
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public LevelChunk(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
