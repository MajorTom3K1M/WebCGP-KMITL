using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class ScoreDisplayer : GameObject
    {
        private string scoreText;
        public ScoreDisplayer(Texture2D texture) : base(texture)
        {
            scoreText = "000000";
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.score > 0)
            {
                scoreText = "";
                int score = Singleton.Instance.score;
                int zeroToken = 6;

                while (score > 0)
                {
                    score /= 10;
                    zeroToken--;
                }

                for (int i = zeroToken; i > 0; --i) scoreText += "0";

                scoreText += Singleton.Instance.score.ToString();
            }

            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Singleton.Instance.gameFont, scoreText, Position, Color.Black, 0f, new Vector2(-20, -5), 1f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = true;
            base.Reset();
        }
    }
}
