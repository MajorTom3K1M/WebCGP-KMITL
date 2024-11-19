using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    public class GameObject
    {
        protected Texture2D _texture;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Vector2 Velocity;

        public string Name;

        public bool IsActive;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
            IsActive = true;
        }

        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void Reset()
        {

        }

        #region Collision
        //TODO: Collision Detection
        protected bool IsTouchingLeft(GameObject gameObject)
        {
            return this.Rectangle.Right > gameObject.Rectangle.Left &&
                    this.Rectangle.Left < gameObject.Rectangle.Left &&
                    this.Rectangle.Bottom > gameObject.Rectangle.Top &&
                    this.Rectangle.Top < gameObject.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(GameObject gameObject)
        {
            return this.Rectangle.Right > gameObject.Rectangle.Right &&
                    this.Rectangle.Left < gameObject.Rectangle.Right &&
                    this.Rectangle.Bottom > gameObject.Rectangle.Top &&
                    this.Rectangle.Top < gameObject.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(GameObject gameObject)
        {
            return this.Rectangle.Right > gameObject.Rectangle.Left &&
                    this.Rectangle.Left < gameObject.Rectangle.Right &&
                    this.Rectangle.Bottom > gameObject.Rectangle.Top &&
                    this.Rectangle.Top < gameObject.Rectangle.Top;
        }

        protected bool IsTouchingBottom(GameObject gameObject)
        {
            return this.Rectangle.Right > gameObject.Rectangle.Left &&
                    this.Rectangle.Left < gameObject.Rectangle.Right &&
                    this.Rectangle.Bottom > gameObject.Rectangle.Bottom &&
                    this.Rectangle.Top < gameObject.Rectangle.Bottom;
        }
        #endregion
    }
}
