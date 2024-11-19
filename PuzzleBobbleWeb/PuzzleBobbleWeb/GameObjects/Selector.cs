using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleWeb;

namespace PuzzleBobbleWeb
{
    public class Selector : GameObject
    {
        Vector2 mousePosition;
        MouseState mouseClickedState, previousMouseState, mouseState;

        public Texture2D[] SelectorButton;
        Color[] colorDisplay = new Color[3];
        public Color ColorDisplayed = Color.White;
        public Color ColorDisabled = Color.DimGray;
        public Color ColorHovered = Color.DarkGray;

        public int indicatorIndex;

        public Selector(Texture2D texture) : base(texture)
        {
            for (int i = 0; i < colorDisplay.Length; ++i) colorDisplay[i] = ColorDisplayed;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            previousMouseState = mouseClickedState;
            mouseState = Mouse.GetState();
            mousePosition = Singleton.MousePosition;

            for (int i = 0; i < colorDisplay.Length; ++i)
            {
                if (mousePosition.X < Position.X + SelectorButton[i].Width + (i * 65) && mousePosition.X > Position.X + (i * 65) && mousePosition.Y < Position.Y + SelectorButton[i].Height && mousePosition.Y > Position.Y)
                {
                    colorDisplay[i] = ColorHovered;
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) indicatorIndex = i;
                }
            }

            for (int j = 0; j < colorDisplay.Length; ++j)
            {
                if (indicatorIndex == j) colorDisplay[j] = ColorDisplayed;
                else colorDisplay[j] = ColorDisabled;
            }

            base.Update(gameTime, gameObjects);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < colorDisplay.Length; ++i)
            {
                if (indicatorIndex == i) spriteBatch.Draw(SelectorButton[i], new Vector2(Position.X + (i * 65) + 7f, Position.Y - 10f), null, colorDisplay[i], MathHelper.ToRadians(10), Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
                else spriteBatch.Draw(SelectorButton[i], new Vector2(Position.X + (i * 65), Position.Y), null, colorDisplay[i], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            }

            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            this.IsActive = true;
            base.Reset();
        }
    }
}
