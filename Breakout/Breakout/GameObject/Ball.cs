using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    public class Ball : GameObject
    {
        public float Speed, Angle;
        public static Vector2 BallPos;

        public Ball(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            BallPos = this.Position;

            switch (Singleton.Instance.currentGameState)
            {
                case Singleton.GameState.Start:
                    {
                        Position.X = Paddle.PaddlePos.X + 100 - Rectangle.Width / 2;
                        Position.Y = Paddle.PaddlePos.Y - _texture.Height;
                        if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                        {
                            Angle = MathHelper.ToRadians(-90);
                        }
                        break;
                    }
                case Singleton.GameState.Playing:
                    {
                        Velocity.X = (float)Math.Cos(Angle) * Speed;
                        Velocity.Y = (float)Math.Sin(Angle) * Speed;

                        Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                        foreach (GameObject g in gameObjects)
                        {
                            //paddle
                            if (g.Name.Equals("Paddle") && this.IsTouchingTop(g))
                            {
                                //change angle according to where the ball hits the paddle
                                Rectangle paddleRect = g.Rectangle;
                                int collidedPointSegment = (int)((Position.X + Rectangle.Width / 2 - paddleRect.X) / paddleRect.Width * 8);
                                switch (collidedPointSegment)
                                {
                                    case 0:
                                        Angle = MathHelper.ToRadians(225);
                                        break;
                                    case 1:
                                        Angle = MathHelper.ToRadians(240);
                                        break;
                                    case 2:
                                        Angle = MathHelper.ToRadians(255);
                                        break;
                                    case 3:
                                    case 4:
                                        Angle = MathHelper.ToRadians(270);
                                        break;
                                    case 5:
                                        Angle = MathHelper.ToRadians(285);
                                        break;
                                    case 6:
                                        Angle = MathHelper.ToRadians(300);
                                        break;
                                    case 7:
                                        Angle = MathHelper.ToRadians(315);
                                        break;
                                }
                            }

                            //Make a collision with brick
                            if (g.Name.Equals("Brick") && g.IsActive)
                            {
                                if (this.IsTouchingTop(g))
                                {
                                    g.IsActive = false;
                                    Angle = -Angle;
                                    Singleton.Instance.brickCount--;
                                }
                                else if (this.IsTouchingLeft(g) || this.IsTouchingRight(g))
                                {
                                    g.IsActive = false;
                                    if (Angle > MathHelper.ToRadians(270)) Angle = MathHelper.ToRadians(270) - (Angle - MathHelper.ToRadians(270));
                                    else if (Angle < MathHelper.ToRadians(270) && Angle > MathHelper.ToRadians(180)) Angle = MathHelper.ToRadians(360) - (Angle - MathHelper.ToRadians(180));
                                    else if (Angle < MathHelper.ToRadians(180) && Angle > MathHelper.ToRadians(90)) Angle = MathHelper.ToRadians(90) - (Angle - MathHelper.ToRadians(90));
                                    else if (Angle < MathHelper.ToRadians(90)) Angle = MathHelper.ToRadians(180) - Angle;
                                    Singleton.Instance.brickCount--;
                                }
                                else if (this.IsTouchingBottom(g))
                                {
                                    g.IsActive = false;
                                    if (Angle >= MathHelper.ToRadians(270)) Angle = MathHelper.ToRadians(90) - (Angle - MathHelper.ToRadians(270));
                                    else if (Angle < MathHelper.ToRadians(270) && Angle > MathHelper.ToRadians(180)) Angle = MathHelper.ToRadians(180) - (Angle - MathHelper.ToRadians(180));
                                    Singleton.Instance.brickCount--;
                                }
                            }
                        }

                        if (Position.Y <= 0)
                        {
                            Angle = -Angle;
                        }

                        if (Position.X <= 0 || Position.X + _texture.Width >= Singleton.WIDTH * Singleton.SIZE)
                        {
                            if (Angle > MathHelper.ToRadians(270)) Angle = MathHelper.ToRadians(270) - (Angle - MathHelper.ToRadians(270));
                            else if (Angle < MathHelper.ToRadians(270) && Angle > MathHelper.ToRadians(180)) Angle = MathHelper.ToRadians(360) - (Angle - MathHelper.ToRadians(180));
                            else if (Angle < MathHelper.ToRadians(180) && Angle > MathHelper.ToRadians(90)) Angle = MathHelper.ToRadians(90) - (Angle - MathHelper.ToRadians(90));
                            else if (Angle < MathHelper.ToRadians(90)) Angle = MathHelper.ToRadians(180) - Angle;
                            else Angle = -Angle;
                        }

                        if (Position.Y >= Singleton.HEIGHT * Singleton.SIZE)
                        {
                            Reset();
                            Singleton.Instance.Life--;
                            Singleton.Instance.currentGameState = Singleton.GameState.Start;
                        }

                        break;
                    }
                case Singleton.GameState.End:
                    {
                        break;
                    }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Speed = 600f;
            Position.X = Paddle.PaddlePos.X + 100 - Rectangle.Width / 2;

            base.Reset();
        }
    }
}
