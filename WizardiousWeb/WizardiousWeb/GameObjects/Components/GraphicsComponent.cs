using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WizardiousWeb
{
    public class GraphicsComponent : Component
    {
        protected Dictionary<string, Animation> _animations;
        protected AnimationManager _animationManager;
        protected Texture2D _texture;
        public Color color = Color.White;

        public GraphicsComponent(GameScene currentScene)
        {
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            if (_animationManager != null)
            {
                _animationManager.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            if (_animationManager == null)
            {
                spriteBatch.Draw(_texture,
                parent.Position,
                parent.Viewport,
                color,
                parent.Rotation,
                parent.Viewport.Center.ToVector2(),
                parent.Scale,
                SpriteEffects.None,
                0.55f);
            }
            else
            {
                _animationManager.Draw(spriteBatch, parent.Position, parent.Rotation, parent.Scale);
            }
        }

        public override void Reset()
        {
            if (_animations != null)
            {
                _animationManager = new AnimationManager(_animations.First().Value);
            }
        }
    }
}
