using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardiousWeb
{
    class Level02 : GameScene
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        // Lists to hold level chunks for each layer
        List<LevelChunk> levelBaseChunks = new List<LevelChunk>();
        List<LevelChunk> levelBackChunks = new List<LevelChunk>();
        List<LevelChunk> levelBacklayerChunks = new List<LevelChunk>();
        List<LevelChunk> levelBackfoundChunks = new List<LevelChunk>();
        List<LevelChunk> levelParallaxChunks = new List<LevelChunk>();

        //Level Mask
        //Texture2D levelBase, levelBack, levelParallax, levelBacklayer, levelFoundation, levelObject, levelBackfound;
        Texture2D pauseScreen, pauseText;
        Texture2D chapText, background, backgroundTexture;
        Texture2D clearScreen, clearText;
        Texture2D gameOver, returnText;

        #endregion

        #region Variables

        //TODO: Add Variables Here

        public const int LEVEL_WIDTH = 7540;
        public const int LEVEL_HEIGHT = Singleton.MAINSCREEN_HEIGHT;

        List<GameObject> gameObjects = Singleton.Instance.gameObjects;
        int objCount;

        List<Platform> platforms = Singleton.Instance.platforms;

        GameObject player;

        public enum StageState
        {
            Intro,
            Normal,
            Pause,
            Cleared,
            Outro,
            Gameover
        }

        public StageState currentStageState, previousStageState;

        //For fading text
        bool playAscend, chapAscend, waitHelper, ascendHelper;
        int curPlay, curIntro, intensity, curParallax;

        double currentGameTime;

        // Camera
        Camera2d cam = new Camera2d();

        #endregion

        #region Initialization

        public Level02()
        {
            TransitionOnTime = TimeSpan.FromSeconds(2);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(SceneManager.Game.Services, "Content");

            //Level Graphics
            pauseScreen = content.Load<Texture2D>("ex_pause");
            pauseText = content.Load<Texture2D>("ex_pausetext");

            //levelBase = content.Load<Texture2D>("Level02/level02_base");
            //levelBack = content.Load<Texture2D>("Level02/level02_back");
            //levelParallax = content.Load<Texture2D>("Level02/level02_parallax");
            //levelBacklayer = content.Load<Texture2D>("Level02/level02_backlayer");
            //levelFoundation = content.Load<Texture2D>("Level02/level02_foundation");
            //levelBackfound = content.Load<Texture2D>("Level02/level02_backfound");

            chapText = content.Load<Texture2D>("chap2_text");
            background = content.Load<Texture2D>("black_bg");

            clearScreen = content.Load<Texture2D>("clear_screen");
            clearText = content.Load<Texture2D>("nextstage");

            gameOver = content.Load<Texture2D>("gameover");
            returnText = content.Load<Texture2D>("returnmenu");

            backgroundTexture = content.Load<Texture2D>("Images/Rect_Texture");

            LoadLevelChunks();

            player = new GameObject(new PlayerInputComponent(this), new PlayerPhysicsComponent(this), new PlayerGraphicsComponent(this))
            {
                Name = "Player",
                Position = new Vector2(50, 568 - 64),
                Viewport = new Rectangle(0, 0, 64, 64)
            };

            gameObjects.Add(player);

            /*
            player = new GameObject(new ViewportHandlerInputComponent(this), null, null)
            {
                Name = "ViewportHandler",
                Position = new Vector2(50, 640),
                Viewport = new Rectangle(0, 0, 64, 64)
            };

            gameObjects.Add(player);
            */

            //Level Generated Section

            /*Layer -1 : Back Platform*/

            /*Layer 0 : Base*/

            //Floor 200
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(750, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 1500, 200)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(1650, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 100, 200)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(1850, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 100, 200)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2050, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 100, 200)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2050, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 100, 200)
            });

            //Floor 100
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2500, Singleton.MAINSCREEN_HEIGHT - 50),
                Viewport = new Rectangle(0, 0, 500, 100)
            });

            //Stair
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2775, Singleton.MAINSCREEN_HEIGHT - 75),
                Viewport = new Rectangle(0, 0, 50, 150)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2825, Singleton.MAINSCREEN_HEIGHT - 100),
                Viewport = new Rectangle(0, 0, 50, 200)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2875, Singleton.MAINSCREEN_HEIGHT - 125),
                Viewport = new Rectangle(0, 0, 50, 250)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(2925, Singleton.MAINSCREEN_HEIGHT - 150),
                Viewport = new Rectangle(0, 0, 50, 300)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(3075, Singleton.MAINSCREEN_HEIGHT - 175),
                Viewport = new Rectangle(0, 0, 250, 350)
            });

            //Floor 50
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(3575, Singleton.MAINSCREEN_HEIGHT - 25),
                Viewport = new Rectangle(0, 0, 750, 50)
            });

            //Floor 400
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(4050, Singleton.MAINSCREEN_HEIGHT - 200),
                Viewport = new Rectangle(0, 0, 100, 400)
            });

            //Floor 450
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(4450, Singleton.MAINSCREEN_HEIGHT - 225),
                Viewport = new Rectangle(0, 0, 700, 450)
            });

            //Down Stair
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(4875, Singleton.MAINSCREEN_HEIGHT - 175),
                Viewport = new Rectangle(0, 0, 50, 350)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(4975, Singleton.MAINSCREEN_HEIGHT - 150),
                Viewport = new Rectangle(0, 0, 50, 300)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(5075, Singleton.MAINSCREEN_HEIGHT - 125),
                Viewport = new Rectangle(0, 0, 50, 250)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(5175, Singleton.MAINSCREEN_HEIGHT - 125),
                Viewport = new Rectangle(0, 0, 50, 250)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(5475, Singleton.MAINSCREEN_HEIGHT - 125),
                Viewport = new Rectangle(0, 0, 450, 250)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(6770, Singleton.MAINSCREEN_HEIGHT - 125),
                Viewport = new Rectangle(0, 0, 1540, 250)
            });

            //Celling
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(LEVEL_WIDTH / 2, 215 / 2),
                Viewport = new Rectangle(0, 0, LEVEL_WIDTH, 215)
            });

            //Mask
            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(3475, 428),
                Viewport = new Rectangle(0, 0, 450, 20)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(3750 + 125, 428),
                Viewport = new Rectangle(0, 0, 250, 20)
            });

            gameObjects.Add(new GameObject(null, new FloorPhysicsComponent(this), null)
            {
                Name = "Floor",
                Position = new Vector2(5850, 528),
                Viewport = new Rectangle(0, 0, 300, 20)
            });

            //Platform

            /*Layer 1 : Object*/

            //Interactable Object

            //Entity

            //Active Zone 1
            gameObjects.Add(new GameObject(null, new ActiveZonePhysicsComponent(this), null)
            {
                Name = "ActiveZone",
                Position = new Vector2(1600, Singleton.MAINSCREEN_HEIGHT / 2),
                Viewport = new Rectangle(0, 0, 600, Singleton.MAINSCREEN_HEIGHT)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(1650, 568 - 50),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(1850, 568 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            //Active Zone 2
            gameObjects.Add(new GameObject(null, new ActiveZonePhysicsComponent(this), null)
            {
                Name = "ActiveZone",
                Position = new Vector2(3450, Singleton.MAINSCREEN_HEIGHT / 2),
                Viewport = new Rectangle(0, 0, 900, Singleton.MAINSCREEN_HEIGHT)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(3450, 418 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(3300, 418 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(3600, 418 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            //Active Zone 3
            gameObjects.Add(new GameObject(null, new ActiveZonePhysicsComponent(this), null)
            {
                Name = "ActiveZone",
                Position = new Vector2(5400, Singleton.MAINSCREEN_HEIGHT / 2),
                Viewport = new Rectangle(0, 0, 600, Singleton.MAINSCREEN_HEIGHT)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(5300, 518 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(5400, 518 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            gameObjects.Add(new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this))
            {
                Name = "Enemy",
                Position = new Vector2(5500, 518 - 25),
                Viewport = new Rectangle(0, 0, 50, 50)
            });

            //Active Zone 4
            gameObjects.Add(new GameObject(null, new ActiveZonePhysicsComponent(this), null)
            {
                Name = "ActiveZone",
                Position = new Vector2(7000, Singleton.MAINSCREEN_HEIGHT / 2),
                Viewport = new Rectangle(0, 0, 600, Singleton.MAINSCREEN_HEIGHT)
            });

            GameObject boss = new GameObject(new EnemyAIComponent(this), new EnemyPhysicsComponent(this), new Enemy2GraphicsComponent(this));

            boss.Name = "Enemy";
            boss.SubName = "BossA";
            boss.Position = new Vector2(7000, 518 - 50);
            boss.Viewport = new Rectangle(0, 0, 50, 50);

            Enemy2GraphicsComponent bossGraphics = boss.Graphics as Enemy2GraphicsComponent;
            bossGraphics.MaxHealth = 800f;
            bossGraphics.Reset();

            EnemyAIComponent bossAI = boss.Input as EnemyAIComponent;
            bossAI.MaxHealth = 800f;
            bossAI.MaxTurnSpeed = 2;
            bossAI.Reset();

            gameObjects.Add(boss);

        }

        private void LoadLevelChunks()
        {
            Console.WriteLine("Load new chunks");

            // Define chunk width and height
            int chunkWidth = 1024; // Adjust as needed
            int chunkHeight = LEVEL_HEIGHT; // Assuming full height

            // Load levelBase chunks
            int baseChunksCount = (int)Math.Ceiling((double)LEVEL_WIDTH / chunkWidth);
            for (int i = 1; i <= baseChunksCount; i++)
            {
                string assetName = $"Level02/level02_base_{i}";
                Texture2D texture = content.Load<Texture2D>(assetName);
                Vector2 position = new Vector2((i - 1) * chunkWidth, 0); // Adjust Y if needed
                levelBaseChunks.Add(new LevelChunk(texture, position));
            }

            // Load levelBack chunks
            for (int i = 1; i <= baseChunksCount; i++)
            {
                string assetName = $"Level02/level02_back_{i}";
                Texture2D texture = content.Load<Texture2D>(assetName);
                Vector2 position = new Vector2((i - 1) * chunkWidth, 0);
                levelBackChunks.Add(new LevelChunk(texture, position));
            }

            // Load levelBacklayer chunks
            for (int i = 1; i <= baseChunksCount; i++)
            {
                string assetName = $"Level02/level02_backlayer_{i}";
                Texture2D texture = content.Load<Texture2D>(assetName);
                Vector2 position = new Vector2((i - 1) * chunkWidth, 0);
                levelBacklayerChunks.Add(new LevelChunk(texture, position));
            }

            // Load levelBackfound chunks
            for (int i = 1; i <= baseChunksCount; i++)
            {
                string assetName = $"Level02/level02_backfound_{i}";
                Texture2D texture = content.Load<Texture2D>(assetName);
                Vector2 position = new Vector2((i - 1) * chunkWidth, 0);
                levelBackfoundChunks.Add(new LevelChunk(texture, position));
            }

            // Load levelParallax chunks
            for (int i = 1; i <= baseChunksCount; i++)
            {
                string assetName = $"Level02/level02_parallax_{i}";
                Texture2D texture = content.Load<Texture2D>(assetName);
                Vector2 position = new Vector2((i - 1) * chunkWidth, 0);
                levelParallaxChunks.Add(new LevelChunk(texture, position));
            }
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
            objCount = gameObjects.Count;

            //TODO: Update here
            for (int i = 0; i < objCount; i++)
            {
                if (gameObjects[i].IsActive) gameObjects[i].Update(gameTime, gameObjects);
            }

            for (int i = 0; i < objCount; i++)
            {
                if (!gameObjects[i].IsActive)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                    objCount--;
                }
            }

            //Console.WriteLine(Singleton.Instance.currentControlState);

            switch (Singleton.Instance.currentControlState)
            {
                case Singleton.ControlState.NormalState:

                    int enemyCount = 0;

                    foreach (GameObject go in gameObjects)
                    {
                        if (go.Name.Equals("Enemy") && go.IsActive)
                        {
                            enemyCount++;
                        }
                    }

                    if (enemyCount <= 0)
                    {
                        currentStageState = StageState.Cleared;
                    }

                    break;

                case Singleton.ControlState.GameOver:

                    currentStageState = StageState.Gameover;

                    break;
            }

            //Console.WriteLine(currentStageState);

            //Handle Input

            switch (currentStageState)
            {
                case StageState.Intro:

                    Singleton.Instance.currentControlState = Singleton.ControlState.PauseState;

                    break;

                case StageState.Normal:

                    if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                    {
                        previousStageState = currentStageState;
                        currentStageState = StageState.Pause;
                        Singleton.Instance.currentControlState = Singleton.ControlState.PauseState;
                    }

                    break;

                case StageState.Pause:

                    if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Enter) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                    {
                        currentStageState = previousStageState;
                        Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
                    }

                    break;

                case StageState.Gameover:

                    if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Enter) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                    {
                        Reset();
                        Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
                        SceneManager.RemoveScreen(this);
                        SceneManager.AddScreen(new MenuScene());
                    }

                    break;

                case StageState.Cleared:

                    if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Enter) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                    {
                        Reset();
                        SceneManager.AddScreen(new MenuScene());
                        SceneManager.RemoveScreen(this);
                    }

                    break;
            }

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                currentStageState = StageState.Pause;
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SceneManager.GraphicsDevice.Clear(ClearOptions.Target, new Color(50, 50, 50), 0, 0);

            SpriteBatch spriteBatch = SceneManager.SpriteBatch;
            Viewport viewport = SceneManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            //Camera2d cam = new Camera2d();

            cam.Zoom = 1.0f;

            Vector2 playerPosition = player.Position;
            MathHelper.Clamp(player.Position.X, 0, LEVEL_WIDTH);
            MathHelper.Clamp(player.Position.Y, 0, LEVEL_HEIGHT);

            switch (Singleton.Instance.currentControlState)
            {
                case Singleton.ControlState.ActiveState:

                    cam.Zoom = 1.2f;
                    cam.Pos = new Vector2(MathHelper.Clamp(playerPosition.X + 300, viewport.Width / 2, LEVEL_WIDTH - viewport.Width / 2), MathHelper.Clamp(playerPosition.Y, viewport.Height / 2, LEVEL_HEIGHT - viewport.Height / 2) + 100);

                    break;

                default:

                    cam.Zoom = 1.0f;
                    cam.Pos = new Vector2(MathHelper.Clamp(playerPosition.X, viewport.Width / 2, LEVEL_WIDTH - viewport.Width / 2), MathHelper.Clamp(playerPosition.Y, viewport.Height / 2, LEVEL_HEIGHT - viewport.Height / 2));

                    break;
            }

            Singleton.Instance.CameraPosition = cam.Pos;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, null, null, cam.get_transformation(SceneManager.GraphicsDevice));

            //Add Black Floor
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, Singleton.MAINSCREEN_HEIGHT, LEVEL_WIDTH, 300), null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            if (curParallax > 250) ascendHelper = false;
            else if (curParallax < 100) ascendHelper = true;

            if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
            {
                if (ascendHelper) curParallax++;
                else curParallax--;
            }

            //Background Object
            //spriteBatch.Draw(levelParallax, new Rectangle(0, 0, levelParallax.Width, levelParallax.Height), null, new Color(Color.White, curParallax), 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            //spriteBatch.Draw(levelBack, new Rectangle(0, 0, levelBack.Width, levelBack.Height), null, new Color(Color.White, 150), 0f, Vector2.Zero, SpriteEffects.None, 1f);
            //spriteBatch.Draw(levelBacklayer, new Rectangle(0, 0, levelParallax.Width, levelParallax.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            DrawLevelLayer(spriteBatch, levelParallaxChunks, 0.4f);
            DrawLevelLayer(spriteBatch, levelBackfoundChunks, 1f);
            DrawLevelLayer(spriteBatch, levelBacklayerChunks, 1f);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }

            //Mask Object
            //spriteBatch.Draw(levelFoundation, new Rectangle(0, 0, levelBase.Width, levelBase.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
            //spriteBatch.Draw(levelBase, new Rectangle(0, 0, levelBase.Width, levelBase.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            //spriteBatch.Draw(levelBackfound, new Rectangle(0, 0, levelParallax.Width, levelParallax.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            DrawLevelLayer(spriteBatch, levelBaseChunks, 0.6f);
            DrawLevelLayer(spriteBatch, levelBackfoundChunks, 0.5f);

            switch (currentStageState)
            {
                case StageState.Intro:

                    if (!waitHelper)
                    {
                        waitHelper = chapAscend = true;
                        currentGameTime = gameTime.TotalGameTime.TotalSeconds;
                    }

                    if (waitHelper)
                    {
                        if (chapAscend && curIntro < 255)
                        {
                            if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                            {
                                curIntro += 1;
                            }
                        }
                        else
                        {
                            chapAscend = false;
                            if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                            {
                                curIntro -= 1;
                            }
                        }
                    }

                    if (gameTime.TotalGameTime.TotalSeconds - currentGameTime >= 8.5f)
                    {
                        currentStageState = StageState.Normal;
                        Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
                    }

                    //Console.WriteLine(currentStageState);

                    spriteBatch.Draw(background, cam.Pos, null, Color.White, 0f, new Vector2(pauseScreen.Width / 2, pauseScreen.Height / 2), 1f, SpriteEffects.None, 0.1f);
                    spriteBatch.Draw(chapText, cam.Pos, null, new Color(Color.White, curIntro), 0f, new Vector2(pauseText.Width / 2, pauseText.Height / 2), 1f, SpriteEffects.None, 0.1f);

                    break;

                case StageState.Normal:

                    switch (Singleton.Instance.currentControlState)
                    {
                        case Singleton.ControlState.NormalState:

                            //TODO: Tint screen when near Active Zone

                            foreach (GameObject g in gameObjects)
                            {
                                if (g.IsActive && g.Name.Equals("ActiveZone") && player.Name.Equals("Player"))
                                {
                                    float deltaPosition = g.Rectangle.Left - player.Position.X;

                                    //Console.WriteLine(deltaPosition);

                                    if (deltaPosition < 400)
                                    {
                                        spriteBatch.Draw(backgroundTexture, new Rectangle((int)cam.Pos.X - Singleton.MAINSCREEN_WIDTH / 2, (int)cam.Pos.Y - Singleton.MAINSCREEN_HEIGHT / 2, Singleton.MAINSCREEN_WIDTH, Singleton.MAINSCREEN_HEIGHT), null, new Color((400 - deltaPosition) / 800, 0, 0, (400 - deltaPosition) / 800), 0f, Vector2.Zero, SpriteEffects.None, 0f);
                                    }
                                }
                            }

                            break;

                        case Singleton.ControlState.ActiveState:

                            //TODO: Fade screen when encounter Active Zone


                            //TODO: Fade out screen when clear Active Zone


                            break;

                    }

                    break;

                case StageState.Pause:

                    if (curPlay > 250) playAscend = false;
                    else if (curPlay < 1) playAscend = true;

                    if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                    {
                        if (playAscend) curPlay += 3;
                        else curPlay -= 5;
                    }

                    spriteBatch.Draw(pauseScreen, cam.Pos, null, Color.White, 0f, new Vector2(pauseScreen.Width / 2, pauseScreen.Height / 2), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(pauseText, cam.Pos, null, new Color(Color.White, curPlay), 0f, new Vector2(pauseText.Width / 2, pauseText.Height / 2), 1f, SpriteEffects.None, 0.5f);

                    break;

                case StageState.Gameover:

                    //TODO: Show Game Over Screen

                    if (curPlay > 250) playAscend = false;
                    else if (curPlay < 1) playAscend = true;

                    if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                    {
                        if (playAscend) curPlay += 3;
                        else curPlay -= 5;
                    }

                    spriteBatch.Draw(gameOver, cam.Pos, null, Color.White, 0f, new Vector2(pauseScreen.Width / 2, pauseScreen.Height / 2), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(returnText, cam.Pos, null, new Color(Color.White, curPlay), 0f, new Vector2(pauseText.Width / 2, pauseText.Height / 2), 1f, SpriteEffects.None, 0.5f);

                    break;

                case StageState.Cleared:

                    //TODO: Show Clear Screen

                    if (curPlay > 250) playAscend = false;
                    else if (curPlay < 1) playAscend = true;

                    if (gameTime.TotalGameTime.Milliseconds % 1 == 0)
                    {
                        if (playAscend) curPlay += 3;
                        else curPlay -= 5;
                    }

                    spriteBatch.Draw(clearScreen, cam.Pos, null, Color.White, 0f, new Vector2(pauseScreen.Width / 2, pauseScreen.Height / 2), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(returnText, cam.Pos, null, new Color(Color.White, curPlay), 0f, new Vector2(pauseText.Width / 2, pauseText.Height / 2), 1f, SpriteEffects.None, 0.5f);

                    break;
            }

            spriteBatch.End();
        }

        private void DrawLevelLayer(SpriteBatch spriteBatch, List<LevelChunk> chunks, float layerDepth)
        {
            foreach (var chunk in chunks)
            {
                // Only draw chunks that are within the camera's view
                if (chunk.Bounds.Intersects(new Rectangle(
                    (int)(cam.Pos.X - SceneManager.GraphicsDevice.Viewport.Width / 2 / cam.Zoom),
                    (int)(cam.Pos.Y - SceneManager.GraphicsDevice.Viewport.Height / 2 / cam.Zoom),
                    SceneManager.GraphicsDevice.Viewport.Width / (int)cam.Zoom,
                    SceneManager.GraphicsDevice.Viewport.Height / (int)cam.Zoom)))
                {
                    spriteBatch.Draw(chunk.Texture, chunk.Position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
                }
            }
        }
        #endregion

        private void Reset()
        {
            Singleton.Instance.gameObjects = new List<GameObject>();
        }
    }
}
