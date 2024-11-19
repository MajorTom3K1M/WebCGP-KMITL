using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WizardiousWeb
{
    public class Singleton
    {
        public const int MAINSCREEN_WIDTH = 1366;
        public const int MAINSCREEN_HEIGHT = 768;

        public const float SPLASH_TIME = 7f;

        public KeyboardState PreviousKey, CurrentKey;

        public List<GameObject> gameObjects = new List<GameObject>();

        public List<Platform> platforms = new List<Platform>();

        public float shootAngle = 75;
        public float shootPower;

        public Vector2 CameraPosition;

        public float Damage = 40f;

        public float Perk1DecreaseCooldown = 0;

        public GameObject[] PlayerItem = new GameObject[4];

        public enum ControlState
        {
            NormalState,
            ActiveState,
            ShopState,
            MessageBox,
            PauseState,
            GameOver
        }

        public ControlState currentControlState;

        private Singleton() { }

        private static Singleton instance;

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
