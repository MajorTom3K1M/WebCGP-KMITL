using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WizardiousWeb
{
    public class PlayerGraphicsComponent : GraphicsComponent
    {
        private bool jump = true;
        private bool isTouchingShopKepper = false;
        public float health = 100f;
        public int chargeBar = 0;
        public bool charging = false;
        public int chargeToggle;

        public Texture2D healthTexture;
        public Rectangle healthRectangle;

        public Texture2D chargeTexture;
        public Vector2 chargeposition;
        public Rectangle rectangle;

        public Texture2D arrowTexture;
        public Vector2 arrowPosition;

        public Texture2D bgBar;
        public Vector2 bgPostion;
        public Rectangle bgRectangle;

        public Texture2D shopTexture;
        public Rectangle shopRect;
        public Vector2 shopPosition;

        public Texture2D selectTexture;
        public Vector2 selectPosition;

        public Texture2D selectExitTexture;
        public Vector2 selectExitPosition;

        public int moveSelectorX = 0;
        public int moveSelectorY = 0;
        //public Rectangle bgRectangle;

        SpriteFont font;

        public Texture2D linearPenetrateTexture;
        public Texture2D linearTexture;
        public Texture2D doubleShotTexture;

        public Texture2D potionSlot;
        public Texture2D doubleDamageSlot;

        Texture2D healthParallax;

        public float angle;
        public float power;

        public enum Skill
        {
            Normal,
            Linear,
            LinearPenetrate,
            DoubleShot
        }

        private float turnCount1 = 0;
        private float turnCount2 = 0;
        private float turnCount3 = 0;

        private Skill currentSkill;
        private Skill[] SkillList = new Skill[3];
        private bool[] skillAvailable = new bool[3];

        public PlayerGraphicsComponent(GameScene currentScene) : base(currentScene)
        {
            Texture2D playerTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Actor_Spritesheet");

            _animations = new Dictionary<string, Animation>()
                            {
                                { "Alive", new Animation(playerTexture, new Rectangle(0, 0, 64, 64), 1) },
                                { "Left", new Animation(playerTexture, new Rectangle(0, 0, 64, 64), 1)},
                                { "Right", new Animation(playerTexture, new Rectangle(448, 0, 512, 64), 1)},
                                { "Dead", new Animation(playerTexture, new Rectangle(448, 448, 512, 512), 1) }
                            };

            _texture = playerTexture;

            _animationManager = new AnimationManager(_animations.First().Value);

            healthTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Health");
            chargeTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/ChargeBar");
            arrowTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/ArrowHead");
            bgBar = currentScene.SceneManager.Content.Load<Texture2D>("Images/BgBar");
            shopTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Store");
            selectTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Select1");
            selectExitTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/SelectExit");
            linearPenetrateTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/LinearPenetrate");
            linearTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/Linear");
            doubleShotTexture = currentScene.SceneManager.Content.Load<Texture2D>("Images/DoubleShot");
            potionSlot = currentScene.SceneManager.Content.Load<Texture2D>("Images/PotionSlot");
            doubleDamageSlot = currentScene.SceneManager.Content.Load<Texture2D>("Images/DoubleDamageSlot");
            font = currentScene.SceneManager.Content.Load<SpriteFont>("Fonts/GameFont");

            healthParallax = currentScene.SceneManager.Content.Load<Texture2D>("health_parallax");

            angle = 75;

            rectangle = new Rectangle(0, 0, 0, chargeTexture.Height);
            bgRectangle = new Rectangle(0, 0, bgBar.Width, bgBar.Height);
            shopRect = new Rectangle(0, 0, shopTexture.Width, shopTexture.Height);
            arrowPosition = new Vector2(50, 50);
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            int healthParallaxHelper = (int)(255 - (health * 2.55));
            Console.WriteLine(healthParallaxHelper);

            switch (Singleton.Instance.currentControlState)
            {
                case Singleton.ControlState.NormalState:

                    spriteBatch.Draw(healthParallax, Singleton.Instance.CameraPosition, null, new Color(Color.DarkRed, healthParallaxHelper), 0f, new Vector2(healthParallax.Width / 2, healthParallax.Height / 2), 1f, SpriteEffects.None, 0.4f);

                    break;


                case Singleton.ControlState.ActiveState:
                    spriteBatch.Draw(arrowTexture, arrowPosition, null, Color.White, MathHelper.ToRadians(angle), new Vector2(arrowTexture.Width / 2, 143), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(chargeTexture, chargeposition, rectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                    if (skillAvailable[0])
                    {
                        spriteBatch.DrawString(font, "Skill", new Vector2(parent.Position.X - 250, 220), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        spriteBatch.Draw(linearTexture, new Vector2(parent.Position.X - 250, 250), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        if (turnCount1 > 0)
                        {
                            Vector2 fontSize1 = font.MeasureString(turnCount1.ToString());
                            spriteBatch.DrawString(font, turnCount1.ToString(), new Vector2(parent.Position.X - 230, 262), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            spriteBatch.Draw(linearTexture, new Vector2(parent.Position.X - 250, 250), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                        else
                        {
                            Vector2 fontSize = font.MeasureString("Q");
                            spriteBatch.DrawString(font, "Q", new Vector2(parent.Position.X - 247, 250), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                    }
                    if (skillAvailable[1])
                    {
                        spriteBatch.Draw(linearPenetrateTexture, new Vector2(parent.Position.X - 250, 300), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        if (turnCount2 > 0)
                        {
                            Vector2 fontSize1 = font.MeasureString(turnCount2.ToString());
                            spriteBatch.DrawString(font, turnCount2.ToString(), new Vector2(parent.Position.X - 230, 312), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            spriteBatch.Draw(linearPenetrateTexture, new Vector2(parent.Position.X - 250, 300), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                        else
                        {
                            Vector2 fontSize = font.MeasureString("W");
                            spriteBatch.DrawString(font, "W", new Vector2(parent.Position.X - 247, 300), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                    }
                    if (skillAvailable[2])
                    {
                        spriteBatch.Draw(doubleShotTexture, new Vector2(parent.Position.X - 250, 350), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        if (turnCount3 > 0)
                        {
                            Vector2 fontSize1 = font.MeasureString(turnCount3.ToString());
                            spriteBatch.DrawString(font, turnCount3.ToString(), new Vector2(parent.Position.X - 230, 362), Color.Red, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            spriteBatch.Draw(doubleShotTexture, new Vector2(parent.Position.X - 250, 350), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                        else
                        {
                            Vector2 fontSize = font.MeasureString("E");
                            spriteBatch.DrawString(font, "E", new Vector2(parent.Position.X - 247, 350), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        }
                    }
                    switch (currentSkill)
                    {
                        case Skill.Linear:
                            spriteBatch.Draw(linearTexture, new Vector2(parent.Position.X - 250, 250), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            break;
                        case Skill.LinearPenetrate:
                            spriteBatch.Draw(linearPenetrateTexture, new Vector2(parent.Position.X - 250, 300), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            break;
                        case Skill.DoubleShot:
                            spriteBatch.Draw(doubleShotTexture, new Vector2(parent.Position.X - 250, 350), null, Color.Black * 0.5f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                            break;
                    }

                    spriteBatch.DrawString(font, "ItemSlot", new Vector2(parent.Position.X - 190, 175), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                    if (Singleton.Instance.PlayerItem[0] != null && Singleton.Instance.PlayerItem[0].Qty > 0)
                    {
                        spriteBatch.DrawString(font, "1", new Vector2(parent.Position.X - 185, 200), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        spriteBatch.DrawString(font, "x" + Singleton.Instance.PlayerItem[0].Qty, new Vector2(parent.Position.X - 170, 220), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        if (Singleton.Instance.PlayerItem[0].Name.Equals("Potion"))
                        {
                            spriteBatch.Draw(potionSlot, new Vector2(parent.Position.X - 190, 200), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        }
                        else if (Singleton.Instance.PlayerItem[0].Name.Equals("DoubleDamage"))
                        {
                            spriteBatch.Draw(doubleDamageSlot, new Vector2(parent.Position.X - 190, 400), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        }
                    }

                    if (Singleton.Instance.PlayerItem[1] != null && Singleton.Instance.PlayerItem[1].Qty > 0)
                    {
                        spriteBatch.DrawString(font, "2", new Vector2(parent.Position.X - 135, 200), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        spriteBatch.DrawString(font, "x" + Singleton.Instance.PlayerItem[1].Qty, new Vector2(parent.Position.X - 120, 220), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
                        if (Singleton.Instance.PlayerItem[1].Name.Equals("Potion"))
                        {
                            spriteBatch.Draw(potionSlot, new Vector2(parent.Position.X - 140, 200), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        }
                        else
                        if (Singleton.Instance.PlayerItem[1].Name.Equals("DoubleDamage"))
                        {
                            spriteBatch.Draw(doubleDamageSlot, new Vector2(parent.Position.X - 140, 200), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
                        }
                    }

                    spriteBatch.Draw(healthParallax, Singleton.Instance.CameraPosition, null, new Color(Color.DarkRed, healthParallaxHelper), 0f, new Vector2(healthParallax.Width / 2, healthParallax.Height / 2), 1f, SpriteEffects.None, 0.4f);

                    break;

                case Singleton.ControlState.ShopState:

                    if (moveSelectorY >= 305)
                    {
                        spriteBatch.Draw(selectExitTexture, selectExitPosition, null, Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);
                    }
                    else
                    {
                        spriteBatch.Draw(selectTexture, selectPosition, null, Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);
                    }

                    spriteBatch.Draw(shopTexture, shopPosition, shopRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);

                    break;
            }

            base.Draw(spriteBatch, parent);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            chargeposition = new Vector2(parent.Position.X - 100, parent.Position.Y + parent.Rectangle.Height + 2);

            switch (Singleton.Instance.currentControlState)
            {
                case Singleton.ControlState.NormalState:
                    if (jump == true)
                    {
                        float i = 1;
                        parent.Velocity.Y += 0.15f * i;
                    }

                    if (parent.Position.Y + _texture.Height >= 1250)
                    {
                        parent.SendMessage(101, this);
                        jump = false;
                    }

                    break;
                case Singleton.ControlState.ActiveState:

                    angle = MathHelper.Clamp(angle, 15, 90);
                    arrowPosition = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                    chargeposition = new Vector2((parent.Position.X - 100), (parent.Position.Y + 35));

                    //CHARGE
                    if (currentSkill == Skill.Normal || currentSkill == Skill.DoubleShot)
                    {
                        if (charging)
                        {
                            if (rectangle.Width <= 0)
                            {
                                chargeToggle = 7;
                            }
                            else if (rectangle.Width >= chargeTexture.Width)
                            {
                                chargeToggle = -7;
                            }
                            rectangle.Width = rectangle.Width + chargeToggle;
                            parent.ShootPower = rectangle.Width;
                        }
                    }

                    break;

                case Singleton.ControlState.ShopState:
                    if (moveSelectorX >= 450 && moveSelectorY >= 300)
                    {
                        moveSelectorX = MathHelper.Clamp(moveSelectorX, 0, 450);
                        moveSelectorY = MathHelper.Clamp(moveSelectorY, 0, 450);
                    }
                    else
                    {
                        moveSelectorX = MathHelper.Clamp(moveSelectorX, 0, 450);
                        moveSelectorY = MathHelper.Clamp(moveSelectorY, 0, 300);
                    }
                    if (parent.Position.X < Singleton.MAINSCREEN_WIDTH / 2)
                    {
                        shopPosition = new Vector2(350, 100);
                        selectPosition = new Vector2(420 + moveSelectorX, 150 + moveSelectorY);
                        selectExitPosition = new Vector2(865, 590);
                    }
                    else
                    {
                        shopPosition = new Vector2(parent.Position.X - 333, 100);
                        selectPosition = new Vector2((parent.Position.X - 263) + moveSelectorX, 150 + moveSelectorY);
                        selectExitPosition = new Vector2(parent.Position.X + 182, 590);
                    }
                    break;
            }

            base.Update(gameTime, gameObjects, parent);
        }

        public override void ReceiveMessage(int message, Component sender)
        {
            base.ReceiveMessage(message, sender);
            if (sender.Equals(this)) return;

            if (message == 101)
            {
                jump = false;
            }
            else if (message == 100)
            {
                jump = true;
            }
            else if (message == 200)
            {
                health -= 10;
            }
            else if (message == 900)
            {
                charging = true;
            }
            else if (message == 901)
            {
                charging = false;
            }
            else if (message == 800)
            {
                angle++;
            }
            else if (message == 801)
            {
                angle--;
            }
            else if (message == 700)
            {
                //Singleton.Instance.currentControlState = Singleton.ControlState.ActiveState;
            }
            else if (message == 500)
            {
                rectangle.Width = 0;
            }
            else if (message == 702)
            {
                Singleton.Instance.currentControlState = Singleton.ControlState.ShopState;
            }
            else if (message == 902)
            {
                isTouchingShopKepper = true;
            }
            else if (message == 903)
            {
                isTouchingShopKepper = false;
            }
            else if (message == 300)
            {
                moveSelectorX += 150;
            }
            else if (message == 301)
            {
                moveSelectorX -= 150;
            }
            else if (message == 302)
            {
                moveSelectorY += 150;
            }
            else if (message == 303)
            {
                moveSelectorY -= 150;
            }
            else if (message == 400)
            {
                currentSkill = Skill.Linear;
            }
            else if (message == 401)
            {
                currentSkill = Skill.LinearPenetrate;
            }
            else if (message == 402)
            {
                currentSkill = Skill.Normal;
            }
            else if (message == 403)
            {
                currentSkill = Skill.DoubleShot;
            }
            else if (message == 551)
            {
                skillAvailable[0] = true;
            }
            else if (message == 552)
            {
                skillAvailable[1] = true;
            }
            else if (message == 553)
            {
                skillAvailable[2] = true;
            }
            else if (message == 554)
            {

            }
            else if (message == 654)
            {
                turnCount1--;
            }
            else if (message == 655)
            {
                turnCount2--;
            }
            else if (message == 656)
            {
                turnCount3--;
            }
            else if (message == 754)
            {
                turnCount1 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
            }
            else if (message == 755)
            {
                turnCount2 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
            }
            else if (message == 756)
            {
                turnCount3 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
            }
            else if (message == 854)
            {
                health += 10;
            }
            else if (message == 855)
            {
                health = 100;
            }


            switch (message)
            {
                case 1001:
                    //_animationManager.Play(_animations["Right"]);
                    break;
                case 1000:
                    _animationManager.Play(_animations["Alive"]);
                    break;
                case 1101:
                    health++;
                    break;
                case 2001:
                    turnCount1--;
                    break;
                case 2002:
                    turnCount2--;
                    break;
                case 2003:
                    turnCount3--;
                    break;
            }
        }
    }
}
