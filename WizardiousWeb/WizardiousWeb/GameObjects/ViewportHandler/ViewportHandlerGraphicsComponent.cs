using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WizardiousWeb
{
    public class ViewportHandlerGraphicsComponent : GraphicsComponent
    {
        public ViewportHandlerGraphicsComponent(GameScene currentScene) : base(currentScene)
        {
            Texture2D rectTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Rect_Texture");
            _texture = rectTexture;
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
        }
    }
}
