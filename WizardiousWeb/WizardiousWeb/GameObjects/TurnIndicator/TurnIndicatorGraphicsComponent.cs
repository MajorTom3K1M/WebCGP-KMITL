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
    public class TurnIndicatorGraphicsComponent : GraphicsComponent
    {
        public ActiveZonePhysicsComponent azpc;
        public List<GameObject> turnList;

        Texture2D rectTexture;

        public TurnIndicatorGraphicsComponent(GameScene currentScene, ActiveZonePhysicsComponent azc) : base(currentScene)
        {
            rectTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Rect_Texture");
            _texture = rectTexture;
            azpc = azc;

            turnList = azpc.activeTurn;
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            for (int i = 0; i < turnList.Count(); ++i)
            {
                if (turnList[i].Name.Equals("Player"))
                {
                    spriteBatch.Draw(rectTexture, new Rectangle((int)((Singleton.Instance.CameraPosition.X) + (60 * i)), Singleton.MAINSCREEN_HEIGHT / 4, 50, 50), Color.White);
                }
                else if (turnList[i].Name.Equals("Enemy"))
                {
                    if (turnList[i].SubName == null)
                    {
                        //TODO: Default
                        spriteBatch.Draw(rectTexture, new Rectangle((int)((Singleton.Instance.CameraPosition.X) + (60 * i)), Singleton.MAINSCREEN_HEIGHT / 4, 50, 50), Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(rectTexture, new Rectangle((int)((Singleton.Instance.CameraPosition.X) + (60 * i)), Singleton.MAINSCREEN_HEIGHT / 4, 50, 50), Color.Blue);
                    }
                }
            }


            base.Draw(spriteBatch, parent);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            turnList = azpc.activeTurn;

            if (turnList.Count() == 1 && turnList[0].Name.Equals("Player"))
            {
                parent.IsActive = false;
            }

            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
        }
    }
}
