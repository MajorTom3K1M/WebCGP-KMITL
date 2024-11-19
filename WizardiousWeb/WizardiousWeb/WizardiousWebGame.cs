using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace WizardiousWeb
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class WizardiousWebGame : Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SceneManager sceneManager;

        #endregion

        #region Initialization
        public WizardiousWebGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Singleton.MAINSCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Singleton.MAINSCREEN_HEIGHT;
            //graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Create the screen manager component.
            sceneManager = new SceneManager(this);

            Components.Add(sceneManager);

            //TODO: Add new screen here
            //sceneManager.AddScreen(new SplashScreen());
            sceneManager.AddScreen(new SplashScreen());

            this.IsMouseVisible = false;
        }
        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialization
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnResize;

            // TODO: Add your initialization logic here
            graphics.ApplyChanges();

            base.Initialize();

        }

        private void OnResize(object sender, System.EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Singleton.MAINSCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Singleton.MAINSCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
