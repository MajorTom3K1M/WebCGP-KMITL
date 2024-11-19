using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class EnemyGraphicsComponent : GraphicsComponent
    {

        Texture2D enemyHp;
        Texture2D rectTexture, enemyTexture;

        Rectangle healthRectangle;
        Vector2 position;

        public float MaxHealth = 80f;
        public float Health;

        public EnemyGraphicsComponent(GameScene currentScene) : base(currentScene)
        {
            enemyTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Poring");
            _texture = enemyTexture;

            rectTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Rect_Texture");

            Health = MaxHealth;

            enemyHp = currentScene.SceneManager.Content.Load<Texture2D>("Images/EnemyHealth");
            healthRectangle = new Rectangle(0, 0, enemyHp.Width, enemyHp.Height);
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            spriteBatch.Draw(enemyHp, healthRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);

            if (parent.SubName != null)
            {
                //Console.WriteLine("Found");
                spriteBatch.Draw(rectTexture, new Rectangle(0, 0, 10, 10), null, Color.Yellow, 0f, new Vector2(parent.Position.X + (enemyTexture.Width / 2), parent.Position.Y - enemyTexture.Height), SpriteEffects.None, 0.4f);
            }

            base.Draw(spriteBatch, parent);
        }

        public override void Reset()
        {
            Health = MaxHealth;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            healthRectangle = new Rectangle((int)parent.Position.X - 40, (int)parent.Position.Y + 28, (int)(Health * 80 / MaxHealth), 10);
            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
            if (sender.Equals(this)) return;

            if (message == 100)
            {
                Health -= Singleton.Instance.Damage;
            }
        }
    }
}
