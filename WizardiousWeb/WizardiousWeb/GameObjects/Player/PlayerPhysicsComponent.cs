using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WizardiousWeb
{
    public class PlayerPhysicsComponent : PhysicsComponent
    {
        List<GameObject> gameObjectss = Singleton.Instance.gameObjects;
        private float timer;
        private float prevData;
        private bool isTouchingShopKepper = false;
        private bool touch = false;
        private bool jump = true;
        private bool isFirstTurn = true;
        int i = 0;
        bool isActiveState = false;

        private float health = 100f;

        public enum Skill
        {
            Normal,
            Linear,
            LinearPenetrate
        }
        private Skill currentSkill;
        private bool[] skillAvailable = new bool[3];

        public PlayerPhysicsComponent(GameScene currentScene) : base(currentScene)
        {
            EntityPhysicsType = PhysicsType.STATICS;
            EntityBoundingBoxType = BoundingBoxType.AABB;
            EntityImpluseType = ImpluseType.NONE;

        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, GameObject parent)
        {
            isActiveState = false;

            foreach (GameObject gameObject in gameObjectss)
            {
                if (parent.IsActive && gameObject.Name.Equals("Floor"))
                {
                    if (IsTouchingTop(gameObject, parent))
                    {
                        parent.Velocity.Y = 0f;
                        jump = false;
                        parent.SendMessage(101, this);

                    }
                    else if (IsTouchingBottom(gameObject, parent))
                    {
                        parent.Velocity.Y = +1f;
                    }

                    if (IsTouchingLeft(gameObject, parent) && parent.Position.X < gameObject.Rectangle.Left - 25)
                    {
                        parent.Velocity.X = -1f;
                    }
                    else if (IsTouchingRight(gameObject, parent) && parent.Position.X > gameObject.Rectangle.Right + 25)
                    {
                        parent.Velocity.X = +1f;
                    }

                    if (parent.Position.X > gameObject.Rectangle.Right + 28.5 && IsTouchingTop(gameObject, parent))
                    {
                        jump = true;
                        parent.SendMessage(100, this);
                    }
                    if (parent.Position.X < gameObject.Rectangle.Left - 28.5 && IsTouchingTop(gameObject, parent))
                    {
                        jump = true;
                        parent.SendMessage(100, this);
                    }
                }

                //Health Decrease after hit bullet
                if (gameObject.Name.Equals("EnemyBullet") && gameObject.IsActive && this.IsTouching(gameObject))
                {
                    //TODO: Resolution after hit
                    gameObject.IsActive = false;
                    parent.SendMessage(200, this);
                    health -= 10;
                }

                if (parent.IsActive && IsTouchingLeft(gameObject, parent) && gameObject.Name.Equals("ActiveZone"))
                {
                    parent.Velocity.X = 0f;

                    if (parent.Velocity.Y == 0f && health > 0)
                    {
                        parent.SendMessage(551, this);
                        parent.SendMessage(552, this);
                        parent.SendMessage(553, this);
                        if (isFirstTurn)
                        {
                            prevData = Singleton.Instance.Damage;
                            isFirstTurn = false;
                        }

                        Console.WriteLine("health" + health);

                        if (health > 0)
                        {
                            Singleton.Instance.currentControlState = Singleton.ControlState.ActiveState;
                        }
                        else
                        {
                            Singleton.Instance.currentControlState = Singleton.ControlState.GameOver;
                        }

                    }

                    isActiveState = true;
                }

                if (parent.IsActive && IsTouching(gameObject) && gameObject.Name.Equals("ShopKeeper"))
                {
                    parent.SendMessage(902, this);
                }
                else if (IsTouching(gameObject) && !gameObject.Name.Equals("ShopKeeper"))
                {
                    parent.SendMessage(903, this);
                }

                if (parent.IsActive && IsTouching(gameObject) && gameObject.Name.Equals("Potion"))
                {
                    if (Singleton.Instance.PlayerItem[0] == null)
                    {
                        Singleton.Instance.PlayerItem[0] = gameObject;
                    }
                    else
                    {
                        Singleton.Instance.PlayerItem[0].Qty = Singleton.Instance.PlayerItem[0].Qty + gameObject.Qty;
                    }
                    gameObject.IsActive = false;
                }

                if (parent.IsActive && IsTouching(gameObject) && gameObject.Name.Equals("DoubleDamage"))
                {
                    if (Singleton.Instance.PlayerItem[1] == null)
                    {
                        Singleton.Instance.PlayerItem[1] = gameObject;
                    }
                    else
                    {
                        Singleton.Instance.PlayerItem[1].Qty = Singleton.Instance.PlayerItem[1].Qty + gameObject.Qty;
                    }
                    gameObject.IsActive = false;
                }
            }
            if (!isActiveState && (!Singleton.Instance.currentControlState.Equals(Singleton.ControlState.NormalState)
                                   && (!Singleton.Instance.currentControlState.Equals(Singleton.ControlState.ShopState))
                                   && (!Singleton.Instance.currentControlState.Equals(Singleton.ControlState.PauseState))
                                   && (!Singleton.Instance.currentControlState.Equals(Singleton.ControlState.GameOver))))
            {
                Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
                //Console.WriteLine(!Singleton.Instance.currentControlState.Equals(Singleton.ControlState.NormalState));
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
            else if (message == 700)
            {
                //Singleton.Instance.currentControlState = Singleton.ControlState.ActiveState;
            }
            else if (message == 701)
            {
                //Singleton.Instance.currentControlState = Singleton.ControlState.NormalState;
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
            else if (message == 200)
            {
                health -= 10;
            }
        }
    }
}
