using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class Bobble : GameObject
    {
        public float Speed;
        public float Angle;
        public bool isNeverShoot = true;
        public bool isInitialized;

        MouseState mouseState, previousMouseState;
        public enum BobbleColor { Red, Green, Blue, Yellow, White, Turquoise, Purple, Orange }

        public Bobble(Texture2D texture) : base(texture)
        {
            isNeverShoot &= !isInitialized;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            int s = gameObjects.Count;
            GameObject a = gameObjects[s - 2];
            int j = (int)Math.Round(Position.Y / 44) % 2;
            int yGrid = (int)Math.Round(Position.Y / 44) * 44;
            int xGrid = j * (Singleton.BOBBLE_SIZE / 2) + (int)Math.Round(Position.X / 50) * 50;

            foreach (GameObject g in gameObjects)
            {
                if (!g.Equals(this) && g.Name.Equals("NormalBobble") && this.circleCollide(g) && isNeverShoot && g.IsActive)
                {
                    this.Speed = 0;
                    if (j == 1) xGrid = j * (Singleton.BOBBLE_SIZE / 2) + (int)(Math.Floor((Position.X) / 50)) * 50;
                    if (Position.Equals(g.Position)) xGrid -= 50;

                    if ((xGrid - 200) / (Singleton.BOBBLE_SIZE / 2) >= 15)
                    {
                        bool hasLeft, hasDown;
                        hasLeft = hasDown = false;

                        int v = (int)(this.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                        int w = (int)this.Position.Y / 44;

                        foreach (GameObject obj in gameObjects)
                        {
                            if (g.Name.Equals("NormalBobble") && g.IsActive)
                            {
                                int x = (int)(g.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                                int y = (int)g.Position.Y / 44;

                                if (v - 2 == x) hasLeft = true;
                                hasDown |= (v - 1 == x && w + 1 == y);
                            }
                        }

                        if (!hasLeft) xGrid = 13 * (Singleton.BOBBLE_SIZE / 2) + 200;
                        else if (!hasDown)
                        {
                            xGrid = 14 * (Singleton.BOBBLE_SIZE / 2) + 200;
                            yGrid += 44;
                        }
                    }
                    else if ((xGrid - 200) / (Singleton.BOBBLE_SIZE / 2) < 0)
                    {
                        bool hasRight, hasDown;
                        hasRight = hasDown = false;

                        int v = (int)(this.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                        int w = (int)this.Position.Y / 44;

                        foreach (GameObject obj in gameObjects)
                        {
                            if (g.Name.Equals("NormalBobble") && g.IsActive)
                            {
                                int x = (int)(g.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                                int y = (int)g.Position.Y / 44;

                                if (v + 2 == x) hasRight = true;
                                hasDown |= (v + 1 == x && w + 1 == y);
                            }
                        }

                        if (!hasRight) xGrid = 1 * (Singleton.BOBBLE_SIZE / 2) + 200;
                        else if (!hasDown)
                        {
                            xGrid = 200;
                            yGrid += 44;
                        }
                    }

                    Position = new Vector2(xGrid, yGrid);
                    Velocity = Vector2.Zero;
                    isNeverShoot = false;
                }
            }

            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            Velocity.X = (float)Math.Cos(MathHelper.ToRadians(Angle)) * Speed;
            Velocity.Y = -1 * (float)Math.Sin(MathHelper.ToRadians(Angle)) * Speed;
            Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (Position.X <= 200) Angle = 180 - Angle;
            if (Position.X >= 550) Angle = 180 - Angle;

            if (Position.Y < (Singleton.Instance.ceilingLevel * 44) && isNeverShoot)
            {
                Speed = 0;
                xGrid = (int)Math.Round(Position.X / 50) * 50;
                yGrid = (int)Math.Round(Position.Y / 44) * 44;
                Position = new Vector2(j * (Singleton.BOBBLE_SIZE / 2) + xGrid, yGrid);

                isNeverShoot = false;
            }

            if (Velocity == Vector2.Zero && !isInitialized && !isNeverShoot)
            {
                destroyCluster(gameObjects, this, 3);
                isInitialized = true;
            }

            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = true;
            base.Reset();
        }
        public static void ResetVisited(List<GameObject> gameObjects)
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name == "NormalBobble" && g.IsActive) g.IsVisited = false;
            }
        }

        public static void ResetWaited(List<GameObject> gameObjects)
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name == "NormalBobble" && g.IsActive) g.IsWaited = false;
            }
        }
        public static void destroySeparate(List<GameObject> gameObjects)
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name == "NormalBobble" && g.IsActive)
                {
                    ResetVisited(gameObjects);

                    g.IsWaited |= destroySeperateHandler(gameObjects, g);
                    ResetVisited(gameObjects);
                }
            }

            foreach (GameObject g in gameObjects)
            {
                int y = (int)g.Position.Y / 44;
                if (g.Name == "NormalBobble" && g.IsActive && !g.IsWaited && y != (Singleton.Instance.ceilingLevel * 44)) g.IsActive = false;
            }

            ResetWaited(gameObjects);
        }

        public static bool destroySeperateHandler(List<GameObject> gameObjects, GameObject current)
        {
            Stack<GameObject> s = new Stack<GameObject>();
            s.Push(current);

            while (s.Count != 0)
            {
                current = s.Pop();

                int j = (int)(current.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                int i = (int)current.Position.Y / 44;

                if (i == Singleton.Instance.ceilingLevel) return true;

                foreach (GameObject g in gameObjects)
                {
                    if (g.Name == "NormalBobble" && g.IsActive && !g.IsVisited)
                    {
                        int x = (int)(g.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                        int y = (int)g.Position.Y / 44;

                        bool isChecked = false;

                        if (i - 1 == y)
                        {
                            isChecked |= j - 1 == x;
                            isChecked |= j + 1 == x;
                        }
                        if (i == y)
                        {
                            isChecked |= j - 2 == x;
                            isChecked |= j + 2 == x;
                        }
                        if (i + 1 == y)
                        {
                            isChecked |= j - 1 == x;
                            isChecked |= j + 1 == x;
                        }

                        if (isChecked)
                        {
                            s.Push(g);
                            g.IsVisited = true;
                            if (y <= Singleton.Instance.ceilingLevel) return true;
                        }
                    }
                }
            }
            return false;
        }

        public void destroyCluster(List<GameObject> gameObjects, GameObject current, int clusterSize)
        {
            ResetVisited(gameObjects);

            Console.WriteLine(findCluster(gameObjects, current));
            ResetVisited(gameObjects);

            int indicator = (Singleton.Instance.colorVariety / 2) * ((35 - Singleton.Instance.ceilingTime) / 5);
            if (Singleton.Instance.IsBlindMode) indicator++;

            if (findCluster(gameObjects, current) >= clusterSize)
            {
                foreach (GameObject g in gameObjects)
                {
                    if (g.Name == "NormalBobble" && g.IsActive && g.IsVisited)
                    {
                        g.IsActive = false;
                        Singleton.Instance.score += 50 * indicator++;
                    }
                }
            }

            ResetVisited(gameObjects);

            if (current.Velocity == Vector2.Zero) destroySeparate(gameObjects);
        }

        private int findCluster(List<GameObject> gameObjects, GameObject current)
        {
            int count = 0;

            Queue<GameObject> q = new Queue<GameObject>();
            q.Enqueue(current);

            while (q.Count != 0)
            {
                current = q.Dequeue();

                int j = (int)(current.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                int i = (int)current.Position.Y / 44;

                foreach (GameObject g in gameObjects)
                {
                    if (g.Name == "NormalBobble" && g.IsActive && !g.IsVisited && g.bobbleColor == current.bobbleColor)
                    {
                        bool isChecked = false;

                        int x = (int)(g.Position.X - 200) / (Singleton.BOBBLE_SIZE / 2);
                        int y = (int)g.Position.Y / 44;

                        if (i - 1 == y)
                        {
                            isChecked |= j - 1 == x;
                            isChecked |= j + 1 == x;
                        }
                        if (i == y)
                        {
                            isChecked |= j - 2 == x;
                            isChecked |= j + 2 == x;
                        }
                        if (i + 1 == y)
                        {
                            isChecked |= j - 1 == x;
                            isChecked |= j + 1 == x;
                        }

                        if (isChecked)
                        {
                            Console.WriteLine("Trigger >> " + x + " " + y);
                            q.Enqueue(g);
                            g.IsVisited = true;
                            count++;
                        }
                    }
                }
            }
            return count;
        }
    }
}
