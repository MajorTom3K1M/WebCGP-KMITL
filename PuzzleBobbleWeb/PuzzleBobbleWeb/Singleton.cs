using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBobbleWeb
{
    class Singleton
    {
        public const int BOBBLE_SIZE = 50;

        public const int GAMESCREEN_WIDTH = BOBBLE_SIZE * 8;
        public const int GAMESCREEN_HEIGHT = BOBBLE_SIZE * 12;

        public const int MAINSCREEN_WIDTH = GAMESCREEN_WIDTH * 2;
        public const int MAINSCREEN_HEIGHT = GAMESCREEN_HEIGHT;

        public const float SPLASH_TIME = 7f;

        public enum GameScene
        {
            TitleScene,     //Splash Screen
            MenuScene,      //Menu
            OptionScene,    //Option like difficulty
            GameScene,      //Main Game Scene
            ExtrasScene     //Show Player Statistic on each play
        }

        public GameScene currentGameScene;

        public enum GameSceneState
        {
            None,           //Not on GameScene
            Start,          //First State on Game
            Playing,        //Playing State
            End             //Game Over with showing Player Score and otherwise
        }

        public GameSceneState currentGameState;

        public enum PlayerStatus
        {
            None,           //Not on End GameSceneState
            Won,            //Showing if Player Won
            Lost            //Showing if Player Lost
        }

        public PlayerStatus currentPlayerStatus;

        public int score;
        public int ceilingLevel;
        public int ceilingTime = 30;
        public bool IsCeilingDowing;
        public int turnCounter;

        public int colorVariety = 4;

        public float bgmSound = 0.9f;
        public float sfxSound = 0.9f;

        public int bombTime = 7;

        public bool IsBlindMode;

        //public bool IsHovered = false;

        public SpriteFont gameFont;

        public static Vector2 MousePosition { get; private set; } = new Vector2(-100, -100);

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

        public static void SetMousePosition(Vector2 position)
        {
            MousePosition = position;
        }
    }
}
