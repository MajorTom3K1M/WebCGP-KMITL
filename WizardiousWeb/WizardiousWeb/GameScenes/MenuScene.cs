using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardiousWeb
{
    class MenuScene : GameScene
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Texture2D background, up, down, play, border;
        Texture2D backgroundTexture;

        #endregion

        #region Variables

        //TODO: Add Variables Here

        List<GameObject> gameObjects = Singleton.Instance.gameObjects;
        int objCount;

        bool playAscend;
        int curUp, curDown, curPlay;

        #endregion

        #region Initialization

        public MenuScene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(4);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            //Load Content Here
            backgroundTexture = content.Load<Texture2D>("Images/Rect_Texture");

            background = content.Load<Texture2D>("Menu/menu_bg");
            up = content.Load<Texture2D>("Menu/menu_up");
            down = content.Load<Texture2D>("Menu/menu_down");
            play = content.Load<Texture2D>("Menu/menu_play");
            border = content.Load<Texture2D>("Menu/menu_border");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            Singleton.Instance.CurrentKey = Keyboard.GetState();

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Enter) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                SceneManager.AddScreen(new Level01());
                SceneManager.RemoveScreen(this);
            }

            base.Update(gameTime);

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            Viewport viewport = SceneManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            if (curUp < 500)
            {
                if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                {
                    curUp += 10;
                }
            }

            if (curUp >= 500 && curDown < 500)
            {
                if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                {
                    curDown += 10;
                }
            }

            if (curPlay > 250) playAscend = false;
            else if (curPlay < 1) playAscend = true;

            if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
            {
                if (playAscend) curPlay += 3;
                else curPlay -= 5;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), null, new Color(fade, fade, fade, fade), 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(up, new Vector2(0, curUp - 500), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(down, new Vector2(0, 500 - curDown), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(play, Vector2.Zero, null, new Color(curPlay, curPlay, curPlay, curPlay), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(border, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        #endregion
    }
}
