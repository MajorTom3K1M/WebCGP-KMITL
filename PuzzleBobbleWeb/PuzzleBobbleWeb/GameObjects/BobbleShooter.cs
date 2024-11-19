using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class BobbleShooter : GameObject
    {
        public float Speed;
        public float Angle;
        public double mouseAngle;
        public Texture2D body, insideBody;

        NormalBobble bobble_primary, bobble_secondary;
        BombBobble bomb;
        Vector2 mousePosition;
        MouseState mouseClickedState, previousMouseState, mouseState;

        bool IsBomb;
        double cur;

        Queue<GameObject> q = new Queue<GameObject>();
        Queue<GameObject> bobbleQ = new Queue<GameObject>();

        private float timer;

        enum shooterState
        {
            shooterReload,
            shooterReady
        }

        private shooterState currentShooterState;

        Texture2D[] color = { PuzzleBobbleWebGame.bobble_red, PuzzleBobbleWebGame.bobble_blue, PuzzleBobbleWebGame.bobble_green, PuzzleBobbleWebGame.bobble_yellow, PuzzleBobbleWebGame.bobble_white, PuzzleBobbleWebGame.bobble_turquoise, PuzzleBobbleWebGame.bobble_purple, PuzzleBobbleWebGame.bobble_orange };
        NormalBobble.BobbleColor[] normalBobbleColor = { NormalBobble.BobbleColor.Red, NormalBobble.BobbleColor.Blue,
            NormalBobble.BobbleColor.Green, NormalBobble.BobbleColor.Yellow, NormalBobble.BobbleColor.Purple, NormalBobble.BobbleColor.Orange, NormalBobble.BobbleColor.White, NormalBobble.BobbleColor.Turquoise };

        public BobbleShooter(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.currentPlayerStatus == Singleton.PlayerStatus.None)
            {
                previousMouseState = mouseClickedState;
                mouseState = Mouse.GetState();
                mousePosition = Singleton.MousePosition;
                mouseAngle = MathHelper.ToDegrees((float)Math.Atan2(((Singleton.MAINSCREEN_HEIGHT - 20) - mousePosition.Y), (float)(mousePosition.X - (Singleton.MAINSCREEN_WIDTH / 2))));

                if (mouseAngle < 0) mouseAngle = 180 + (180 + mouseAngle);

                var lbound = 8;
                var ubound = 172;

                if (mouseAngle > 90 && mouseAngle < 270 && mouseAngle > ubound) mouseAngle = ubound;
                else if (mouseAngle < lbound || mouseAngle >= 270) mouseAngle = lbound;

                switch (currentShooterState)
                {
                    case shooterState.shooterReload:
                        if (bobble_primary != null && !IsBomb)
                        {
                            foreach (GameObject g in gameObjects)
                            {
                                if (bobble_primary.circleCollide(g) && !g.Equals(bobble_primary) && g.Name.Equals("NormalBobble"))
                                {
                                    bobble_primary = bobble_secondary.Clone() as NormalBobble;
                                    bobble_primary.Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 25, Singleton.MAINSCREEN_HEIGHT - 75);
                                }
                            }
                        }
                        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (timer > 1)
                        {
                            if (Singleton.Instance.turnCounter >= Singleton.Instance.bombTime)
                            {
                                IsBomb = true;
                                bomb = new BombBobble(PuzzleBobbleWebGame.bombbobble)
                                {
                                    Name = "BombBobble",
                                    Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 25, Singleton.MAINSCREEN_HEIGHT - 75),
                                    SoundEffects = new Dictionary<string, SoundEffectInstance>()
                                    {
                                        {"Burst", PuzzleBobbleWebGame.burst }
                                    }
                                };
                                Singleton.Instance.turnCounter = 0;
                                gameObjects.Add(bomb);
                                bobble_primary = null;
                            }
                            else
                            {
                                Random rand = new Random();
                                int rnd = rand.Next(0, Singleton.Instance.colorVariety);
                                if (bobble_primary == null)
                                {
                                    bobble_primary = new NormalBobble(color[rnd])
                                    {
                                        Name = "NormalBobble",
                                        bobbleColor = normalBobbleColor[rnd],
                                        Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 25, Singleton.MAINSCREEN_HEIGHT - 75)
                                    };
                                }

                                gameObjects.Add(bobble_primary);

                                //Add secondary bobble
                                rnd = rand.Next(0, Singleton.Instance.colorVariety);
                                bobble_secondary = new NormalBobble(color[rnd])
                                {
                                    Name = "NormalBobble",
                                    bobbleColor = normalBobbleColor[rnd],
                                    Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 150, Singleton.MAINSCREEN_HEIGHT - 75)
                                };

                                gameObjects.Add(bobble_secondary);
                            }

                            timer = 0;

                            currentShooterState = shooterState.shooterReady;
                        }
                        break;
                    case shooterState.shooterReady:
                        previousMouseState = mouseClickedState;
                        mouseClickedState = Mouse.GetState();

                        Velocity.X = (float)Math.Cos(MathHelper.ToRadians(Angle)) * Speed;
                        Velocity.Y = -1 * (float)Math.Sin(MathHelper.ToRadians(Angle)) * Speed;
                        Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && Singleton.Instance.currentGameState == Singleton.GameSceneState.Playing && Singleton.Instance.currentPlayerStatus == Singleton.PlayerStatus.None)
                        {
                            if (!Singleton.Instance.IsCeilingDowing)
                            {
                                if (!IsBomb)
                                {
                                    ShootBobble(bobble_primary);
                                    SoundEffects["ShootNormal"].Volume = Singleton.Instance.sfxSound;
                                    SoundEffects["ShootNormal"].Play();
                                }
                                else
                                {
                                    ShootBobble(bomb);
                                    SoundEffects["ShootNormal"].Volume = Singleton.Instance.sfxSound;
                                    SoundEffects["ShootNormal"].Play();
                                    IsBomb = false;
                                }
                                Singleton.Instance.turnCounter++;

                                currentShooterState = shooterState.shooterReload;
                            }
                            else
                            {
                                foreach (GameObject g in gameObjects)
                                {
                                    if (g.Name.Equals("WarningText") && !g.IsActive)
                                    {
                                        cur = gameTime.TotalGameTime.TotalSeconds;
                                        g.IsActive = true;
                                    }
                                }
                            }
                        }
                        else if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released && Singleton.Instance.currentGameState == Singleton.GameSceneState.Playing && Singleton.Instance.currentPlayerStatus == Singleton.PlayerStatus.None && !IsBomb)
                        {
                            SwapBobble(bobble_primary, bobble_secondary, gameObjects);
                            NormalBobble temp;
                            temp = bobble_primary;
                            bobble_primary = bobble_secondary;
                            bobble_secondary = temp;
                        }
                        break;
                }
                if (gameTime.TotalGameTime.TotalSeconds - cur >= 0.5f)
                {
                    foreach (GameObject g in gameObjects)
                    {
                        if (g.Name.Equals("WarningText") && g.IsActive) g.IsActive = false;
                    }
                }
            }

            base.Update(gameTime, gameObjects);
        }

        private void ShootBobble(Bobble bob)
        {
            bob.Angle = (float)mouseAngle;
            bob.Speed = 700;
            bob = null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(body, Position, null, Color.White, 0f, new Vector2(body.Width / 2, body.Height / 2), 1f, SpriteEffects.None, 0.9f);
            spriteBatch.Draw(_texture, Position, null, Color.White, MathHelper.ToRadians((float)-(mouseAngle + 90)), new Vector2(_texture.Width / 2, 0), 1f, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(insideBody, new Vector2(Position.X + body.Width / 3, Position.Y + body.Height / 3), null, Color.White, 0f, new Vector2(body.Width / 2, body.Height / 2), 1f, SpriteEffects.None, 0.7f);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = true;
            base.Reset();
        }

        protected void SwapBobble(GameObject b1, GameObject b2, List<GameObject> gameObjects)
        {
            if (gameObjects.Contains(b1) && gameObjects.Contains(b2))
            {
                int index = gameObjects.FindIndex(x => x.Equals(b1));
                int index2 = gameObjects.FindIndex(x => x.Equals(b2));

                gameObjects[index].Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 150, Singleton.MAINSCREEN_HEIGHT - 75);
                gameObjects[index2].Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2 - 25, Singleton.MAINSCREEN_HEIGHT - 75);
            }
        }
    }
}
