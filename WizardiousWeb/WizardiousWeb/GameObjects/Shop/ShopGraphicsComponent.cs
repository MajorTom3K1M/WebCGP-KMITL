﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardiousWeb
{
    public class ShopGraphicsComponent : GraphicsComponent
    {
        public ShopGraphicsComponent(GameScene currentScene) : base(currentScene)
        {
            Texture2D shopTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/ShopKeeper");
            _texture = shopTexture;
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

            if (message == 101)
            {

            }
            else if (message == 100)
            {

            }

        }

    }
}
