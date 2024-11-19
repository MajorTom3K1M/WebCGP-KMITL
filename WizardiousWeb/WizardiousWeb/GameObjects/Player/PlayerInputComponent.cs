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
    public class PlayerInputComponent : InputComponent
    {
        //private float ACCELERATIONSEN = 300;
        private bool jump = true;
        private bool charging = false;
        private bool isTouchingShopKepper = false;
        private bool skillSelected = false;
        private bool isWaitState = false;
        private float timer = 0;
        private float countshot = 0;
        private bool isWait = false;
        private bool finishDoubleShoot = false;
        private bool isCount = false;
        private bool isFirstTurn = true;

        public bool Perk1;
        public bool Perk2;
        public bool Perk3;

        public float health = 100f;

        private bool Potion;

        private float turnCount1 = 0;
        private float turnCount2 = 0;
        private float turnCount3 = 0;

        private GameObject catapultBullet;
        private GameObject linearBullet;

        private Rectangle rectangle;
        private float angle = 75;
        public int moveSelectorX = 0;
        public int moveSelectorY = 0;

        GameObject newBullet;

        public enum Skill
        {
            Normal,
            Linear,
            LinearPenetrate,
            DoubleShot
        }

        private Skill currentSkill;

        public enum ActiveState
        {
            Wait,
            Attack,
            WaitForWait,
            Died
        }

        public ActiveState currentState;
        private Skill[] SkillList = new Skill[3];
        private bool[] skillAvailable = new bool[3];

        private bool TimerHelper;
        private double curTime;

        public PlayerInputComponent(GameScene currentScene) : base(currentScene)
        {
            //For Turn based Mechanics
            TurnSpeed = 3;

            InputList["Left"] = Keys.Left;
            InputList["Right"] = Keys.Right;
            InputList["Up"] = Keys.Up;
            InputList["Down"] = Keys.Down;
            InputList["SpaceBar"] = Keys.Space;

            InputList["Shoot"] = Keys.Z;
            InputList["ArrowUp"] = Keys.S;
            InputList["ArrowDown"] = Keys.X;

            //Skill Test
            InputList["Skill1"] = Keys.Q;
            InputList["Skill2"] = Keys.W;
            InputList["Skill3"] = Keys.E;

            //Item Use 
            InputList["1"] = Keys.D1;
            InputList["2"] = Keys.D2;
            InputList["3"] = Keys.D3;
            catapultBullet = new GameObject(null, new CatapultPhysicsComponent(currentScene), new CatapultGraphicsComponent(currentScene));
            linearBullet = new GameObject(null, new Skill1PhysicsComponent(currentScene), new Skill1GraphicsComponent(currentScene));
        }

        public override void Draw(SpriteBatch spriteBatch, GameObject parent)
        {
            base.Draw(spriteBatch, parent);
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {

            if (health <= 0 || parent.Position.Y > Singleton.MAINSCREEN_HEIGHT)
            {
                Console.WriteLine("Died");
                currentState = ActiveState.Died;
                Singleton.Instance.currentControlState = Singleton.ControlState.GameOver;
            }
            else
            {
                switch (Singleton.Instance.currentControlState)
                {
                    case Singleton.ControlState.PauseState:

                        //TODO: Do nothing

                        break;

                    case Singleton.ControlState.NormalState:

                        parent.Position += parent.Velocity;

                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Right"]))
                        {
                            parent.Velocity.X = 3f;
                        }
                        else if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Left"]))
                        {
                            parent.Velocity.X = -3f;
                            parent.SendMessage(1001, this);
                        }
                        else
                        {
                            parent.Velocity.X = 0f;
                            parent.SendMessage(1000, this);
                        }

                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Up"]) && jump == false)
                        {
                            parent.Position.Y -= 10f;
                            parent.Velocity.Y = -5f;
                            parent.SendMessage(100, this);
                            jump = true;
                        }
                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["SpaceBar"]) &&
                            Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey && isTouchingShopKepper)
                        {
                            parent.SendMessage(702, this);
                            Singleton.Instance.currentControlState = Singleton.ControlState.ShopState;
                        }

                        //Regen while not encounter
                        if (gameTime.TotalGameTime.Milliseconds % 1000 == 0)
                        {
                            if (health < 100)
                            {
                                health++;
                                parent.SendMessage(1101, this);
                            }
                        }

                        break;

                    //ISACTIVEMODE INPUT
                    case Singleton.ControlState.ActiveState:

                        switch (currentState)
                        {
                            case ActiveState.Wait:

                                //TODO: Do nothing

                                break;

                            case ActiveState.Attack:
                                if (!isCount)
                                {
                                    if (turnCount1 > 0)
                                    {
                                        turnCount1--;
                                        parent.SendMessage(654, this);
                                    }
                                    if (turnCount2 > 0)
                                    {
                                        turnCount2--;
                                        parent.SendMessage(655, this);
                                    }
                                    if (turnCount3 > 0)
                                    {
                                        turnCount3--;
                                        parent.SendMessage(656, this);
                                    }
                                    isCount = true;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Down"]) && !isWait)
                                {
                                    parent.SendMessage(800, this);
                                    angle++;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Up"]) && !isWait)
                                {
                                    parent.SendMessage(801, this);
                                    angle--;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["Skill1"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["Skill1"]) && skillAvailable[0] && !skillSelected && turnCount1 == 0)
                                {
                                    currentSkill = Skill.Linear;
                                    parent.SendMessage(400, this);
                                    Console.WriteLine("skill1");
                                    skillSelected = true;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["Skill2"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["Skill2"]) && skillAvailable[1] && !skillSelected && turnCount2 == 0)
                                {
                                    currentSkill = Skill.LinearPenetrate;
                                    parent.SendMessage(401, this);
                                    Console.WriteLine("skill2");
                                    skillSelected = true;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["Skill3"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["Skill3"]) && skillAvailable[2] && !skillSelected && turnCount3 == 0)
                                {
                                    currentSkill = Skill.DoubleShot;
                                    parent.SendMessage(403, this);
                                    Console.WriteLine("skill3");
                                    skillSelected = true;
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["1"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["1"]) && Singleton.Instance.PlayerItem[0].Qty > 0)
                                {
                                    if (Singleton.Instance.PlayerItem[0].Name.Equals("Potion"))
                                    {
                                        if (health <= 100)
                                        {
                                            health += 10;
                                            parent.SendMessage(854, this);
                                        }
                                        else
                                        {
                                            health = 100;
                                            parent.SendMessage(855, this);
                                        }
                                        Singleton.Instance.PlayerItem[0].Qty--;
                                    }
                                    else if (Singleton.Instance.PlayerItem[0].Name.Equals("DoubleDamage"))
                                    {
                                        Singleton.Instance.Damage = Singleton.Instance.Damage * 2;
                                        Singleton.Instance.PlayerItem[0].Qty--;
                                    }
                                }

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["2"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["2"]) && Singleton.Instance.PlayerItem[1].Qty > 0)
                                {
                                    if (Singleton.Instance.PlayerItem[1].Name.Equals("Potion"))
                                    {
                                        if (health <= 100)
                                        {
                                            health += 10;
                                            parent.SendMessage(854, this);
                                        }
                                        else
                                        {
                                            health = 100;
                                            parent.SendMessage(855, this);
                                        }
                                        Singleton.Instance.PlayerItem[1].Qty--;
                                    }
                                    else if (Singleton.Instance.PlayerItem[1].Name.Equals("DoubleDamage"))
                                    {
                                        Singleton.Instance.Damage = Singleton.Instance.Damage * 2;
                                        Singleton.Instance.PlayerItem[1].Qty--;
                                    }
                                }

                                angle = MathHelper.Clamp(angle, 15, 90);
                                if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["SpaceBar"]))
                                {
                                    parent.SendMessage(900, this);
                                }
                                else
                                {
                                    parent.SendMessage(901, this);
                                }

                                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (timer > 10)
                                {
                                    timer = 0;
                                }

                                if (countshot == 1 && timer > 1.5)
                                {
                                    newBullet = catapultBullet.Clone() as GameObject;
                                    newBullet.Name = "Bullet";
                                    newBullet.ShootAngle = angle;
                                    newBullet.ShootPower = parent.ShootPower * 3;
                                    newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                                    newBullet.Position = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                                    newBullet.Reset();
                                    gameObjects.Add(newBullet);
                                    countshot = 0;

                                    currentSkill = Skill.Normal;
                                    parent.SendMessage(402, this);

                                    finishDoubleShoot = true;
                                    parent.SendMessage(500, this);
                                }

                                if (timer > 5 && finishDoubleShoot)
                                {
                                    finishDoubleShoot = false;
                                    isWait = false;
                                    timer = 0;

                                    currentState = ActiveState.WaitForWait;
                                    isWaitState = false;
                                }

                                //Perk1 Decrease Cooldown for 1 turn
                                if (Perk1)
                                {
                                    Singleton.Instance.Perk1DecreaseCooldown = 1;
                                    Perk1 = !Perk1;
                                }

                                //Perk2 Increase Damage 30 percent
                                if (Perk2)
                                {
                                    Singleton.Instance.Damage = Singleton.Instance.Damage + Singleton.Instance.Damage * 0.3f;
                                    Perk2 = !Perk2;
                                }

                                //Perk3 

                                if (Singleton.Instance.CurrentKey.IsKeyUp(InputList["SpaceBar"]) && Singleton.Instance.PreviousKey.IsKeyDown(InputList["SpaceBar"]) && !isWait)
                                {
                                    newBullet = catapultBullet.Clone() as GameObject;
                                    switch (currentSkill)
                                    {
                                        case Skill.Normal:
                                            newBullet = catapultBullet.Clone() as GameObject;
                                            newBullet.Name = "Bullet";
                                            newBullet.ShootAngle = angle;
                                            newBullet.ShootPower = parent.ShootPower * 3;
                                            newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                                            newBullet.Position = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                                            newBullet.Reset();
                                            gameObjects.Add(newBullet);
                                            break;

                                        case Skill.Linear:
                                            newBullet = linearBullet.Clone() as GameObject;
                                            newBullet.Name = "Bullet";
                                            newBullet.ShootAngle = angle;
                                            newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                                            newBullet.Position = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                                            newBullet.Reset();
                                            gameObjects.Add(newBullet);
                                            turnCount1 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
                                            parent.SendMessage(754, this);
                                            currentSkill = Skill.Normal;
                                            parent.SendMessage(402, this);
                                            skillSelected = false;
                                            break;

                                        case Skill.LinearPenetrate:
                                            newBullet = linearBullet.Clone() as GameObject;
                                            newBullet.Name = "PenetrateBullet";
                                            newBullet.ShootAngle = angle;
                                            newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                                            newBullet.Position = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                                            newBullet.Reset();
                                            gameObjects.Add(newBullet);
                                            turnCount2 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
                                            parent.SendMessage(755, this);
                                            currentSkill = Skill.Normal;
                                            parent.SendMessage(402, this);
                                            skillSelected = false;
                                            break;

                                        case Skill.DoubleShot:
                                            isWaitState = true;

                                            newBullet = catapultBullet.Clone() as GameObject;
                                            newBullet.Name = "Bullet";
                                            newBullet.ShootAngle = angle;
                                            newBullet.ShootPower = parent.ShootPower * 3;
                                            newBullet.Viewport = new Rectangle(0, 0, 28, 28);
                                            newBullet.Position = new Vector2((parent.Position.X + parent.Viewport.Width / 2 - 32), (parent.Position.Y + parent.Viewport.Height / 2 - 32));
                                            newBullet.Reset();
                                            gameObjects.Add(newBullet);
                                            turnCount3 = 3 - Singleton.Instance.Perk1DecreaseCooldown;
                                            parent.SendMessage(756, this);
                                            timer = 0;
                                            countshot++;
                                            isWait = true;
                                            skillSelected = false;
                                            break;
                                    }
                                    if (!isWaitState)
                                    {
                                        currentState = ActiveState.WaitForWait;
                                    }
                                }

                                break;

                            case ActiveState.WaitForWait:

                                if (!newBullet.IsActive)
                                {
                                    if (!TimerHelper)
                                    {
                                        curTime = gameTime.TotalGameTime.TotalSeconds;
                                        TimerHelper = true;
                                    }

                                    if (gameTime.TotalGameTime.TotalSeconds - curTime >= 0.5f)
                                    {
                                        currentState = ActiveState.Wait;
                                        TimerHelper = false;
                                    }
                                    parent.SendMessage(500, this);
                                    isCount = false;
                                }

                                break;

                            case ActiveState.Died:



                                break;
                        }
                        break;

                    case Singleton.ControlState.ShopState:
                        parent.Velocity = Vector2.Zero;
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
                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Down"]) &&
                            Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                        {
                            parent.SendMessage(302, this);
                            moveSelectorY += 150;
                        }

                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Up"]) &&
                            Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                        {
                            parent.SendMessage(303, this);
                            moveSelectorY -= 150;
                        }

                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Right"]) &&
                            Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                        {
                            parent.SendMessage(300, this);
                            moveSelectorX += 150;
                        }
                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["Left"]) &&
                            Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                        {
                            parent.SendMessage(301, this);
                            moveSelectorX -= 150;
                        }
                        if (Singleton.Instance.CurrentKey.IsKeyDown(InputList["SpaceBar"]) &&
                           Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                        {
                            //row 1 slot
                            if (moveSelectorX == 0 && moveSelectorY == 0)
                            {
                                Console.WriteLine("Selected1");
                            }
                            if (moveSelectorX == 150 && moveSelectorY == 0)
                            {
                                Console.WriteLine("Selected2");
                            }
                            if (moveSelectorX == 300 && moveSelectorY == 0)
                            {
                                Console.WriteLine("Selected3");
                            }
                            if (moveSelectorX == 450 && moveSelectorY == 0)
                            {
                                Console.WriteLine("Selected4");
                            }

                            //row 2 slot
                            if (moveSelectorX == 0 && moveSelectorY == 150)
                            {
                                Console.WriteLine("Selected5");
                            }
                            if (moveSelectorX == 150 && moveSelectorY == 150)
                            {
                                Console.WriteLine("Selected6");
                            }
                            if (moveSelectorX == 300 && moveSelectorY == 150)
                            {
                                Console.WriteLine("Selected7");
                            }
                            if (moveSelectorX == 450 && moveSelectorY == 150)
                            {
                                Console.WriteLine("Selected8");
                            }

                            //row 3 slot
                            if (moveSelectorX == 0 && moveSelectorY == 300)
                            {
                                Console.WriteLine("Selected9");
                            }
                            if (moveSelectorX == 150 && moveSelectorY == 300)
                            {
                                Console.WriteLine("Selected10");
                            }
                            if (moveSelectorX == 300 && moveSelectorY == 300)
                            {
                                Console.WriteLine("Selected11");
                            }
                            if (moveSelectorX == 450 && moveSelectorY == 300)
                            {
                                Console.WriteLine("Selected12");
                            }

                            if (moveSelectorY >= 305)
                            {
                                Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
                                parent.SendMessage(701, this);
                            }
                        }
                        break;
                }
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
            else if (message == 701)
            {
                //Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
            }
            else if (message == 702)
            {
                //Singleton.Instance.currentControlState = Singleton.ControlState.ShopState;
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

        }
    }
}
