using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class ActiveZoneGraphicsComponent : GraphicsComponent
    {
        public Vector2 position;
        public Rectangle rectangle;
        public Texture2D activeZone;
        public ActiveZoneGraphicsComponent(GameScene currentScene) : base(currentScene)
        {
            activeZone = currentScene.SceneManager.Content.Load<Texture2D>("Images/Active");
            _texture = activeZone;
            color = Color.Black * 0.5f;
        }
        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            base.Draw(spriteBatch, parent);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {

            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
            if (sender.Equals(this)) return;

        }
    }
}
