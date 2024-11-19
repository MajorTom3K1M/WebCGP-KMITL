using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using nkast.Wasm.Dom;
using System;
using System.Collections.Generic;

namespace PuzzleBobbleWeb
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PuzzleBobbleWebGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Global Variable
        public static Texture2D bobble_red, bobble_green, bobble_blue, bobble_yellow, bobble_white, bobble_turquoise, bobble_orange, bobble_purple, bombbobble;
        public static SoundEffectInstance burst;

        Texture2D rectTexture, mouseNormal;
        Texture2D pointer, bobble_shooter, shooter_inside, loseCollider;
        Texture2D cave, splashScreen, game_bg, gameover_panel, win_panel, warning;
        Texture2D menuBG, menuParallax, menuTitle;
        Texture2D buttonNew, buttonOption, buttonExtras, buttonExit, buttonBack, backmenu_button;
        Texture2D optionBG, optionParallax, levelIndicator, speed30, speed25, speed20, color4, color6, color8;
        Texture2D extrasBG, hcMode, blMode, nmMode;

        List<GameObject> gameObjects;
        int numObj;

        Queue<GameObject> q = new Queue<GameObject>();

        Random rnd = new Random();

        double currentGameTime;
        bool isFirstTime;
        int parallaxHelper;
        bool isAscend;

        Song bgm;

        //private Vector2 _relativeMousePosition = new Vector2(-100, -100); // Start off-screen
        #endregion
        public PuzzleBobbleWebGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.Title = "GEMyth";
        }

        [JSInvokable]
        public void UpdateMousePosition(float x, float y)
        {
            if (x >= 0 && y >= 0)
            {
                Singleton.SetMousePosition(new Vector2(x, y));
            }
            else
            {
                Singleton.SetMousePosition(new Vector2(-100, -100));
            }
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Singleton.MAINSCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Singleton.MAINSCREEN_HEIGHT;
            graphics.ApplyChanges();

            Singleton.Instance.currentGameScene = Singleton.GameScene.TitleScene;
            Singleton.Instance.currentGameState = Singleton.GameSceneState.None;

            // Initialization
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnResize;

            isFirstTime = true;

            parallaxHelper = 0;
            isAscend = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameObjects = new List<GameObject>();
            rectTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);

            Color[] dataRect = new Color[1 * 1];
            for (int i = 0; i < dataRect.Length; ++i) dataRect[i] = Color.White;
            rectTexture.SetData(dataRect);

            #region Import Content
            bgm = this.Content.Load<Song>("bgm");
            burst = this.Content.Load<SoundEffect>("burst").CreateInstance();

            splashScreen = this.Content.Load<Texture2D>("splashScreen");

            buttonNew = this.Content.Load<Texture2D>("button_new");
            buttonOption = this.Content.Load<Texture2D>("button_option");
            buttonExtras = this.Content.Load<Texture2D>("button_ext");
            buttonExit = this.Content.Load<Texture2D>("button_exit");

            menuBG = this.Content.Load<Texture2D>("menu_bg");
            menuTitle = this.Content.Load<Texture2D>("menu_title");
            menuParallax = this.Content.Load<Texture2D>("menu_parallax");

            warning = this.Content.Load<Texture2D>("warning_text");

            mouseNormal = this.Content.Load<Texture2D>("mousepointer");

            //Reference: https://www.reddit.com/r/PixelArt/comments/61xvdq/ocwipcc_a_parallax_cave_background_i_made/
            cave = this.Content.Load<Texture2D>("cave");

            //Reference: http://steamtradingcards.wikia.com/wiki/File:Amygdala_Background_Cave_Background.jpg
            game_bg = this.Content.Load<Texture2D>("game_bg");

            bobble_red = this.Content.Load<Texture2D>("bobble_red");
            bobble_blue = this.Content.Load<Texture2D>("bobble_blue");
            bobble_green = this.Content.Load<Texture2D>("bobble_green");
            bobble_yellow = this.Content.Load<Texture2D>("bobble_yellow");
            bobble_white = this.Content.Load<Texture2D>("bobble_white");
            bobble_turquoise = this.Content.Load<Texture2D>("bobble_turquoise");
            bobble_purple = this.Content.Load<Texture2D>("bobble_purple");
            bobble_orange = this.Content.Load<Texture2D>("bobble_orange");
            bombbobble = this.Content.Load<Texture2D>("bobble_bomb");

            Singleton.Instance.gameFont = this.Content.Load<SpriteFont>("GameFont");

            bobble_shooter = this.Content.Load<Texture2D>("bobble_shooter");
            shooter_inside = this.Content.Load<Texture2D>("shooter_center");
            pointer = this.Content.Load<Texture2D>("arrow");
            loseCollider = this.Content.Load<Texture2D>("lose_collider");

            gameover_panel = this.Content.Load<Texture2D>("gameover_panel");
            win_panel = this.Content.Load<Texture2D>("win_panel");

            optionBG = this.Content.Load<Texture2D>("option_bg");
            optionParallax = this.Content.Load<Texture2D>("star_parallax");
            levelIndicator = this.Content.Load<Texture2D>("level_indicator");
            speed30 = this.Content.Load<Texture2D>("speed30");
            speed25 = this.Content.Load<Texture2D>("speed25");
            speed20 = this.Content.Load<Texture2D>("speed20");
            color4 = this.Content.Load<Texture2D>("color4");
            color6 = this.Content.Load<Texture2D>("color6");
            color8 = this.Content.Load<Texture2D>("color8");

            buttonBack = this.Content.Load<Texture2D>("back_button");
            backmenu_button = this.Content.Load<Texture2D>("backmenu_button");

            extrasBG = this.Content.Load<Texture2D>("extras_bg");
            hcMode = this.Content.Load<Texture2D>("hcmode");
            blMode = this.Content.Load<Texture2D>("blmode");
            nmMode = this.Content.Load<Texture2D>("nmmode");
            #endregion
            
            switch (Singleton.Instance.currentGameScene)
            {
                case Singleton.GameScene.MenuScene:

                    gameObjects.Add(
                        new Button(buttonNew)
                        {
                            Name = "NewGameButton",
                            Position = new Vector2(100, 300)
                        }
                    );

                    gameObjects.Add(
                        new Button(buttonOption)
                        {
                            Name = "OptionButton",
                            Position = new Vector2(100, 330)
                        }
                    );

                    gameObjects.Add(
                        new Button(buttonExtras)
                        {
                            Name = "ExtrasButton",
                            Position = new Vector2(100, 360)
                        }
                    );

                    gameObjects.Add(
                        new Button(buttonExit)
                        {
                            Name = "ExitButton",
                            Position = new Vector2(100, 450),
                            ColorHovered = Color.DarkRed
                        }
                    );

                    break;
                case Singleton.GameScene.OptionScene:

                    gameObjects.Add(
                        new Button(buttonBack)
                        {
                            Name = "BackButton",
                            Position = new Vector2(20, 20)
                        }
                    );

                    gameObjects.Add(
                        new BGMIndicator(levelIndicator)
                        {
                            Name = "BGMIndicator",
                            Position = new Vector2(560, 235),
                            indicatorIndex = (int)(Singleton.Instance.bgmSound / 0.3f)
                        }
                    );

                    gameObjects.Add(
                        new SFXIndicator(levelIndicator)
                        {
                            Name = "SFXIndicator",
                            Position = new Vector2(560, 280),
                            ColorDisplayed = Color.LightBlue,
                            indicatorIndex = (int)(Singleton.Instance.sfxSound / 0.3f)
                        }
                    );

                    gameObjects.Add(
                        new SpeedSelector(levelIndicator)
                        {
                            Name = "SpeedSelector",
                            Position = new Vector2(560, 330),
                            SelectorButton = new Texture2D[] { speed30, speed25, speed20 },
                            indicatorIndex = (2 - (Singleton.Instance.ceilingTime - 20) / 5)
                        }
                    );

                    gameObjects.Add(
                        new ColorSelector(levelIndicator)
                        {
                            Name = "ColorSelector",
                            Position = new Vector2(560, 410),
                            SelectorButton = new Texture2D[] { color4, color6, color8 },
                            indicatorIndex = ((Singleton.Instance.colorVariety - 4) / 2)
                        }
                    );

                    break;
                case Singleton.GameScene.ExtrasScene:
                    gameObjects.Add(
                        new Button(buttonBack)
                        {
                            Name = "BackButton",
                            Position = new Vector2(20, 20)
                        }
                    );

                    gameObjects.Add(
                        new Button(hcMode)
                        {
                            Name = "HardcorePanel",
                            Position = new Vector2(77, 228)
                        }
                    );

                    gameObjects.Add(
                        new Button(blMode)
                        {
                            Name = "BlindPanel",
                            Position = new Vector2(312, 228)
                        }
                    );

                    gameObjects.Add(
                        new Button(nmMode)
                        {
                            Name = "NightmarePanel",
                            Position = new Vector2(551, 228)
                        }
                    );

                    gameObjects.Add(
                        new Window(warning)
                        {
                            Name = "WarningPopup",
                            Position = new Vector2(551 + (nmMode.Width / 2) - warning.Width / 2, 238 + nmMode.Height)
                        }
                    );

                    break;
                case Singleton.GameScene.GameScene:

                    gameObjects.Add(
                        new LoseCollider(loseCollider)
                        {
                            Name = "LoseCollider",
                            Position = new Vector2(200, 500)
                        }
                    );

                    gameObjects.Add(
                        new BobbleShooter(pointer)
                        {
                            Name = "Shooter",
                            Position = new Vector2(Singleton.MAINSCREEN_WIDTH / 2, Singleton.MAINSCREEN_HEIGHT - 50),
                            body = bobble_shooter,
                            insideBody = shooter_inside,
                            SoundEffects = new Dictionary<string, SoundEffectInstance>()
                            {
                                {"ShootNormal", Content.Load<SoundEffect>("shoot_normal").CreateInstance() },
                            }
                        }
                    );

                    gameObjects.Add(
                        new ScoreDisplayer(buttonNew)
                        {
                            Name = "ScoreDisplayer",
                            Position = new Vector2(625, 150)
                        }
                    );

                    #region Initial Bobble Pattern
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int j = (i % 2); j < 15; j += 2)
                        {
                            int iOffset = 6 * i;
                            if (i < 2)
                            {
                                if (j < 4)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_red)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Red,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else if (j < 8)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_yellow)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Yellow,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else if (j < 12)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_blue)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Blue,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_green)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Green,
                                            isInitialized = true
                                        }
                                    );
                                }
                            }
                            else if (i < 4)
                            {
                                if (j < 3)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_blue)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Blue,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else if (j < 7)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_green)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Green,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else if (j < 11)
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_red)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Red,
                                            isInitialized = true
                                        }
                                    );
                                }
                                else
                                {
                                    gameObjects.Add(
                                        new NormalBobble(bobble_yellow)
                                        {
                                            Name = "NormalBobble",
                                            Position = new Vector2(j * Singleton.BOBBLE_SIZE / 2 + 200, i * Singleton.BOBBLE_SIZE - iOffset),
                                            bobbleColor = NormalBobble.BobbleColor.Yellow,
                                            isInitialized = true
                                        }
                                    );
                                }
                            }
                        }
                    }
                    #endregion
                    gameObjects.Add(
                        new Window(gameover_panel)
                        {
                            Name = "LoseWindow",
                            Position = new Vector2(0, 0),
                            IsActive = false
                        }
                    );

                    gameObjects.Add(
                        new Window(win_panel)
                        {
                            Name = "WinWindow",
                            Position = new Vector2(0, 0),
                            IsActive = false
                        }
                    );

                    gameObjects.Add(
                        new Button(backmenu_button)
                        {
                            Name = "BackMenuButton",
                            Position = new Vector2(380, 335),
                            InitialActivated = false
                        }
                    );

                    gameObjects.Add(
                        new Window(warning)
                        {
                            Name = "WarningText",
                            Position = new Vector2(400 - warning.Width / 2, 450)
                        }
                    );
                    break;
            }
            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif

            Singleton.GameScene gameScene = Singleton.Instance.currentGameScene;

            numObj = gameObjects.Count;

            for (int i = 0; i < numObj; i++)
            {
                if (gameObjects[i].IsActive) gameObjects[i].Update(gameTime, gameObjects);
            }

            switch (Singleton.Instance.currentGameScene)
            {
                case Singleton.GameScene.TitleScene:
                    if (isFirstTime)
                    {
                        currentGameTime = gameTime.TotalGameTime.TotalSeconds;
                        isFirstTime = false;
                    }

                    if (gameTime.TotalGameTime.TotalSeconds - currentGameTime >= Singleton.SPLASH_TIME) Singleton.Instance.currentGameScene = Singleton.GameScene.MenuScene;

                    break;
                case Singleton.GameScene.GameScene:
                    switch (Singleton.Instance.currentGameState)
                    {
                        case Singleton.GameSceneState.Start:
                            isFirstTime = true;
                            Singleton.Instance.currentGameState = Singleton.GameSceneState.Playing;
                            break;
                        case Singleton.GameSceneState.Playing:
                            if (isFirstTime)
                            {
                                currentGameTime = gameTime.TotalGameTime.TotalSeconds;
                                isFirstTime = false;
                            }

                            if (Math.Round(gameTime.TotalGameTime.TotalSeconds - currentGameTime) >= Singleton.Instance.ceilingTime)
                            {
                                CeilingDown();
                                Singleton.Instance.IsCeilingDowing = false;
                                currentGameTime = gameTime.TotalGameTime.TotalSeconds;
                            }
                            else if (Math.Round(gameTime.TotalGameTime.TotalSeconds - currentGameTime) >= Singleton.Instance.ceilingTime - 1) Singleton.Instance.IsCeilingDowing = true;
                            #region Game Over
                            int count = 0;

                            foreach (GameObject g in gameObjects)
                            {
                                if (g.Name.Equals("NormalBobble") && g.Position.Y == (Singleton.Instance.ceilingLevel * 44) && g.IsActive) count++;
                            }

                            if (count == 0)
                            {
                                Singleton.Instance.currentPlayerStatus = Singleton.PlayerStatus.Won;
                                Singleton.Instance.currentGameState = Singleton.GameSceneState.End;
                            }
                            #endregion
                            break;
                        case Singleton.GameSceneState.End:
                            switch (Singleton.Instance.currentPlayerStatus)
                            {
                                case Singleton.PlayerStatus.Won:
                                    foreach (GameObject g in gameObjects)
                                    {
                                        if (g.Name.Equals("WinWindow") && !g.IsActive) g.IsActive = true;
                                        if (g.Name.Equals("BackMenuButton") && !g.IsActive) g.IsActive = true;
                                    }
                                    break;
                                case Singleton.PlayerStatus.Lost:
                                    foreach (GameObject g in gameObjects)
                                    {
                                        if (g.Name.Equals("LoseWindow") && !g.IsActive) g.IsActive = true;
                                        if (g.Name.Equals("BackMenuButton") && !g.IsActive) g.IsActive = true;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }

            if (gameScene != Singleton.Instance.currentGameScene) LoadContent();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            numObj = gameObjects.Count;

            spriteBatch.Begin(SpriteSortMode.BackToFront);
            #region Mouse Cursor
            //spriteBatch.Draw(mouseNormal, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mouseNormal, Singleton.MousePosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            #endregion
            switch (Singleton.Instance.currentGameScene)
            {
                case Singleton.GameScene.TitleScene:
                    spriteBatch.Draw(splashScreen, Vector2.Zero, null, new Color(Color.White, parallaxHelper), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    break;
                case Singleton.GameScene.MenuScene:
                    spriteBatch.Draw(menuBG, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    if (parallaxHelper > 250) isAscend = false;
                    else isAscend |= parallaxHelper < 1;

                    if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                    {
                        if (isAscend) parallaxHelper++;
                        else parallaxHelper--;
                    }
                    spriteBatch.Draw(menuParallax, Vector2.Zero, null, new Color(Color.White, parallaxHelper), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
                    spriteBatch.Draw(menuTitle, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

                    break;
                case Singleton.GameScene.OptionScene:
                    spriteBatch.Draw(optionBG, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    if (parallaxHelper > 250) isAscend = false;
                    else isAscend |= parallaxHelper < 1;

                    if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                    {
                        if (isAscend) parallaxHelper += 2;
                        else parallaxHelper -= 2;
                    }
                    spriteBatch.Draw(optionParallax, new Vector2(0, 150), null, new Color(Color.White, parallaxHelper), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    break;
                case Singleton.GameScene.ExtrasScene:
                    spriteBatch.Draw(extrasBG, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    break;
                case Singleton.GameScene.GameScene:
                    spriteBatch.Draw(game_bg, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.Draw(cave, new Vector2((Singleton.MAINSCREEN_WIDTH - Singleton.GAMESCREEN_WIDTH) / 2, 0f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    #region Ceiling
                    spriteBatch.Draw(rectTexture, new Rectangle(200, 0, 400, Singleton.Instance.ceilingLevel * 44), null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                    #endregion Ceiling
                    #region Blind Mode
                    if (Singleton.Instance.IsBlindMode)
                    {
                        if (parallaxHelper > 250) isAscend = false;
                        else isAscend |= parallaxHelper < 1;

                        if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                        {
                            if (isAscend) parallaxHelper++;
                            else parallaxHelper--;
                        }
                        spriteBatch.Draw(rectTexture, new Rectangle(200, 0, 400, 600), null, new Color(Color.Black, parallaxHelper), 0f, Vector2.Zero, SpriteEffects.None, 0.15f);
                    }
                    #endregion
                    break;
            }

            for (int i = 0; i < numObj; i++)
            {
                if (gameObjects[i].IsActive) gameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            //graphics.BeginDraw();
            //graphics.Dispose();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.Reset();
            }

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.bgmSound;
            MediaPlayer.Play(bgm);

            Singleton.Instance.ceilingLevel = 0;
            Singleton.Instance.turnCounter = 0;

            if (Singleton.Instance.ceilingTime < 20) Singleton.Instance.ceilingTime = 20;
        }

        protected void CeilingDown()
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name.Equals("NormalBobble") && g.IsActive)
                {
                    Bobble obj = g as Bobble;
                    if (!obj.isNeverShoot || obj.isInitialized) g.Position.Y += 88;
                }
            }
            Singleton.Instance.ceilingLevel += 2;
            for (int i = 0; i < gameObjects.Count; ++i)
            {
                if (gameObjects[i].Name.Equals("NormalBobble") && !gameObjects[i].IsActive) gameObjects.RemoveAt(i);
            }
        }

        public static void CeilingUp(List<GameObject> gameObjects)
        {
            foreach (GameObject g in gameObjects)
            {
                if (g.Name.Equals("NormalBobble") && g.IsActive)
                {
                    Bobble obj = g as Bobble;
                    if (!obj.isNeverShoot || obj.isInitialized) g.Position.Y -= 88;
                }
            }

            Singleton.Instance.ceilingLevel -= 2;

            for (int i = 0; i < gameObjects.Count; ++i)
            {
                if (gameObjects[i].Name.Equals("NormalBobble") && !gameObjects[i].IsActive) gameObjects.RemoveAt(i);
            }

            Bobble.destroySeparate(gameObjects);
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Singleton.MAINSCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Singleton.MAINSCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                spriteBatch?.Dispose();
                mouseNormal?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
