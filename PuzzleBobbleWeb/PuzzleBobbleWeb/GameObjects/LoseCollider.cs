using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class LoseCollider : GameObject
    {
        public LoseCollider(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name.Equals("NormalBobble") && g.IsActive && IsTriggered(g) && g.Velocity == Vector2.Zero)
                {
                    Singleton.Instance.currentPlayerStatus = Singleton.PlayerStatus.Lost;
                    Singleton.Instance.currentGameState = Singleton.GameSceneState.End;
                }
            }
            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = true;
            base.Reset();
        }
    }
}
