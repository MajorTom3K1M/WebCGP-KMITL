using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    public class Paddle : GameObject
    {
        public Keys Left;
        public Keys Right;
        public float Speed;
        public static Vector2 PaddlePos;

        public Paddle(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            
            if (Singleton.Instance.CurrentKey.IsKeyDown(Left)) Velocity.X = -Speed;
            else if (Singleton.Instance.CurrentKey.IsKeyDown(Right)) Velocity.X = Speed;

            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            Position.X = MathHelper.Clamp(Position.X, 0, Singleton.WIDTH * Singleton.SIZE - _texture.Width);

            Velocity = Vector2.Zero;
            

            PaddlePos = Position;


            ////Autoplay
            //if (Singleton.Instance.currentGameState == Singleton.GameState.Playing)
            //{
            //    Position.X = Ball.BallPos.X - 10;
            //    Position.X = MathHelper.Clamp(Position.X, 0, Singleton.WIDTH * Singleton.SIZE - _texture.Width);
            //}


            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Speed = 500f;
            base.Reset();
        }
    }
}
