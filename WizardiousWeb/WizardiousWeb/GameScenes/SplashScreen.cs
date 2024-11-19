using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardiousWeb
{
    class SplashScreen : GameScene
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Texture2D backgroundTexture, majoxLogo;

        #endregion

        #region Variables

        //TODO: Add Variables Here

        List<GameObject> gameObjects = Singleton.Instance.gameObjects;
        int objCount;

        bool isWaited;
        double curGametime;

        #endregion

        #region Initialization

        public SplashScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            //Load Content Here
            backgroundTexture = content.Load<Texture2D>("Images/Rect_Texture");
            majoxLogo = content.Load<Texture2D>("Images/Majox_Logo");

        }

        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            if (!isWaited)
            {
                isWaited = true;
                curGametime = gameTime.TotalGameTime.Seconds;
            }

            if (gameTime.TotalGameTime.Seconds - curGametime >= Singleton.SPLASH_TIME)
            {
                SceneManager.AddScreen(new MenuScene());
                SceneManager.RemoveScreen(this);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            Viewport viewport = SceneManager.GraphicsDevice.Viewport;
            //Console.WriteLine("viewPort!!!!");
            //Console.WriteLine(viewport);
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(majoxLogo, new Vector2(viewport.Width / 2, viewport.Height / 2), null, new Color(fade, fade, fade), 0f, new Vector2(majoxLogo.Width / 2, majoxLogo.Height / 2), 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        #endregion
    }
}
