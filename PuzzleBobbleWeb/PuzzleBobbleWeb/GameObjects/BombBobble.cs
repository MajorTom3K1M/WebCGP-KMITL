using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class BombBobble : Bobble
    {
        int indicator = (Singleton.Instance.colorVariety / 2) * ((35 - Singleton.Instance.ceilingTime) / 5);

        public BombBobble(Texture2D texture) : base(texture)
        {
            if (Singleton.Instance.IsBlindMode) indicator++;
            isNeverShoot = true;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            base.Update(gameTime, gameObjects);
            if (!isNeverShoot)
            {
                Burst(gameObjects);
                //Chance of push celling back 25%
                if (Singleton.Instance.ceilingLevel > 0)
                {
                    Random rand = new Random();
                    int rnd = rand.Next(0, 4);
                    if (rnd == 0) PuzzleBobbleWebGame.CeilingUp(gameObjects);
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
            this.IsActive = true;
            base.Reset();
        }

        protected void Burst(List<GameObject> gameObjects)
        {
            //TODO: Bomb Burst Function
            foreach (GameObject g in gameObjects)
            {
                if (g.Name.Equals("NormalBobble") && g.IsActive)
                {
                    NormalBobble bobble = g as NormalBobble;
                    if (g.Position.Y == this.Position.Y)
                    {
                        if (g.Position.X == this.Position.X - Singleton.BOBBLE_SIZE) DestroyBobble(bobble, gameObjects);
                        if (g.Position.X == this.Position.X + Singleton.BOBBLE_SIZE) DestroyBobble(bobble, gameObjects);
                    }
                    if (g.Position.Y == this.Position.Y - 44)
                    {
                        if (g.Position.X == this.Position.X - Singleton.BOBBLE_SIZE / 2) DestroyBobble(bobble, gameObjects);
                        if (g.Position.X == this.Position.X + Singleton.BOBBLE_SIZE / 2) DestroyBobble(bobble, gameObjects);
                    }
                    if (g.Position.Y == this.Position.Y + 44)
                    {
                        if (g.Position.X == this.Position.X - Singleton.BOBBLE_SIZE / 2) DestroyBobble(bobble, gameObjects);
                        if (g.Position.X == this.Position.X + Singleton.BOBBLE_SIZE / 2) DestroyBobble(bobble, gameObjects);
                    }
                }
            }
            SoundEffects["Burst"].Volume = Singleton.Instance.sfxSound;
            SoundEffects["Burst"].Play();
            this.IsActive = false;
        }

        protected void DestroyBobble(NormalBobble bob, List<GameObject> gameObjects)
        {
            destroyCluster(gameObjects, bob, 2);
            bob.IsActive = false;
            Singleton.Instance.score += 50 * indicator;
        }
    }
}
