using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Breakout
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BreakoutGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D paddleTexture, brickTexture, ballTexture;

        List<GameObject> gameObjects;
        int numObject;

        SpriteFont _font;

        int[,] bricks = new int[Singleton.BRICKAREA_COLUMN, Singleton.BRICKAREA_ROW];
        int headerOffset = Singleton.HEADER * Singleton.SIZE;

        public BreakoutGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Initialization
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnResize;

            graphics.PreferredBackBufferWidth = Singleton.WIDTH * Singleton.SIZE;
            graphics.PreferredBackBufferHeight = Singleton.HEIGHT * Singleton.SIZE;
            graphics.ApplyChanges();

            Singleton.Instance.currentGameState = Singleton.GameState.Start;

            base.Initialize();
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Singleton.WIDTH * Singleton.SIZE;
            graphics.PreferredBackBufferHeight = Singleton.HEIGHT * Singleton.SIZE;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Paddle
            int paddleWidth = Singleton.PADDLE_WIDTH * Singleton.SIZE;
            int paddleHeight = Singleton.PADDLE_HEIGHT * Singleton.SIZE;

            paddleTexture = new Texture2D(graphics.GraphicsDevice, paddleWidth, paddleHeight);
            Color[] dataPaddle = new Color[paddleWidth * paddleHeight];
            for (int i = 0; i < dataPaddle.Length; ++i) dataPaddle[i] = Color.LightGray;
            paddleTexture.SetData(dataPaddle);

            //Brick
            int brickWidth = Singleton.BRICK_WIDTH * Singleton.SIZE;
            int brickHeight = Singleton.BRICK_HEIGHT * Singleton.SIZE;

            brickTexture = new Texture2D(graphics.GraphicsDevice, brickWidth, brickHeight);
            Color[] dataBrick = new Color[brickWidth * brickHeight];
            for (int i = 0; i < dataBrick.Length; ++i) dataBrick[i] = Color.LightGray;
            brickTexture.SetData(dataBrick);

            //Ball
            ballTexture = this.Content.Load<Texture2D>("Ball");

            //Initialize gameObject List
            gameObjects = new List<GameObject>();

            //Add Brick
            for (int i = 0; i < Singleton.BRICKAREA_ROW; i++)
            {
                for (int j = 1; j < Singleton.BRICKAREA_COLUMN - 1; j++)
                {
                    gameObjects.Add(
                        new Brick(brickTexture)
                        {
                            Name = "Brick",
                            Position = new Vector2(j * brickWidth, i * brickHeight + Singleton.HEADER * Singleton.SIZE)
                        }
                    );
                }
            }

            //Add Paddle
            gameObjects.Add(
                new Paddle(paddleTexture)
                {
                    Name = "Paddle",
                    Position = new Vector2(400 - (Singleton.SIZE * Singleton.PADDLE_WIDTH / 2), (Singleton.HEIGHT - Singleton.FOOTER) * Singleton.SIZE),
                    Left = Keys.A,
                    Right = Keys.D
                }
            );

            //Add Ball
            gameObjects.Add(
                new Ball(ballTexture)
                {
                    Name = "Ball",
                    Position = new Vector2(400 - (ballTexture.Width / 2), (Singleton.HEIGHT - Singleton.FOOTER) * Singleton.SIZE - paddleTexture.Height)
                }
            );

            _font = Content.Load<SpriteFont>("GameFont");

            Reset();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Singleton.Instance.CurrentKey = Keyboard.GetState();

            numObject = gameObjects.Count;

            switch (Singleton.Instance.currentGameState)
            {
                case Singleton.GameState.Start:
                    {
                        for (int i = 0; i < numObject; i++)
                        {
                            if (gameObjects[i].IsActive) gameObjects[i].Update(gameTime, gameObjects);
                        }

                        if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.IsKeyDown(Keys.Space))
                        {
                            //press space to start
                            Singleton.Instance.currentGameState = Singleton.GameState.Playing;
                        }

                        //Player Lost
                        if (Singleton.Instance.Life < 0)
                        {
                            Singleton.Instance.currentPlayerStatus = Singleton.PlayerStatus.Lost;
                            Singleton.Instance.currentGameState = Singleton.GameState.End;
                        }

                        break;
                    }
                case Singleton.GameState.Playing:
                    {
                        for (int i = 0; i < numObject; i++)
                        {
                            if (gameObjects[i].IsActive) gameObjects[i].Update(gameTime, gameObjects);
                        }

                        //Player won by collide all brick
                        if (Singleton.Instance.brickCount <= 0)
                        {
                            Singleton.Instance.currentPlayerStatus = Singleton.PlayerStatus.Won;
                            Singleton.Instance.currentGameState = Singleton.GameState.End;
                        }

                        break;
                    }
                case Singleton.GameState.End:
                    {
                        //TODO: Show Game Over Text
                        switch (Singleton.Instance.currentPlayerStatus)
                        {
                            case Singleton.PlayerStatus.Lost:
                                {

                                    break;
                                }
                            case Singleton.PlayerStatus.Won:
                                {

                                    break;
                                }
                        }

                        //TODO: Press any key to Play again
                        if (!Singleton.Instance.CurrentKey.Equals(Singleton.Instance.PreviousKey) && Singleton.Instance.CurrentKey.GetPressedKeys().Length > 0)
                        {
                            //any keys pressed to start
                            Reset();
                            Singleton.Instance.currentGameState = Singleton.GameState.Start;
                        }

                        break;
                    }
            }

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Set BG
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //Draw All Active GameObject
            for (int i = 0; i < numObject; i++)
            {
                if (gameObjects[i].IsActive) gameObjects[i].Draw(spriteBatch);
            }

            Vector2 fontSize = _font.MeasureString("Life: " + Singleton.Instance.Life.ToString());
            if (Singleton.Instance.Life >= 0) spriteBatch.DrawString(_font, "Life: " + Singleton.Instance.Life.ToString(), new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 4, 20), Color.White);
            else spriteBatch.DrawString(_font, "Life: Game Over", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 4, 20), Color.White);

            _font.MeasureString("Brick Left: " + Singleton.Instance.brickCount.ToString());
            spriteBatch.DrawString(_font, "Brick Left: " + Singleton.Instance.brickCount.ToString(), new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2 + 120, 20), Color.White);

            switch (Singleton.Instance.currentGameState)
            {
                case Singleton.GameState.Start:
                    {
                        fontSize = _font.MeasureString("Press space to play");
                        spriteBatch.DrawString(_font, "Press space to play", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2, (Singleton.HEIGHT * Singleton.SIZE - fontSize.Y) / 2 + 80), Color.White);
                        fontSize = _font.MeasureString("Press A to move left, Press D to move right");
                        spriteBatch.DrawString(_font, "Press A to move left, Press D to move right", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2, (Singleton.HEIGHT * Singleton.SIZE - fontSize.Y) / 2 + 120), Color.White);
                        break;
                    }

                case Singleton.GameState.Playing:
                    {
                        break;
                    }
                case Singleton.GameState.End:
                    {
                        if (Singleton.Instance.currentPlayerStatus == Singleton.PlayerStatus.Lost)
                        {
                            fontSize = _font.MeasureString("Game Over");
                            spriteBatch.DrawString(_font, "Game Over", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2, (Singleton.HEIGHT * Singleton.SIZE - fontSize.Y) / 2), Color.White);
                        }
                        else if (Singleton.Instance.currentPlayerStatus == Singleton.PlayerStatus.Won)
                        {
                            fontSize = _font.MeasureString("Game Won");
                            spriteBatch.DrawString(_font, "Game Won", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2, (Singleton.HEIGHT * Singleton.SIZE - fontSize.Y) / 2), Color.White);
                        }

                        fontSize = _font.MeasureString("Press any key to restart");
                        spriteBatch.DrawString(_font, "Press any key to restart", new Vector2((Singleton.WIDTH * Singleton.SIZE - fontSize.X) / 2, (Singleton.HEIGHT * Singleton.SIZE - fontSize.Y) / 2 + 80), Color.White);
                        break;
                    }
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            //Reset all GameObject
            foreach (GameObject obj in gameObjects)
            {
                obj.Reset();
            }

            Singleton.Instance.Life = 3;
            Singleton.Instance.brickCount = 0;

            foreach (GameObject obj in gameObjects)
            {
                if (obj.Name.Equals("Brick")) Singleton.Instance.brickCount++;
            }
        }
    }
}
