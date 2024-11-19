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
    public class ViewportHandlerInputComponent : InputComponent
    {
        public ViewportHandlerInputComponent(GameScene currentScene) : base(currentScene)
        {
            InputList["Left"] = Keys.Left;
            InputList["Right"] = Keys.Right;
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
            if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Right"]))
            {
                parent.Position.X += 10;
            }
            else if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Left"]))
            {
                parent.Position.X -= 10;
            }

            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);

            if (sender.Equals(this)) return;
        }
    }
}
