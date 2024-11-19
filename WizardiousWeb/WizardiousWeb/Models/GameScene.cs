using System;
using Microsoft.Xna.Framework;

namespace WizardiousWeb
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff
    }

    public abstract class GameScene
    {
        #region Transitions

        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }
        TimeSpan transitionOnTime = TimeSpan.Zero;

        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }
        TimeSpan transitionOffTime = TimeSpan.Zero;

        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;

        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionPosition * 255); }
        }

        public ScreenState ScreenState
        {
            get { return sceneState; }
            protected set { sceneState = value; }
        }

        ScreenState sceneState = ScreenState.TransitionOn;

        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;

        public bool IsActive
        {
            get
            {
                return (sceneState == ScreenState.TransitionOn || sceneState == ScreenState.Active);
            }
        }

        public SceneManager SceneManager
        {
            get { return sceneManager; }
            internal set { sceneManager = value; }
        }

        SceneManager sceneManager;

        #endregion

        #region Initialization

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime)
        {
            if (isExiting)
            {
                // If the scene is going away to die, it should transition off.
                sceneState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the scene.
                    SceneManager.RemoveScreen(this);
                }
            }
            else
            {
                // Otherwise the scene should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    sceneState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    sceneState = ScreenState.Active;
                }
            }
        }

        public virtual void Draw(GameTime gameTime) { }

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        #endregion

        #region Public Methods

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the scene has a zero transition time, remove it immediately.
                SceneManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }

        #endregion
    }
}
