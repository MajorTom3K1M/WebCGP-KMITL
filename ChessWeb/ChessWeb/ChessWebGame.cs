using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace ChessWeb
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ChessWebGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D rect;
        Texture2D bPawn, bRook, bHorse, bBishop, bQueen, bKing;
        Texture2D wPawn, wRook, wHorse, wBishop, wQueen, wKing;

        Token[,] gameBoard = new Token[8, 8];
        Board[,] board = new Board[8, 8];

        class Coordinate
        {
            public int i, j;

            public Coordinate(int i, int j)
            {
                this.i = i;
                this.j = j;
            }
        }

        class Token
        {
            public Type type;
            public Turn color;
            public bool isFirstTurn = false;

            public Token(Type type, Turn color)
            {
                this.type = type;
                this.color = color;
                if (type == Type.Pawn) isFirstTurn = true;
            }

            public Token()
            {
                this.type = Type.Null;
                this.color = Turn.Null;
            }
        }

        class Board
        {
            public BoardColor color;

            public Board(BoardColor color)
            {
                this.color = color;
            }

            public Board()
            {
                this.color = BoardColor.Null;
            }
        }

        enum Type
        {
            Null,
            Pawn,
            Rook,
            Horse,
            Bishop,
            Queen,
            King
        }

        enum Turn
        {
            Null,
            Black,
            White
        }

        enum BoardColor
        {
            Null,
            Movable,
            Capturable,
            Selected,
            Check
        }

        Turn currentTurn;
        MouseState currentState, previousState;
        Token curToken; Coordinate curPos;
        Vector2 relativeMousePostion;

        public ChessWebGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            //graphics.ApplyChanges();
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            // Initialization
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnResize;

            //Current Turn Initialize
            currentTurn = Turn.Black;

            //Token Initialize
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    gameBoard[i, j] = new Token();
                    board[i, j] = new Board(BoardColor.Null);
                }
            }

            gameBoard[0, 0] = new Token(Type.Rook, Turn.Black);
            gameBoard[0, 1] = new Token(Type.Horse, Turn.Black);
            gameBoard[0, 2] = new Token(Type.Bishop, Turn.Black);
            gameBoard[0, 3] = new Token(Type.Queen, Turn.Black);
            gameBoard[0, 4] = new Token(Type.King, Turn.Black);
            gameBoard[0, 5] = new Token(Type.Bishop, Turn.Black);
            gameBoard[0, 6] = new Token(Type.Horse, Turn.Black);
            gameBoard[0, 7] = new Token(Type.Rook, Turn.Black);
            for (int i = 0; i < 8; i++) gameBoard[1, i] = new Token(Type.Pawn, Turn.Black);

            gameBoard[7, 0] = new Token(Type.Rook, Turn.White);
            gameBoard[7, 1] = new Token(Type.Horse, Turn.White);
            gameBoard[7, 2] = new Token(Type.Bishop, Turn.White);
            gameBoard[7, 3] = new Token(Type.King, Turn.White);
            gameBoard[7, 4] = new Token(Type.Queen, Turn.White);
            gameBoard[7, 5] = new Token(Type.Bishop, Turn.White);
            gameBoard[7, 6] = new Token(Type.Horse, Turn.White);
            gameBoard[7, 7] = new Token(Type.Rook, Turn.White);
            for (int i = 0; i < 8; i++) gameBoard[6, i] = new Token(Type.Pawn, Turn.White);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: use this.Content to load your game content here 
            rect = this.Content.Load<Texture2D>("rect");
            bPawn = this.Content.Load<Texture2D>("BlackPawn");
            bRook = this.Content.Load<Texture2D>("BlackRook");
            bHorse = this.Content.Load<Texture2D>("BlackHorse");
            bBishop = this.Content.Load<Texture2D>("BlackBishop");
            bQueen = this.Content.Load<Texture2D>("BlackQueen");
            bKing = this.Content.Load<Texture2D>("BlackKing");
            wPawn = this.Content.Load<Texture2D>("WhitePawn");
            wRook = this.Content.Load<Texture2D>("WhiteRook");
            wHorse = this.Content.Load<Texture2D>("WhiteHorse");
            wBishop = this.Content.Load<Texture2D>("WhiteBishop");
            wQueen = this.Content.Load<Texture2D>("WhiteQueen");
            wKing = this.Content.Load<Texture2D>("WhiteKing");
        }

        [JSInvokable]
        public void UpdateMousePosition(float x, float y)
        {
            if (x >= 0 && y >= 0)
            {
                relativeMousePostion = new Vector2(x, y);
            }
            else
            {
                relativeMousePostion = new Vector2(-100, -100);
            }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            previousState = currentState;
            currentState = Mouse.GetState();

            int x = (int)(relativeMousePostion.X / 75); int j = x;
            int y = (int)(relativeMousePostion.Y / 75); int i = y;

            if (x < 0 || x >= 8 || y < 0 || y >= 8)
            {
                return;
            }


            if (previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed && gameBoard[i, j].color == currentTurn)
            {

                switch (currentTurn)
                {
                    case Turn.Black:
                        {
                            moveHandler(new Coordinate(i, j));
                            break;
                        }
                    case Turn.White:
                        {
                            moveHandler(new Coordinate(i, j));
                            break;
                        }
                }
            }

            if (previousState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed && (board[i, j].color == BoardColor.Movable || board[i, j].color == BoardColor.Capturable))
            {
                switch (currentTurn)
                {
                    case Turn.Black:
                        {
                            move(curToken, curPos, new Coordinate(i, j));
                            currentTurn = Turn.White;
                            break;
                        }
                    case Turn.White:
                        {
                            move(curToken, curPos, new Coordinate(i, j));
                            currentTurn = Turn.Black;
                            break;
                        }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            //TODO: Add your drawing code here
            spriteBatch.Begin();

            //Board Layer
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (board[i, j].color)
                    {
                        case BoardColor.Null:
                            {
                                if ((i + j) % 2 == 0) spriteBatch.Draw(rect, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else spriteBatch.Draw(rect, new Vector2(j * 75, i * 75), null, Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case BoardColor.Movable:
                            {
                                spriteBatch.Draw(rect, new Vector2(j * 75, i * 75), null, Color.LightGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case BoardColor.Selected:
                            {
                                spriteBatch.Draw(rect, new Vector2(j * 75, i * 75), null, Color.SkyBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case BoardColor.Capturable:
                            {
                                spriteBatch.Draw(rect, new Vector2(j * 75, i * 75), null, Color.MistyRose, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                    }
                }
            }

            //Chess Layer
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (gameBoard[i, j].type)
                    {
                        case Type.Pawn:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bPawn, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wPawn, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case Type.Rook:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bRook, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wRook, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case Type.Horse:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bHorse, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wHorse, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case Type.Bishop:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bBishop, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wBishop, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case Type.Queen:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bQueen, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wQueen, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                        case Type.King:
                            {
                                if (gameBoard[i, j].color == Turn.Black) spriteBatch.Draw(bKing, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                else if (gameBoard[i, j].color == Turn.White) spriteBatch.Draw(wKing, new Vector2(j * 75, i * 75), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                break;
                            }
                    }
                }
            }

            spriteBatch.End();

            //graphics.BeginDraw();

            base.Draw(gameTime);
        }

        private void moveHandler(Coordinate currentPos)
        {

            //Cancel other selection
            cancel();

            board[currentPos.i, currentPos.j].color = BoardColor.Selected;
            curToken = gameBoard[currentPos.i, currentPos.j];
            curPos = currentPos;

            switch (gameBoard[currentPos.i, currentPos.j].type)
            {
                case Type.Pawn:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    if (gameBoard[currentPos.i, currentPos.j].isFirstTurn)
                                    {
                                        if (gameBoard[currentPos.i + 2, currentPos.j].type == Type.Null) board[currentPos.i + 2, currentPos.j].color = BoardColor.Movable;
                                    }
                                    if (currentPos.i + 1 < 8)
                                    {
                                        if (gameBoard[currentPos.i + 1, currentPos.j].type == Type.Null) board[currentPos.i + 1, currentPos.j].color = BoardColor.Movable;

                                        //Capture
                                        if (currentPos.j - 1 >= 0 && gameBoard[currentPos.i + 1, currentPos.j - 1].color == Turn.White) board[currentPos.i + 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        if (currentPos.j + 1 < 8 && gameBoard[currentPos.i + 1, currentPos.j + 1].color == Turn.White) board[currentPos.i + 1, currentPos.j + 1].color = BoardColor.Capturable;
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    if (gameBoard[currentPos.i, currentPos.j].isFirstTurn)
                                    {
                                        if (gameBoard[currentPos.i - 2, currentPos.j].type == Type.Null) board[currentPos.i - 2, currentPos.j].color = BoardColor.Movable;
                                    }
                                    if (currentPos.i - 1 >= 0)
                                    {
                                        if (gameBoard[currentPos.i - 1, currentPos.j].type == Type.Null) board[currentPos.i - 1, currentPos.j].color = BoardColor.Movable;

                                        //Capture
                                        if (currentPos.j - 1 >= 0 && gameBoard[currentPos.i - 1, currentPos.j - 1].color == Turn.Black) board[currentPos.i - 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        if (currentPos.j + 1 < 8 && gameBoard[currentPos.i - 1, currentPos.j + 1].color == Turn.Black) board[currentPos.i - 1, currentPos.j + 1].color = BoardColor.Capturable;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Type.Rook:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    bool isUpEnd, isDownEnd, isLeftEnd, isRightEnd;
                                    isUpEnd = isDownEnd = isLeftEnd = isRightEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isUpEnd && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j].type == Type.Null) board[currentPos.i + x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j].color == Turn.White) board[currentPos.i + x, currentPos.j].color = BoardColor.Capturable;
                                                isUpEnd = true;
                                            }
                                        }

                                        if (!isDownEnd && currentPos.i - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j].type == Type.Null) board[currentPos.i - x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j].color == Turn.White) board[currentPos.i - x, currentPos.j].color = BoardColor.Capturable;
                                                isDownEnd = true;
                                            }
                                        }

                                        if (!isLeftEnd && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j - x].type == Type.Null) board[currentPos.i, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j - x].color == Turn.White) board[currentPos.i, currentPos.j - x].color = BoardColor.Capturable;
                                                isLeftEnd = true;
                                            }
                                        }

                                        if (!isRightEnd && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j + x].type == Type.Null) board[currentPos.i, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j + x].color == Turn.White) board[currentPos.i, currentPos.j + x].color = BoardColor.Capturable;
                                                isRightEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    bool isUpEnd, isDownEnd, isLeftEnd, isRightEnd;
                                    isUpEnd = isDownEnd = isLeftEnd = isRightEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isUpEnd && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j].type == Type.Null) board[currentPos.i + x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j].color == Turn.Black) board[currentPos.i + x, currentPos.j].color = BoardColor.Capturable;
                                                isUpEnd = true;
                                            }
                                        }

                                        if (!isDownEnd && currentPos.i - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j].type == Type.Null) board[currentPos.i - x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j].color == Turn.Black) board[currentPos.i - x, currentPos.j].color = BoardColor.Capturable;
                                                isDownEnd = true;
                                            }
                                        }

                                        if (!isLeftEnd && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j - x].type == Type.Null) board[currentPos.i, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j - x].color == Turn.Black) board[currentPos.i, currentPos.j - x].color = BoardColor.Capturable;
                                                isLeftEnd = true;
                                            }
                                        }

                                        if (!isRightEnd && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j + x].type == Type.Null) board[currentPos.i, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j + x].color == Turn.Black) board[currentPos.i, currentPos.j + x].color = BoardColor.Capturable;
                                                isRightEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Type.Horse:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    if (currentPos.i - 2 >= 0)
                                    {
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 2, currentPos.j - 1].type == Type.Null) board[currentPos.i - 2, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 2, currentPos.j - 1].color == Turn.White) board[currentPos.i - 2, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 2, currentPos.j + 1].type == Type.Null) board[currentPos.i - 2, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 2, currentPos.j + 1].color == Turn.White) board[currentPos.i - 2, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i - 1 >= 0)
                                    {
                                        if (currentPos.j - 2 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j - 2].type == Type.Null) board[currentPos.i - 1, currentPos.j - 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j - 2].color == Turn.White) board[currentPos.i - 1, currentPos.j - 2].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 2 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j + 2].type == Type.Null) board[currentPos.i - 1, currentPos.j + 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j + 2].color == Turn.White) board[currentPos.i - 1, currentPos.j + 2].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 1 < 8)
                                    {
                                        if (currentPos.j - 2 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j - 2].type == Type.Null) board[currentPos.i + 1, currentPos.j - 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j - 2].color == Turn.White) board[currentPos.i + 1, currentPos.j - 2].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 2 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j + 2].type == Type.Null) board[currentPos.i + 1, currentPos.j + 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j + 2].color == Turn.White) board[currentPos.i + 1, currentPos.j + 2].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 2 < 8)
                                    {
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 2, currentPos.j - 1].type == Type.Null) board[currentPos.i + 2, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 2, currentPos.j - 1].color == Turn.White) board[currentPos.i + 2, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 2, currentPos.j + 1].type == Type.Null) board[currentPos.i + 2, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 2, currentPos.j + 1].color == Turn.White) board[currentPos.i + 2, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    if (currentPos.i - 2 >= 0)
                                    {
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 2, currentPos.j - 1].type == Type.Null) board[currentPos.i - 2, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 2, currentPos.j - 1].color == Turn.Black) board[currentPos.i - 2, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 2, currentPos.j + 1].type == Type.Null) board[currentPos.i - 2, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 2, currentPos.j + 1].color == Turn.Black) board[currentPos.i - 2, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i - 1 >= 0)
                                    {
                                        if (currentPos.j - 2 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j - 2].type == Type.Null) board[currentPos.i - 1, currentPos.j - 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j - 2].color == Turn.Black) board[currentPos.i - 1, currentPos.j - 2].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 2 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j + 2].type == Type.Null) board[currentPos.i - 1, currentPos.j + 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j + 2].color == Turn.Black) board[currentPos.i - 1, currentPos.j + 2].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 1 < 8)
                                    {
                                        if (currentPos.j - 2 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j - 2].type == Type.Null) board[currentPos.i + 1, currentPos.j - 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j - 2].color == Turn.Black) board[currentPos.i + 1, currentPos.j - 2].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 2 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j + 2].type == Type.Null) board[currentPos.i + 1, currentPos.j + 2].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j + 2].color == Turn.Black) board[currentPos.i + 1, currentPos.j + 2].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 2 < 8)
                                    {
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 2, currentPos.j - 1].type == Type.Null) board[currentPos.i + 2, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 2, currentPos.j - 1].color == Turn.Black) board[currentPos.i + 2, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 2, currentPos.j + 1].type == Type.Null) board[currentPos.i + 2, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 2, currentPos.j + 1].color == Turn.Black) board[currentPos.i + 2, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Type.Bishop:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    bool isLUEnd, isRUEnd, isLDEnd, isRDEnd;
                                    isLUEnd = isRUEnd = isLDEnd = isRDEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isLUEnd && currentPos.i - x >= 0 && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j - x].type == Type.Null) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j - x].color == Turn.White) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLUEnd = true;
                                            }
                                        }

                                        if (!isRUEnd && currentPos.i - x >= 0 && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j + x].type == Type.Null) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j + x].color == Turn.White) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRUEnd = true;
                                            }
                                        }

                                        if (!isLDEnd && currentPos.j - x >= 0 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j - x].type == Type.Null) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j - x].color == Turn.White) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLDEnd = true;
                                            }
                                        }

                                        if (!isRDEnd && currentPos.j + x < 8 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j + x].type == Type.Null) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j + x].color == Turn.White) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRDEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    bool isLUEnd, isRUEnd, isLDEnd, isRDEnd;
                                    isLUEnd = isRUEnd = isLDEnd = isRDEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isLUEnd && currentPos.i - x >= 0 && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j - x].type == Type.Null) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j - x].color == Turn.Black) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLUEnd = true;
                                            }
                                        }

                                        if (!isRUEnd && currentPos.i - x >= 0 && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j + x].type == Type.Null) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j + x].color == Turn.Black) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRUEnd = true;
                                            }
                                        }

                                        if (!isLDEnd && currentPos.j - x >= 0 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j - x].type == Type.Null) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j - x].color == Turn.Black) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLDEnd = true;
                                            }
                                        }

                                        if (!isRDEnd && currentPos.j + x < 8 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j + x].type == Type.Null) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j + x].color == Turn.Black) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRDEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Type.Queen:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    bool isLUEnd, isRUEnd, isLDEnd, isRDEnd, isUpEnd, isDownEnd, isLeftEnd, isRightEnd;
                                    isLUEnd = isRUEnd = isLDEnd = isRDEnd = isUpEnd = isDownEnd = isLeftEnd = isRightEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isLUEnd && currentPos.i - x >= 0 && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j - x].type == Type.Null) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j - x].color == Turn.White) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLUEnd = true;
                                            }
                                        }

                                        if (!isRUEnd && currentPos.i - x >= 0 && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j + x].type == Type.Null) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j + x].color == Turn.White) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRUEnd = true;
                                            }
                                        }

                                        if (!isLDEnd && currentPos.j - x >= 0 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j - x].type == Type.Null) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j - x].color == Turn.White) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLDEnd = true;
                                            }
                                        }

                                        if (!isRDEnd && currentPos.j + x < 8 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j + x].type == Type.Null) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j + x].color == Turn.White) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRDEnd = true;
                                            }
                                        }
                                        if (!isUpEnd && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j].type == Type.Null) board[currentPos.i + x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j].color == Turn.White) board[currentPos.i + x, currentPos.j].color = BoardColor.Capturable;
                                                isUpEnd = true;
                                            }
                                        }

                                        if (!isDownEnd && currentPos.i - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j].type == Type.Null) board[currentPos.i - x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j].color == Turn.White) board[currentPos.i - x, currentPos.j].color = BoardColor.Capturable;
                                                isDownEnd = true;
                                            }
                                        }

                                        if (!isLeftEnd && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j - x].type == Type.Null) board[currentPos.i, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j - x].color == Turn.White) board[currentPos.i, currentPos.j - x].color = BoardColor.Capturable;
                                                isLeftEnd = true;
                                            }
                                        }

                                        if (!isRightEnd && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j + x].type == Type.Null) board[currentPos.i, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j + x].color == Turn.White) board[currentPos.i, currentPos.j + x].color = BoardColor.Capturable;
                                                isRightEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    bool isLUEnd, isRUEnd, isLDEnd, isRDEnd, isUpEnd, isDownEnd, isLeftEnd, isRightEnd;
                                    isLUEnd = isRUEnd = isLDEnd = isRDEnd = isUpEnd = isDownEnd = isLeftEnd = isRightEnd = false;
                                    for (int x = 1; x < 8; x++)
                                    {

                                        if (!isLUEnd && currentPos.i - x >= 0 && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j - x].type == Type.Null) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j - x].color == Turn.Black) board[currentPos.i - x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLUEnd = true;
                                            }
                                        }

                                        if (!isRUEnd && currentPos.i - x >= 0 && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j + x].type == Type.Null) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j + x].color == Turn.Black) board[currentPos.i - x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRUEnd = true;
                                            }
                                        }

                                        if (!isLDEnd && currentPos.j - x >= 0 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j - x].type == Type.Null) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j - x].color == Turn.Black) board[currentPos.i + x, currentPos.j - x].color = BoardColor.Capturable;
                                                isLDEnd = true;
                                            }
                                        }

                                        if (!isRDEnd && currentPos.j + x < 8 && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j + x].type == Type.Null) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j + x].color == Turn.Black) board[currentPos.i + x, currentPos.j + x].color = BoardColor.Capturable;
                                                isRDEnd = true;
                                            }
                                        }
                                        if (!isUpEnd && currentPos.i + x < 8)
                                        {
                                            if (gameBoard[currentPos.i + x, currentPos.j].type == Type.Null) board[currentPos.i + x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i + x, currentPos.j].color == Turn.Black) board[currentPos.i + x, currentPos.j].color = BoardColor.Capturable;
                                                isUpEnd = true;
                                            }
                                        }

                                        if (!isDownEnd && currentPos.i - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i - x, currentPos.j].type == Type.Null) board[currentPos.i - x, currentPos.j].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - x, currentPos.j].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i - x, currentPos.j].color == Turn.Black) board[currentPos.i - x, currentPos.j].color = BoardColor.Capturable;
                                                isDownEnd = true;
                                            }
                                        }

                                        if (!isLeftEnd && currentPos.j - x >= 0)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j - x].type == Type.Null) board[currentPos.i, currentPos.j - x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j - x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j - x].color == Turn.Black) board[currentPos.i, currentPos.j - x].color = BoardColor.Capturable;
                                                isLeftEnd = true;
                                            }
                                        }

                                        if (!isRightEnd && currentPos.j + x < 8)
                                        {
                                            if (gameBoard[currentPos.i, currentPos.j + x].type == Type.Null) board[currentPos.i, currentPos.j + x].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i, currentPos.j + x].color != Turn.Null)
                                            {
                                                if (gameBoard[currentPos.i, currentPos.j + x].color == Turn.Black) board[currentPos.i, currentPos.j + x].color = BoardColor.Capturable;
                                                isRightEnd = true;
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case Type.King:
                    {
                        switch (gameBoard[currentPos.i, currentPos.j].color)
                        {
                            case Turn.Black:
                                {
                                    if (currentPos.i - 1 >= 0)
                                    {
                                        if (gameBoard[currentPos.i - 1, currentPos.j].type == Type.Null) board[currentPos.i - 1, currentPos.j].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i - 1, currentPos.j].color == Turn.White) board[currentPos.i - 1, currentPos.j].color = BoardColor.Capturable;
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j - 1].type == Type.Null) board[currentPos.i - 1, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j - 1].color == Turn.White) board[currentPos.i - 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j + 1].type == Type.Null) board[currentPos.i - 1, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j + 1].color == Turn.White) board[currentPos.i - 1, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 1 < 8)
                                    {
                                        if (gameBoard[currentPos.i + 1, currentPos.j].type == Type.Null) board[currentPos.i + 1, currentPos.j].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i + 1, currentPos.j].color == Turn.White) board[currentPos.i + 1, currentPos.j].color = BoardColor.Capturable;
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j - 1].type == Type.Null) board[currentPos.i + 1, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j - 1].color == Turn.White) board[currentPos.i + 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j + 1].type == Type.Null) board[currentPos.i + 1, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j + 1].color == Turn.White) board[currentPos.i + 1, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.j - 1 >= 0)
                                    {
                                        if (gameBoard[currentPos.i, currentPos.j - 1].type == Type.Null) board[currentPos.i, currentPos.j - 1].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i, currentPos.j - 1].color == Turn.White) board[currentPos.i, currentPos.j - 1].color = BoardColor.Capturable;
                                    }
                                    if (currentPos.j + 1 < 8)
                                    {
                                        if (gameBoard[currentPos.i, currentPos.j + 1].type == Type.Null) board[currentPos.i, currentPos.j + 1].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i, currentPos.j + 1].color == Turn.White) board[currentPos.i, currentPos.j + 1].color = BoardColor.Capturable;
                                    }
                                    break;
                                }
                            case Turn.White:
                                {
                                    if (currentPos.i - 1 >= 0)
                                    {
                                        if (gameBoard[currentPos.i - 1, currentPos.j].type == Type.Null) board[currentPos.i - 1, currentPos.j].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i - 1, currentPos.j].color == Turn.Black) board[currentPos.i - 1, currentPos.j].color = BoardColor.Capturable;
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j - 1].type == Type.Null) board[currentPos.i - 1, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j - 1].color == Turn.Black) board[currentPos.i - 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i - 1, currentPos.j + 1].type == Type.Null) board[currentPos.i - 1, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i - 1, currentPos.j + 1].color == Turn.Black) board[currentPos.i - 1, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.i + 1 < 8)
                                    {
                                        if (gameBoard[currentPos.i + 1, currentPos.j].type == Type.Null) board[currentPos.i + 1, currentPos.j].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i + 1, currentPos.j].color == Turn.Black) board[currentPos.i + 1, currentPos.j].color = BoardColor.Capturable;
                                        if (currentPos.j - 1 >= 0)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j - 1].type == Type.Null) board[currentPos.i + 1, currentPos.j - 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j - 1].color == Turn.Black) board[currentPos.i + 1, currentPos.j - 1].color = BoardColor.Capturable;
                                        }
                                        if (currentPos.j + 1 < 8)
                                        {
                                            if (gameBoard[currentPos.i + 1, currentPos.j + 1].type == Type.Null) board[currentPos.i + 1, currentPos.j + 1].color = BoardColor.Movable;
                                            else if (gameBoard[currentPos.i + 1, currentPos.j + 1].color == Turn.Black) board[currentPos.i + 1, currentPos.j + 1].color = BoardColor.Capturable;
                                        }
                                    }
                                    if (currentPos.j - 1 >= 0)
                                    {
                                        if (gameBoard[currentPos.i, currentPos.j - 1].type == Type.Null) board[currentPos.i, currentPos.j - 1].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i, currentPos.j - 1].color == Turn.Black) board[currentPos.i, currentPos.j - 1].color = BoardColor.Capturable;
                                    }
                                    if (currentPos.j + 1 < 8)
                                    {
                                        if (gameBoard[currentPos.i, currentPos.j + 1].type == Type.Null) board[currentPos.i, currentPos.j + 1].color = BoardColor.Movable;
                                        else if (gameBoard[currentPos.i, currentPos.j + 1].color == Turn.Black) board[currentPos.i, currentPos.j + 1].color = BoardColor.Capturable;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        //Move 'token' to ('x','y')
        private void move(Token token, Coordinate currentPos, Coordinate targetPos)
        {
            gameBoard[currentPos.i, currentPos.j] = new Token();
            gameBoard[targetPos.i, targetPos.j] = token;
            if (token.isFirstTurn) token.isFirstTurn = false;

            cancel();
        }

        private void cancel()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j].color = BoardColor.Null;
                }
            }
            curToken = null;
            curPos = null;

        }

        private void OnResize(object sender, System.EventArgs e)
        {
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
        }
    }
}
