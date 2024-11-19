using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Singleton
    {
        //Global variable initializing
        public const int SIZE = 25;

        public const int WIDTH = 32;
        public const int HEIGHT = 24;

        public int Score;

        public enum GameState
        {
            Start,
            Playing,
            End
        }

        public GameState currentGameState;

        public enum PlayerStatus
        {
            None,
            Won,
            Lost
        }

        public PlayerStatus currentPlayerStatus;

        public const int PADDLE_WIDTH = 8;
        public const int PADDLE_HEIGHT = 1;

        public const int BRICK_WIDTH = 2;
        public const int BRICK_HEIGHT = 1;

        public const int HEADER = 4;
        public const int FOOTER = 2;

        public const int BRICKAREA_COLUMN = 16;
        public const int BRICKAREA_ROW = 3;

        public int brickCount;

        public KeyboardState CurrentKey, PreviousKey;

        public int Life;

        private static Singleton instance;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
