using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.ComponentModel;

namespace WizardiousWeb
{
    public class GameObject : ICloneable
    {
        #region PUBLIC_VARIABLES

        public Dictionary<string, SoundEffectInstance> SoundEffects;

        public string Name, SubName;
        //SubName used to define Object type or other label
        public int Qty;

        public Vector2 Position;

        public float Rotation;
        public Vector2 Scale;

        public Vector2 Velocity;
        public Vector2 Acceleration;

        public bool IsActive;

        //Shooting Parameter
        public float ShootAngle;
        public float ShootPower;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - Viewport.Width / 2,
                                    (int)Position.Y - Viewport.Height / 2,
                                    Viewport.Width,
                                    Viewport.Height);
            }
        }

        public Rectangle Viewport;

        public InputComponent Input;
        public PhysicsComponent Physics;
        public GraphicsComponent Graphics;

        #endregion

        public GameObject(InputComponent input, PhysicsComponent physics, GraphicsComponent graphics)
        {
            Input = input;
            Physics = physics;
            Graphics = graphics;

            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            IsActive = true;
        }

        public void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Input != null) Input.Update(gameTime, gameObjects, this);
            if (Physics != null) Physics.Update(gameTime, gameObjects, this);
            if (Graphics != null) Graphics.Update(gameTime, gameObjects, this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Graphics != null) Graphics.Draw(spriteBatch, this);
        }

        public void Reset()
        {
            if (Input != null) Input.Reset();
            if (Physics != null) Physics.Reset();
            if (Graphics != null) Graphics.Reset();
        }

        public void SendMessage(int message, Component sender)
        {
            //to broadcast message to all components
            //using code to know message 
            if (Input != null) Input.ReceiveMessage(message, sender);
            if (Physics != null) Physics.ReceiveMessage(message, sender);
            if (Graphics != null) Graphics.ReceiveMessage(message, sender);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
