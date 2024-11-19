using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class Window : GameObject
    {
        public Window(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = false;
            base.Reset();
        }
    }
}
