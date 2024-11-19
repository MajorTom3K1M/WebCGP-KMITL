using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace WizardiousWeb
{
    public class SceneManager : DrawableGameComponent
    {
        #region Fields

        List<GameScene> scenes = new List<GameScene>();
        List<GameScene> scenesToUpdate = new List<GameScene>();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        public ContentManager Content;

        bool isInitialized;

        bool traceEnabled;

        #endregion

        #region Properties

        /*
        public ContentManager Content
        {
            get { return Content; }
        }
        */

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion

        #region Initialization

        public SceneManager(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            // Load content belonging to the scene manager.
            Content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/menufont");
            blankTexture = Content.Load<Texture2D>("Images/Rect_Texture");

            // Tell each of the scenes to load their content.
            foreach (GameScene scene in scenes)
            {
                scene.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            // Tell each of the scenes to unload their content.
            foreach (GameScene scene in scenes)
            {
                scene.UnloadContent();
            }
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            // Make a copy of the master scene list, to avoid confusion if
            // the process of updating one scene adds or removes others.
            scenesToUpdate.Clear();

            foreach (GameScene scene in scenes)
                scenesToUpdate.Add(scene);

            // Loop as long as there are scenes waiting to be updated.
            while (scenesToUpdate.Count > 0)
            {
                // Pop the topmost scene off the waiting list.
                GameScene scene = scenesToUpdate[scenesToUpdate.Count - 1];

                scenesToUpdate.RemoveAt(scenesToUpdate.Count - 1);

                // Update the scene.
                scene.Update(gameTime);
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }


        void TraceScreens()
        {
            List<string> sceneNames = new List<string>();

            foreach (GameScene scene in scenes)
                sceneNames.Add(scene.GetType().Name);

            Trace.WriteLine(string.Join(", ", sceneNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScene scene in scenes)
            {
                scene.Draw(gameTime);
            }
        }


        #endregion

        #region Public Methods

        public void AddScreen(GameScene scene)
        {
            scene.SceneManager = this;
            scene.IsExiting = false;

            // If we have a graphics device, tell the scene to load content.
            if (isInitialized)
            {
                scene.LoadContent();
            }

            scenes.Add(scene);
        }

        public void RemoveScreen(GameScene scene)
        {
            // If we have a graphics device, tell the scene to unload content.
            if (isInitialized)
            {
                scene.UnloadContent();
            }

            scenes.Remove(scene);
            scenesToUpdate.Remove(scene);
        }

        public GameScene[] GetScreens()
        {
            return scenes.ToArray();
        }

        public void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), new Color(0, 0, 0, alpha));

            spriteBatch.End();
        }

        #endregion
    }
}
